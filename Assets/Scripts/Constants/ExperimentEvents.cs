using System;
using UXF;

namespace Constants
{
    /// <summary>
    /// Experiment events aggregator
    /// </summary>
    public static class ExperimentEvents
    {
        public static event Action<TrialEventArgs> OnRequestTrialStart;

        public static void RequestTrialStart(TrialEventArgs args)
        {
            OnRequestTrialStart?.Invoke(args);
        }

        public static event Action<TrialEventArgs> OnTrialFinished;

        public static void TrialFinished(TrialEventArgs args)
        {
            OnTrialFinished?.Invoke(args);
        }

        public static event Action<BlockEventArgs> OnRequestBlockStart;

        public static void RequestBlockStart(BlockEventArgs args)
        {
            OnRequestBlockStart?.Invoke(args);
        }

        public static event Action<BlockEventArgs> OnBlockFinished;

        public static void BlockFinished(BlockEventArgs args)
        {
            OnBlockFinished?.Invoke(args);
        }
    }

    public class TrialEventArgs
    {
        public Trial Trial { get; set; }


    }    
    public class BlockEventArgs
    {
        public Block Block { get; set; }


    }
}
