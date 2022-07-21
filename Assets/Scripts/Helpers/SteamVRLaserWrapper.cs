using UnityEngine;

namespace Helpers
{
    // TODO MOVE TO MRTK, create new laser pointer system
    /*[RequireComponent(typeof(SteamVR_LaserPointer))]
    public class SteamVRLaserWrapper : MonoBehaviour
    {
        private SteamVR_LaserPointer steamVrLaserPointer;
    
        private void Awake()
        {
            steamVrLaserPointer = gameObject.GetComponent<SteamVR_LaserPointer>();
            steamVrLaserPointer.PointerIn += OnPointerIn;
            steamVrLaserPointer.PointerOut += OnPointerOut;
            steamVrLaserPointer.PointerClick += OnPointerClick;
        }

        private void OnPointerClick(object sender, PointerEventArgs e)
        {
            Debug.Log($"OnPointerClick {e.target.name}");
            var laserHandler = e.target.GetComponent<ILaserInteraction>();
            if (laserHandler == null)
            {
                return;
            }

            laserHandler.OnLaserClick(new LaserEventArgs { SteamVRInputSource = e.fromInputSource });
        }

        private void OnPointerOut(object sender, PointerEventArgs e)
        {
            var laserHandler = e.target.GetComponent<ILaserInteraction>();
            if (laserHandler == null)
            {
                return;
            }

            laserHandler.OnLaserOut(new LaserEventArgs { SteamVRInputSource = e.fromInputSource });
        }

        private void OnPointerIn(object sender, PointerEventArgs e)
        {
            var laserHandler = e.target.GetComponent<ILaserInteraction>();
            if (laserHandler == null)
            {
                return;
            }

            laserHandler.OnLaserIn(new LaserEventArgs { SteamVRInputSource = e.fromInputSource });
        }
    }*/
}