using System.Collections.Generic;
using Experiment;
using UnityEngine;
using UXF;

namespace UXFTrackers
{
    [RequireComponent(typeof(InteractableSphere))]
    public class SphereParameterTracker: Tracker
    {
        private const string visibleString = "visible";
        private const string highlightedString = "highlighted";
        private InteractableSphere InteractableSphere;

        private void Start()
        {
            InteractableSphere = GetComponent<InteractableSphere>();
        }
        #region Overrides of Tracker

        public override string MeasurementDescriptor => "parameters";
        public override IEnumerable<string> CustomHeader => new string[] { visibleString, highlightedString };

        /// <summary>
        /// Returns current position and rotation values
        /// </summary>
        /// <returns></returns>
        protected override UXFDataRow GetCurrentValues()
        {
            // get position and rotation
            var visible = InteractableSphere.Visible;
            var highlighted = InteractableSphere.Highlighted;
            
            var values = new UXFDataRow()
            {
                (visibleString, visible),
                (highlightedString, highlighted)
            };

            return values;
        }

        #endregion
    }
}
