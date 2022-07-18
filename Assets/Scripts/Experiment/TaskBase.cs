using Constants;
using Experiment.Settings;
using UnityEngine;
using UXF;

namespace Experiment
{
    public abstract class TaskBase : MonoBehaviour
    {
        public abstract string TaskType { get; }
        protected CountdownTimer countdown;

        protected Trial currentTrial;
        protected Block currentBlock;
        private int countdownDelay;
        protected int relativeTrialNumber = 1; // Trial numbers start at 1        

        /// <summary>
        /// Unity start method
        /// Use starting in derived classes to set Start behavior
        /// </summary>
        private void Start()
        {
            ExperimentEvents.OnRequestBlockStart += OnRequestBlockStart;
            countdown = GameObject.Find(GameObjectNames.CountdownTimer).GetComponent<CountdownTimer>();
            Starting();
        }

        /// <summary>
        /// Called when Unity calls Start
        /// </summary>
        protected virtual void Starting()
        {

        }

        /// <summary>
        /// Called when Unity destroys this object
        /// Use Destroying in derived class to set Destroy behavior
        /// </summary>
        private void OnDestroy()
        {
            ExperimentEvents.OnRequestBlockStart -= OnRequestBlockStart;
            Destroying();
        }

        /// <summary>
        /// Called when Unity calls OnDestroy
        /// </summary>
        protected virtual void Destroying()
        {

        }

        private void OnRequestBlockStart(BlockEventArgs obj)
        {
            // Check if this is a handler for the task type
            if (obj.Block.settings.GetString(BlockSettingNames.TaskType, TaskType).Equals(TaskType))
                StartBlock(obj.Block);
        }

        public virtual void StartBlock(Block block)
        {
            currentBlock = block;
            currentTrial = block.firstTrial;

            SetupTask(currentTrial);
            StartTrial(currentTrial);
        }

        /// <summary>
        /// Setup task
        /// Sets countdown timer countdown
        /// </summary>
        /// <param name="trial"></param>
        public virtual void SetupTask(Trial trial)
        {
            countdownDelay = trial.settings.GetInt(SessionSettingNames.DelayTime, 0);

            // set the countdown delay
            countdown.countdownTimeInSeconds = countdownDelay;
        }

        /// <summary>
        /// Start task
        /// If countdown delay is set uses timer
        /// </summary>
        /// <param name="trial"></param>
        public virtual void StartTrial(Trial trial)
        {
            currentTrial = trial;

            // if there is a delay, start timer
            if (countdownDelay > 0)
            {
                countdown.ShowTimer();
                countdown.onTimeElapsed.AddListener(CountdownFinished);
                countdown.StartCountdown();
                return;
            }

            TrialStarting();
            trial.Begin();
        }

        /// <summary>
        /// Right before the actual trial begins, this is called
        /// Use this to set starting conditions that need to be set right at the beginning
        /// </summary>
        protected abstract void TrialStarting();

        /// <summary>
        /// Countdown timer finished        
        /// </summary>
        private void CountdownFinished()
        {
            countdown.onTimeElapsed.RemoveListener(CountdownFinished);
            OnCountdownFinished();
        }

        /// <summary>
        /// Countdown timer finished        
        /// Start current trial
        /// </summary>
        protected virtual void OnCountdownFinished()
        {
            countdown.HideTimer();
            TrialStarting();
            currentTrial.Begin();
        }
    }
}