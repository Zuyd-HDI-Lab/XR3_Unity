using System;
using System.Collections.Generic;
using System.IO;
using Constants;
using Experiment;
using Minimap.Data;
using Newtonsoft.Json;
using UnityEngine;
using UXF;

namespace Minimap
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField]
        private List<PositionRotationTracker> minimapObjects = new();

        private MinimapItemTracker[] minimapItemTrackers;

        private MinimapData _minimapData = new();

        public Transform center;

        public MinimapRenderer MinimapRenderer;

        // Start is called before the first frame update
        void Start()
        {
            minimapItemTrackers = FindObjectsOfType<MinimapItemTracker>(true);
            Session.instance.onSessionEnd?.AddListener(OnSessionEnd);
        }

        private void OnSessionEnd(Session arg0)
        {
            Save();
        }

        // Update is called once per frame
        void Update()
        {
            var minimapEntry = new MinimapDataEntry();
            minimapEntry.Timestamp = Time.time;

            foreach(var minimapItemTracker in minimapItemTrackers)
            {
                var data = minimapItemTracker.GetCurrentData();

                switch (data.MinimapItemType)
                {
                    case MinimapItemType.HMD:
                        minimapEntry.Hmd = data as MinimapHmd;
                        break;
                    case MinimapItemType.LeftController:
                        minimapEntry.Controllers.Add(data as MinimapController);
                        break;
                    case MinimapItemType.RightController:
                        minimapEntry.Controllers.Add(data as MinimapController);
                        break;
                    case MinimapItemType.Object:
                        minimapEntry.Objects.Add(data as MinimapObject);
                        break;
                }
            }

            /* foreach (var positionRotationTracker in minimapObjects)
         {
             var p = positionRotationTracker.transform.position;
             var r = positionRotationTracker.transform.eulerAngles;

             switch (positionRotationTracker.name)
             {
                 case "VRCamera":
                     minimapEntry.Hmd.Position.X = p.x;
                     minimapEntry.Hmd.Position.Y = p.z;
                     minimapEntry.Hmd.Rotation = r.y;
                     break;
                 case "LeftHand":
                 case "RightHand":
                     var leftController = new MinimapController
                     {
                         Side = positionRotationTracker.name == "LeftHand" ? MinimapControllerSide.Left : MinimapControllerSide.Right,
                         Rotation = r.y,
                         Position = new MinimapPosition
                         {
                             X = p.x,
                             Y = p.z,
                         }
                     };
                     minimapEntry.Controllers.Add(leftController);
                     break;                
             }

             if (positionRotationTracker.name.Contains("Sphere"))
             {
                 var sphere = positionRotationTracker.gameObject.GetComponent<InteractableSphere>();
                     var sphereObject = new MinimapObject
                     {
                         Id = sphere.Id,
                         Visible = sphere.Visible,
                         Rotation = r.y,
                         Position = new MinimapPosition
                         {
                             X = p.x,
                             Y = p.z,
                         }
                     };
                 minimapEntry.Objects.Add(sphereObject);
             }
         }*/

            _minimapData.Entries.Add(minimapEntry);
            MinimapRenderer.UpdateMinimap(minimapEntry);
        }

        /// <summary>
        /// Save minimap data
        /// </summary>
        public void Save()
        {
            var experiment = GameObject.Find(GameObjectNames.Experiment).GetComponent<IExperiment>();

            // TODO fix and try to integrate in UXF, check if enum as string works
            var text = JsonConvert.SerializeObject(_minimapData, new Newtonsoft.Json.Converters.StringEnumConverter());
            //var text = MiniJSON.Json.Serialize(_minimapData);
            var dir = Path.Combine(experiment.OutputLocation, "Minimap");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            var fileName = $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.json";
            
            var savePath = Path.Combine(dir, fileName);

            File.WriteAllText(savePath, text);
        }
    }
}