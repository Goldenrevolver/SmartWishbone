using HarmonyLib;
using System.Linq;

namespace SmartWishbone
{
    [HarmonyPatch]
    internal class BeaconAddPatches
    {
        [HarmonyPatch(typeof(Destructible), nameof(Destructible.Awake)), HarmonyPostfix]
        public static void Awake_Postfix(Destructible __instance)
        {
            if (TrackableData.Data.Keys.Contains(Utils.GetPrefabName(__instance.gameObject)))
            {
                TryAddBeacon(__instance);
            }
        }

        public static void TryAddBeacon(Destructible destructible)
        {
            var child = destructible.gameObject.GetComponentInChildren<Beacon>();
            var parent = destructible.gameObject.GetComponentInParent<Beacon>();

            if (!child && !parent)
            {
                destructible.gameObject.AddComponent<Beacon>();
            }
        }
    }
}