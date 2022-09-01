using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UXF;
using ViveSR.anipal.Eye;

namespace UXFTrackers
{
    public class EyeDataTracker : Tracker
    {
        private string pupilDialationHeaderLeft = "pupil_dialation_left";
        private string pupilDialationHeaderRight = "pupil_dialation_right";
        private string combinedGazeOriginHeaderX = "combined_gaze_origin_x";
        private string combinedGazeOriginHeaderY = "combined_gaze_origin_y";
        private string combinedGazeOriginHeaderZ = "combined_gaze_origin_z";
        private string combinedGazeDirectionHeaderX = "combined_gaze_direction_x";
        private string combinedGazeDirectionHeaderY = "combined_gaze_direction_y";
        private string combinedGazeDirectionHeaderZ = "combined_gaze_direction_z";

        public override string MeasurementDescriptor => "eyedata";
        private static EyeData_v2 eyeData = new EyeData_v2();
        private bool eye_callback_registered = false;

        public override IEnumerable<string> CustomHeader => new string[] { pupilDialationHeaderLeft, pupilDialationHeaderRight, combinedGazeOriginHeaderX, combinedGazeOriginHeaderY, combinedGazeOriginHeaderZ, combinedGazeDirectionHeaderX, combinedGazeDirectionHeaderY, combinedGazeDirectionHeaderZ };

        private void Start()
        {
            if (!SRanipal_Eye_Framework.Instance.EnableEye)
            {
                enabled = false;
                return;
            }
        }

        protected override UXFDataRow GetCurrentValues()
        {
            if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
            SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return // UXF adds time column each time so cannot be reused
                    new UXFDataRow()
                    {
                        (pupilDialationHeaderLeft, 0),
                        (pupilDialationHeaderRight, 0),
                        (combinedGazeOriginHeaderX, 0),
                        (combinedGazeOriginHeaderY, 0),
                        (combinedGazeOriginHeaderZ, 0),
                        (combinedGazeDirectionHeaderX, 0),
                        (combinedGazeDirectionHeaderY, 0),
                        (combinedGazeDirectionHeaderZ, 0)
                    };

            if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
            {
                SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = true;
            }
            else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
            {
                SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }

            Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;

            // TODO Fix return
            if (eye_callback_registered)
            {
                if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                else return new UXFDataRow()
                {
                    (pupilDialationHeaderLeft, 0),
                    (pupilDialationHeaderRight, 0),
                    (combinedGazeOriginHeaderX, 0),
                    (combinedGazeOriginHeaderY, 0),
                    (combinedGazeOriginHeaderZ, 0),
                    (combinedGazeDirectionHeaderX, 0),
                    (combinedGazeDirectionHeaderY, 0),
                    (combinedGazeDirectionHeaderZ, 0)
                };
            }
            else
            {
                if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                else if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                else return new UXFDataRow()
                {
                    (pupilDialationHeaderLeft, 0),
                    (pupilDialationHeaderRight, 0),
                    (combinedGazeOriginHeaderX, 0),
                    (combinedGazeOriginHeaderY, 0),
                    (combinedGazeOriginHeaderZ, 0),
                    (combinedGazeDirectionHeaderX, 0),
                    (combinedGazeDirectionHeaderY, 0),
                    (combinedGazeDirectionHeaderZ, 0)
                };
            }

            // SRanipal_Eye.Focus(GazeIndex.COMBINE, out var r, out var f);

            //Vector3 GazeDirectionCombined = Camera.main.transform.TransformDirection(GazeDirectionCombinedLocal);

            var values = new UXFDataRow()
            {
                (pupilDialationHeaderLeft, eyeData.verbose_data.left.pupil_diameter_mm),
                (pupilDialationHeaderRight, eyeData.verbose_data.right.pupil_diameter_mm),
                (combinedGazeOriginHeaderX, GazeOriginCombinedLocal.x),
                (combinedGazeOriginHeaderY, GazeOriginCombinedLocal.y),
                (combinedGazeOriginHeaderZ, GazeOriginCombinedLocal.z),
                (combinedGazeDirectionHeaderX, GazeOriginCombinedLocal.x),
                (combinedGazeDirectionHeaderY, GazeOriginCombinedLocal.y),
                (combinedGazeDirectionHeaderZ, GazeOriginCombinedLocal.z)
            };

            return values;
        }

        private void Release()
        {
            if (eye_callback_registered == true)
            {
                SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
                eye_callback_registered = false;
            }
        }
        private static void EyeCallback(ref EyeData_v2 eye_data)
        {
            eyeData = eye_data;
        }
    }
}