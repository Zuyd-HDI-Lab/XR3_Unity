using System.Collections;
using Helpers;
using UnityEngine;
using Valve.VR;

namespace Experiment
{
    public class InteractableSphere : MonoBehaviour, ILaserInteraction
    {
        private SteamVR_Action_Vibration hapticAction;
    

        public bool Visible;
        public bool Highlighted;
        public bool Target;
        public int Id;

        [SerializeField]
        private Material normalMaterial;
        [SerializeField]
        private Material highlightedMaterial;
        [SerializeField]
        private int audioDuration = 1;

        public AudioSource audioSource;
        private MeshRenderer meshRenderer;
        private SphereController sphereController;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            meshRenderer = GetComponent<MeshRenderer>();
            sphereController = GetComponentInParent<SphereController>();

            hapticAction = SteamVR_Actions.default_Haptic;
        }

        private void Selected()
        {
            sphereController.SphereSelected(this);
        }

        public void Setup()
        {
            meshRenderer.enabled = Visible;
            meshRenderer.material = (Highlighted && Target) ? highlightedMaterial : normalMaterial;

            if (!Target) return;

            StartCoroutine(StopAudio());
            audioSource.Play();
        }


        #region Implementation of IPointerClickHandler

        /// <inheritdoc />
        public void OnLaserClick(LaserEventArgs laserEventArgs)
        {
            hapticAction.Execute(0, 1.5f, 150, 75, laserEventArgs.SteamVRInputSource);
            Selected();
        }

        /// <inheritdoc />
        public void OnLaserIn(LaserEventArgs laserEventArgs)
        {
            // not used
        }

        /// <inheritdoc />
        public void OnLaserOut(LaserEventArgs laserEventArgs)
        {
            // not used
        }

        #endregion

        public IEnumerator StopAudio()
        {
            yield return new WaitForSeconds(audioDuration);

            audioSource.Stop();
        }
    }
}