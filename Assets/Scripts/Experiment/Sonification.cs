using System.IO;
using Constants;
using Experiment.Settings;
using Helpers;
using Questionnaire;
using UnityEngine;
using UXF;

namespace Experiment
{
    public class Sonification : MonoBehaviour, IExperiment
    {
        public bool UXFSessionStarted;
        public bool ParticipantAtStart;
        public bool Started;

        private string saveLocationBase = $"data_output/{nameof(Sonification)}";

        public string OutputLocation { get; private set; } 

        private QuestionnaireController questionnaireController;
        private TutorialButtonHints tutorialButtonHints;

        private void Awake()
        {
            // TODO improve retrieval of questionnaire controller script
            questionnaireController = GameObject.Find(GameObjectNames.QuestionnaireController)?.GetComponent<QuestionnaireController>();
            tutorialButtonHints = GetComponent<TutorialButtonHints>();
        }

        private void Start()
        {
            EventManager.Listen<PlayerAtStartEventArgs>(PlayerAtStart);
        }

        public void SetupBlocks()
        {
            foreach(var b in Session.instance.blocks)
            {
                var taskType = Session.instance.settings.GetString($"block{b.number}_{BlockSettingNames.TaskType}", TaskTypes.Tutorial);
                b.settings.SetValue(BlockSettingNames.TaskType, taskType);
            }
        }

        public void PlayerAtStart(PlayerAtStartEventArgs args)
        {
            ParticipantAtStart = true;
        }

        public void SessionStarted()
        {
            OutputLocation = Path.Combine(saveLocationBase, Session.instance.ppid);
            Session.instance.BasePath = Path.Combine(OutputLocation, "SessionData");
            SetupBlocks();

            UXFSessionStarted = true;
            // TODO fix this
            //ParticipantAtStart = false;    
        }

        public void StartQuestionnaire()
        {
            questionnaireController.EnableQuestionnaire();
        }

        private void Update()
        {
            if (!Started && ParticipantAtStart && UXFSessionStarted)
            {
                Started = true;
                StartQuestionnaire();
            }
        }
    }
}
