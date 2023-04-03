using HarmonyLib;
using System;

namespace SmartWishbone.ServerSync
{
    [HarmonyPatch(typeof(ZNet))]
    internal class RPCHandler
    {
        private const string rpc_server_toggle = SmartWishbonePlugin.GUID + "ServerToggleRPC";

        [HarmonyPatch(nameof(ZNet.Awake)), HarmonyPostfix]
        private static void AwakePostfix(ZNet __instance)
        {
            if (__instance.IsServer())
            {
                WishboneConfig.SetConfigDataWithoutEvent(TrackableDataLoader.ParseCustomDictToString(TrackableData.Data));
                ZRoutedRpc.instance.Register(rpc_server_toggle, new Action<long, string>(ReceiveTrackableToggleRPC));
            }
        }

        internal static void SendTrackableToggleRPC(string trackableName)
        {
            if (!ZNet.instance || ZNet.instance.IsServer())
            {
                return;
            }

            Helper.Log($"Send new trackable toggle request for: {trackableName}");
            ZRoutedRpc.instance.InvokeRoutedRPC(rpc_server_toggle, new object[] { trackableName });
        }

        private static void ReceiveTrackableToggleRPC(long sender, string trackableName)
        {
            if (!ZNet.instance || !ZNet.instance.IsServer())
            {
                return;
            }

            var allowed = WishboneConfig.UsersAllowedToAddTrackables.Value;

            if (allowed == WishboneConfig.UserLevel.Noone)
            {
                return;
            }

            if (allowed == WishboneConfig.UserLevel.OnlyAdmins)
            {
                var peer = ZNet.instance.GetPeer(sender);

                if (peer == null || !ZNet.instance.ListContainsId(ZNet.instance.m_adminList, peer.m_socket.GetHostName()))
                {
                    Helper.Log($"Received trackable request for '{trackableName}' but sender is not an admin");
                    return;
                }
            }

            Helper.Log($"Received valid trackable request for '{trackableName}'");

            TrackableData.ToggleTarget(trackableName);
        }
    }
}