using System;
using System.Collections.Generic;

namespace Minimap.Data
{
    [Serializable]
    public class MinimapDataEntry
    {
        public float Timestamp { get; set; }
        public MinimapHmd Hmd { get; set; } = new();

        public List<MinimapController> Controllers { get; set; } = new();

        public List<MinimapObject> Objects { get; set; } = new();
    }
}
