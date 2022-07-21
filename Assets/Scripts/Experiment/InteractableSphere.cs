using System.Collections;
using Helpers;
using UnityEngine;

namespace Experiment
{
    public class InteractableSphere : MonoBehaviour, ILaserInteraction
    {
        // TODO MOVE TO MRTK Haptics
        // private SteamVR_Action_Vibration hapticAction;


        public bool Visible;
        public bool Highlighted;
        public bool Target;
        public int Id;
        public bool Clickable;

        [SerializeField]
        private Material normalMaterial;
        [SerializeField]
        private Material highlightedMaterial;
        [SerializeField]
        private int audioDuration = 1;

        public AudioSource audioSource;
        private MeshRenderer meshRenderer;
        private SphereController sphereController;
        private MeshCollider meshCollider;

        private void Awake()
        {
            meshCollider = GetComponent<MeshCollider>();
            audioSource = GetComponent<AudioSource>();
            meshRenderer = GetComponent<MeshRenderer>();
            sphereController = GetComponentInParent<SphereController>();

            // TODO MOVE TO MRTK Haptics
            // hapticAction = SteamVR_Actions.default_Haptic;
        }

        public void DeactivateCollider()
        {
            meshCollider.enabled = false;
        }


        public void ActivateCollider()
        {
            meshCollider.enabled = false;
        }

        private void Selected()
        {
            sphereController.SphereSelected(this);
        }

        public void Setup()
        {
            meshRenderer.enabled = Visible;
            meshRenderer.material = (Highlighted && Target) ? highlightedMaterial : normalMaterial;
            meshCollider.enabled = Clickable;
            if (!Target) return;

            StartCoroutine(StopAudio());
            audioSource.Play();
        }


        #region Implementation of IPointerClickHandler

        /// <inheritdoc />
        public void OnLaserClick(LaserEventArgs laserEventArgs)
        {
            // TODO MOVE TO MRTK Haptics
            // hapticAction.Execute(0, 1.5f, 150, 75, laserEventArgs.SteamVRInputSource);
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