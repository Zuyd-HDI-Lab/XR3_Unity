using System;
using System.Collections.Generic;

namespace Minimap.Data
{
    [Serializable]
    public class MinimapData
    {
        public List<MinimapDataEntry> Entries { get; set; } = new();
    }
}
