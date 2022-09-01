using Constants;
using Experiment.Settings;
using Helpers;
using Questionnaire;
using UnityEngine;
using UXF;

namespace Experiment
{
    public class SphereTask : TaskBase
    {
        public override string TaskType => TaskTypes.Spheres;
        private SphereController SphereController;

        protected override void Starting()
        {
            SphereController = GetComponentInChildren<SphereController>();
        }

        public override void SetupTask(Trial trial)
        {
            var visible = trial.settings.GetBool(TrialSettingNames.Visible, true);
            var target = trial.settings.GetInt(TrialSettingNames.TargetId, 0);
            var highlighted = trial.settings.GetBool(TrialSettingNames.Highlighted, false);

            SphereController.Setup(visible, target, highlighted);

            base.SetupTask(trial);
        }

        public override void StartBlock(Block block)
        {
            EventManager.Trigger(new VisibilityChangeRequest { Show = false });
            SphereController.OnSphereSelected?.AddListener(SphereSelected);
            base.StartBlock(block);
        }

        protected override void TrialStarting()
        {
            SphereController.EnableSpheres();
        }

        public void SphereSelected(InteractableSphere sphere)
        {
            if (Session.instance.CurrentTrial == null || Session.instance.CurrentTrial.status != TrialStatus.InProgress)
                return;
            Debug.Log($"Sphere {sphere.Id} is {(sphere.Target ? "target" : "not target")} in block {Session.instance.currentBlockNum} and (relative) trial {relativeTrialNumber}");
            // Store trial results
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
                EventManager.Trigger(new VisibilityChangeRequest { Show = true });
                SphereController.DeactivateSpheres();
                SphereController.OnSphereSelected?.RemoveListener(SphereSelected);
                ExperimentEvents.BlockFinished(new BlockEventArgs { Block = currentBlock });
            }
        }
    }
}