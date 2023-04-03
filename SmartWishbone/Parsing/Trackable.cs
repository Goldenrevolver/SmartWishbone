using BepInEx;

namespace SmartWishbone
{
    internal class Trackable
    {
        public string prefabName;
        public string displayItem;
        public string condition;
        public double range = 20;

        public Trackable()
        {
        }

        public Trackable(string prefabName, string displayItem) : this()
        {
            this.prefabName = prefabName;
            this.displayItem = displayItem;
        }

        public Trackable(string prefabName, string displayItem, string condition, double range) : this(prefabName, displayItem)
        {
            this.condition = condition;
            this.range = range;
        }
    }

    internal static class TrackableExtension
    {
        internal static bool FulFillsCondition(this Trackable trackable)
        {
            if (!WishboneConfig.EnforceWorldConditions.Value)
            {
                return true;
            }

            return trackable.condition.IsNullOrWhiteSpace() || ZoneSystem.instance.GetGlobalKey(trackable.condition);
        }
    }
}