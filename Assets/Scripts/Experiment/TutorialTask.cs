using Constants;
using Experiment.Settings;
using UnityEngine;
using UXF;
using Valve.VR.InteractionSystem;

namespace Experiment
{
    public class TutorialTask : TaskBase
    {
        private TutorialButtonHints TutorialButtonHints;
        private SphereController SphereController;       

        public override string TaskType => TaskTypes.Tutorial;

        protected override void TrialStarting()
        {
            SphereController.EnableSpheres();

            // TODO improve
            foreach (var hand in GameObject.FindObjectsOfType<Hand>())
                TutorialButtonHints.ShowAll(hand);
        }

        protected override void Starting()
        {
            useCountdown = false;
            SphereController = FindObjectOfType<SphereController>();
            TutorialButtonHints = FindObjectOfType<TutorialButtonHints>();
        }

        public override void StartBlock(Block block)
        {
            SphereController.OnSphereSelected?.AddListener(SphereSelected);
            
            base.StartBlock(block);
        }

        public override void SetupTask(Trial trial)
        {
            var target = trial.settings.GetInt(TrialSettingNames.TargetId, 0);
            SphereController.Setup(true, target, false);

            base.SetupTask(trial);
        }

        public void SphereSelected(InteractableSphere sphere)
        {
            Session.instance.CurrentTrial.result["target_clicked"] = sphere.Target;
            Session.instance.CurrentTrial.result["sphere_clicked"] = sphere.Id;
            Session.instance.CurrentTrial.End();

            // Check if we have move trials, if so start the next one
            if (currentBlock.lastTrial != currentTrial)
            {

                relativeTrialNumber++;
                var t = currentBlock.GetRelativeTrial(relativeTrialNumber);
                SetupTask(t);
                StartTrial(t);
            }
            else
            {
                // TODO improve
                foreach (var hand in GameObject.FindObjectsOfType<Hand>())
                    TutorialButtonHints.HideAll(hand);

                SphereController.DisableSpheres();
                SphereController.OnSphereSelected?.RemoveListener(SphereSelected);
                ExperimentEvents.BlockFinished(new BlockEventArgs { Block = currentBlock });
            }
        }
    }
}