﻿using UnityEngine;

namespace SmartWishbone
{
    internal class Interactions
    {
        internal static void TrackObject(Player player)
        {
            GameObject hoverObject = player.GetHoverObject();

            if (!hoverObject)
            {
                return;
            }

            GameObject hoverRoot = hoverObject.transform.root.gameObject;

            if (!hoverRoot)
            {
                return;
            }

            Destructible destructible = hoverRoot.GetComponent<Destructible>();

            if (!destructible || !destructible.gameObject)
            {
                Helper.LogWarning("Invalid hover object: not destructible");
                return;
            }

            string prefabName;

            var child = hoverRoot.GetComponentInChildren<Beacon>();

            if (child)
            {
                prefabName = Utils.GetPrefabName(child.gameObject);
            }
            else
            {
                prefabName = Utils.GetPrefabName(hoverRoot.gameObject);
            }

            Helper.Log($"Valid hover object: {prefabName}");

            TrackableData.ToggleTarget(prefabName);
        }
    }
}