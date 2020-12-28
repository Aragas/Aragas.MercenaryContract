using System;
using System.Collections.Generic;

using TaleWorlds.CampaignSystem;

namespace Aragas
{
    internal class MapEventComparer : IEqualityComparer<MapEvent>
    {
        public bool Equals(MapEvent x, MapEvent y) => string.Equals(x.StringId, y.StringId, StringComparison.InvariantCulture);

        public int GetHashCode(MapEvent obj) => obj.StringId.GetHashCode();
    }
}