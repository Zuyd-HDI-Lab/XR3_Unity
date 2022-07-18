namespace Experiment
{
    public interface IExperiment
    {
        string OutputLocation { get; }
        void SetupBlocks();
        void PlayerAtStart(PlayerAtStartEventArgs args);
        void SessionStarted();
        void StartQuestionnaire();
    }
}
