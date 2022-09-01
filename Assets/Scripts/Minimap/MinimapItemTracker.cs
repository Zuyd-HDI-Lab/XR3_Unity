using Experiment;
using Minimap.Data;
using UnityEngine;

namespace Minimap
{
    public class MinimapItemTracker : MonoBehaviour
    {
        [SerializeField]
        private MinimapItemType MinimapItemType;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public IMinimapItem GetCurrentData()
        {
            // TODO Revisit this, especially spheres
            IMinimapItem item;
            var p = transform.position;
            var r = transform.eulerAngles;

            switch (MinimapItemType)
            {
                case MinimapItemType.HMD:
                    item = new MinimapHmd();
                    break;
                case MinimapItemType.LeftController:
                    item = new MinimapController
                    {
                        Side = MinimapControllerSide.Left,
                    };
                    break;
                case MinimapItemType.RightController:
                    item = new MinimapController
                    {
                        Side= MinimapControllerSide.Right,  
                    };
                    break;
                case MinimapItemType.Object:
                default:
                    var sphere = GetComponent<InteractableSphere>();
                    item = new MinimapObject
                    {
                        Id = sphere.Id,
                        Visible = sphere.Visible
                    };
                    break;
            }

            item.MinimapItemType = MinimapItemType;
            item.Position.X = p.x;
            item.Position.Y = p.z; // Y is up, so use Z for 2D Y position
            item.Rotation = r.y * -1;

            return item;
        }
    }
}
