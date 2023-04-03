using System.Collections.Generic;
using UnityEngine;

namespace SmartWishbone
{
    internal class Target
    {
        public string targetName;
        public ItemDrop.ItemData.SharedData optionalItem;
        public string displayName;
        public Sprite displaySprite;
        public Dictionary<string, Trackable> possibleTargets;

        public Target(string targetName, Dictionary<string, Trackable> possibleTargets)
        {
            this.targetName = targetName;
            this.possibleTargets = possibleTargets;
        }
    }
}