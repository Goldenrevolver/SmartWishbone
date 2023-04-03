using System;
using UnityEngine;

namespace SmartWishbone
{
    public static class Helper
    {
        internal static void Log(object s)
        {
            if (!WishboneConfig.EnableDebugLogs.Value)
            {
                return;
            }

            var toPrint = $"{SmartWishbonePlugin.NAME} {SmartWishbonePlugin.VERSION}: {(s != null ? s.ToString() : "null")}";

            Debug.Log(toPrint);
        }

        internal static void LogWarning(object s)
        {
            var toPrint = $"{SmartWishbonePlugin.NAME} {SmartWishbonePlugin.VERSION}: {(s != null ? s.ToString() : "null")}";

            Debug.LogWarning(toPrint);
        }

        internal static void LogError(Exception e)
        {
            var toPrint = $"{SmartWishbonePlugin.NAME} {SmartWishbonePlugin.VERSION}: {(e != null ? e.Message + '\n' + e.StackTrace : "null")}";

            Debug.LogError(toPrint);
        }
    }
}