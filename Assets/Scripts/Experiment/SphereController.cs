using UnityEngine;
using UnityEngine.Events;

namespace Experiment
{
    public class SphereController : MonoBehaviour
    {
        private InteractableSphere[] spheres;
        public UnityEvent<InteractableSphere> OnSphereSelected;

        private void Start()
        {
            // Get all spheres
            spheres = GetComponentsInChildren<InteractableSphere>(true);
        }

        private void Awake()
        {
            OnSphereSelected = new UnityEvent<InteractableSphere>();
        }

        /// <summary>
        /// Set up the scene for the experiment
        /// </summary>
        /// <param name="visible">Sets the sphere visibility</param>
        /// <param name="targetId">Sets the sphere that is the target</param>
        /// <param name="highlighted">Is the target sphere highlighed</param>
        public void Setup(bool visible, int targetId, bool highlighted)
        {
            foreach (var sphere in spheres)
            {
                // TODO Move to sphere initialization
                sphere.Visible = visible;
                sphere.Target = sphere.Id == targetId;
                sphere.Highlighted = highlighted;
                //sphere.gameObject.SetActive(false);
            }
        }

        /// <summary>
        ///  Clean up
        /// </summary>
        public void DisableSpheres()
        {
            foreach (var sphere in spheres)
            {
                sphere.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Start the trial
        /// </summary>
        /// <param name="trial"></param>
        public void EnableSpheres()
        {
            // enable all spheres
            foreach (var sphere in spheres)
            {
                sphere.gameObject.SetActive(true);
                sphere.Setup();
            }
        }

        public void SphereSelected(InteractableSphere sphere)
        {
            OnSphereSelected?.Invoke(sphere);
        }
    }
}
