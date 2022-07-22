using System.Linq;
using Minimap.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Minimap
{
    public class MinimapRenderer : MonoBehaviour
    {
        public GameObject minimapItemPrefab;

        public GameObject HmdItem;
        public GameObject LeftItem;
        public GameObject RightItem;

        public GameObject S0;
        public GameObject S1;
        public GameObject S2;
        public GameObject S3;
        public GameObject S4;
        public GameObject S5;
        public GameObject S6;
        public GameObject S7;

        private float scalefactor = 100f/3f;
        // private float rotationFlip = -1f;

        // Start is called before the first frame update
        void Start()
        {
            HmdItem = Instantiate(minimapItemPrefab, transform);
            HmdItem.name = "HMD";

            LeftItem = Instantiate(minimapItemPrefab, transform);
            LeftItem.name = "Left Controller";

            RightItem = Instantiate(minimapItemPrefab, transform);
            RightItem.name = "Right Controller";

            S0 = Instantiate(minimapItemPrefab, transform);
            S0.name = "S 0";

            S1 = Instantiate(minimapItemPrefab, transform);
            S1.name = "S 1";

            S2 = Instantiate(minimapItemPrefab, transform);
            S2.name = "S 2";

            S3 = Instantiate(minimapItemPrefab, transform);
            S3.name = "S 3";

            S4 = Instantiate(minimapItemPrefab, transform);
            S4.name = "S 4";

            S5 = Instantiate(minimapItemPrefab, transform);
            S5.name = "S 5";

            S6 = Instantiate(minimapItemPrefab, transform);
            S6.name = "S 6";

            S7 = Instantiate(minimapItemPrefab, transform);
            S7.name = "S 7";
        }

        public void UpdateMinimap(MinimapDataEntry entry)
        {
            var hmdImage = HmdItem.GetComponent<Image>();

            hmdImage.rectTransform.anchoredPosition = new Vector2(entry.Hmd.Position.X * scalefactor, entry.Hmd.Position.Y * scalefactor);
            hmdImage.rectTransform.eulerAngles = new Vector3(0,0,entry.Hmd.Rotation);

            var leftImage = LeftItem.GetComponent<Image>();
            leftImage.color = Color.blue;
            var leftControllerData = entry.Controllers.FirstOrDefault(c => c.Side == MinimapControllerSide.Left);
            leftImage.rectTransform.anchoredPosition = new Vector2(leftControllerData.Position.X * scalefactor, leftControllerData.Position.Y * scalefactor);
            leftImage.rectTransform.eulerAngles = new Vector3(0, 0, leftControllerData.Rotation);

            var rightImage = RightItem.GetComponent<Image>();
            rightImage.color = Color.cyan;
            var rightControllerData = entry.Controllers.FirstOrDefault(c => c.Side == MinimapControllerSide.Right);
            rightImage.rectTransform.anchoredPosition = new Vector2(rightControllerData.Position.X * scalefactor, rightControllerData.Position.Y * scalefactor);
            rightImage.rectTransform.eulerAngles = new Vector3(0, 0, rightControllerData.Rotation);

            var s0Image = S0.GetComponent<Image>();
            s0Image.color = Color.green;
            var s0Data = entry.Objects[0];
            s0Image.rectTransform.anchoredPosition = new Vector2(s0Data.Position.X * scalefactor, s0Data.Position.Y * scalefactor);
            s0Image.rectTransform.eulerAngles = new Vector3(0, 0, s0Data.Rotation);

            var s1Image = S1.GetComponent<Image>();
            s1Image.color = Color.green;
            var s1Data = entry.Objects[1];
            s1Image.rectTransform.anchoredPosition = new Vector2(s1Data.Position.X * scalefactor, s1Data.Position.Y * scalefactor);
            s1Image.rectTransform.eulerAngles = new Vector3(0, 0, s1Data.Rotation);

            var s2Image = S2.GetComponent<Image>();
            s2Image.color = Color.green;
            var s2Data = entry.Objects[2];
            s2Image.rectTransform.anchoredPosition = new Vector2(s2Data.Position.X * scalefactor, s2Data.Position.Y * scalefactor);
            s2Image.rectTransform.eulerAngles = new Vector3(0, 0, s2Data.Rotation);

            var s3Image = S3.GetComponent<Image>();
            s3Image.color = Color.green;
            var s3Data = entry.Objects[3];
            s3Image.rectTransform.anchoredPosition = new Vector2(s3Data.Position.X * scalefactor, s3Data.Position.Y * scalefactor);
            s3Image.rectTransform.eulerAngles = new Vector3(0, 0, s3Data.Rotation);

            var s4Image = S4.GetComponent<Image>();
            s4Image.color = Color.green;
            var s4Data = entry.Objects[4];
            s4Image.rectTransform.anchoredPosition = new Vector2(s4Data.Position.X * scalefactor, s4Data.Position.Y * scalefactor);
            s4Image.rectTransform.eulerAngles = new Vector3(0, 0, s4Data.Rotation);

            var s5Image = S5.GetComponent<Image>();
            s5Image.color = Color.green;
            var s5Data = entry.Objects[5];
            s5Image.rectTransform.anchoredPosition = new Vector2(s5Data.Position.X * scalefactor, s5Data.Position.Y * scalefactor);
            s5Image.rectTransform.eulerAngles = new Vector3(0, 0, s5Data.Rotation);

            var s6Image = S6.GetComponent<Image>();
            s6Image.color = Color.green;
            var s6Data = entry.Objects[6];
            s6Image.rectTransform.anchoredPosition = new Vector2(s6Data.Position.X * scalefactor, s6Data.Position.Y * scalefactor);
            s6Image.rectTransform.eulerAngles = new Vector3(0, 0, s6Data.Rotation);

            var s7Image = S7.GetComponent<Image>();
            s7Image.color = Color.green;
            var s7Data = entry.Objects[7];
            s7Image.rectTransform.anchoredPosition = new Vector2(s7Data.Position.X * scalefactor, s7Data.Position.Y * scalefactor);
            s7Image.rectTransform.eulerAngles = new Vector3(0, 0, s7Data.Rotation);
        }   

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
