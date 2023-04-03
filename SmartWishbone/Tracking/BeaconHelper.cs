using System.Collections.Generic;
using UnityEngine;

namespace SmartWishbone
{
    internal static class BeaconHelper
    {
        public static Beacon FindClosestBeaconInRange(Vector3 point, Target target)
        {
            Beacon closestBeacon = null;
            float closestBeaconRange = 999999f;

            Dictionary<string, Trackable> targetList = target != null ? target.possibleTargets : TrackableData.Data;

            foreach (Beacon thisBeacon in Beacon.m_instances)
            {
                if (!thisBeacon || !thisBeacon.gameObject)
                {
                    continue;
                }

                double range = 20f;

                if (!targetList.TryGetValue(Utils.GetPrefabName(thisBeacon.gameObject), out var targetInstance))
                {
                    continue;
                }
                else
                {
                    if (!targetInstance.FulFillsCondition())
                    {
                        continue;
                    }

                    range = targetInstance.range;
                }

                if (WishboneConfig.SearchDistanceOverride.Value != 0f)
                {
                    if (WishboneConfig.SearchDistanceOverrideStyle.Value == WishboneConfig.RangeStyle.SetTo)
                    {
                        range = WishboneConfig.SearchDistanceOverride.Value;
                    }
                    else
                    {
                        range += WishboneConfig.SearchDistanceOverride.Value;
                    }
                }

                float thisRange = Vector3.Distance(point, thisBeacon.transform.position);

                if (thisRange < range && (closestBeacon == null || thisRange < closestBeaconRange))
                {
                    closestBeacon = thisBeacon;
                    closestBeaconRange = thisRange;
                }
            }

            return closestBeacon;
        }
    }
}