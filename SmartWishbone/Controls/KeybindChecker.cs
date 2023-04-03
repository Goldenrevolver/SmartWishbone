using BepInEx.Configuration;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace SmartWishbone
{
    [HarmonyPatch]
    internal static class KeybindChecker
    {
        public static bool IgnoreKeyPresses()
        {
            return IgnoreKeyPressesDueToPlayer(Player.m_localPlayer)
                || !ZNetScene.instance
                || Minimap.IsOpen()
                || Menu.IsVisible()
                || Console.IsVisible()
                || StoreGui.IsVisible()
                || TextInput.IsVisible()
                || InventoryGui.IsVisible()
                || (Chat.instance && Chat.instance.HasFocus())
                || (ZNet.instance && ZNet.instance.InPasswordDialog())
                || (TextViewer.instance && TextViewer.instance.IsVisible());
        }

        private static bool IgnoreKeyPressesDueToPlayer(Player player)
        {
            return !player
                || player.InCutscene()
                || player.IsTeleporting()
                || player.IsDead()
                || player.InPlaceMode();
        }

        // thank you to 'Margmas' for giving me this snippet from VNEI https://github.com/MSchmoecker/VNEI/blob/master/VNEI/Logic/BepInExExtensions.cs#L21
        // since KeyboardShortcut.IsPressed and KeyboardShortcut.IsDown behave unintuitively
        public static bool IsKeyDown(this KeyboardShortcut shortcut)
        {
            return shortcut.MainKey != KeyCode.None && Input.GetKeyDown(shortcut.MainKey) && shortcut.Modifiers.All(Input.GetKey);
        }

        public static bool IsKeyHeld(this KeyboardShortcut shortcut)
        {
            return shortcut.MainKey != KeyCode.None && Input.GetKey(shortcut.MainKey) && shortcut.Modifiers.All(Input.GetKey);
        }

        [HarmonyPatch(typeof(Player), nameof(Player.Update))]
        public static class Player_Update_Patch
        {
            internal static void Prefix(Player __instance)
            {
                if (Player.m_localPlayer != __instance)
                {
                    return;
                }

                if (IgnoreKeyPresses())
                {
                    return;
                }

                if (WishboneConfig.NextTargetKey.Value.IsKeyDown())
                {
                    TrackableSwitcher.SwitchTarget(true);
                }
                else if (WishboneConfig.PreviousTargetKey.Value.IsKeyDown())
                {
                    TrackableSwitcher.SwitchTarget(false);
                }
                else if (WishboneConfig.ToggleTrackingObjectKey.Value.IsKeyDown())
                {
                    Interactions.TrackObject(__instance);
                }
            }
        }
    }
}