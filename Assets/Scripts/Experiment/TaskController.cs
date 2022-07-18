using Assets.Scripts.Video;
using Constants;
using Questionnaire;
using Questionnaires.Scripts.Events;
using UnityEngine;
using UXF;
using Video;

namespace Experiment
{
    public class TaskController : MonoBehaviour
    {
        private QuestionnaireController questionnaireController;
        private AudioRecorder audioRecorder;

        private void Awake()
        {
            // TODO improve retrieval of questionnaire controller script
            questionnaireController = GameObject.Find(GameObjectNames.QuestionnaireController)?.GetComponent<QuestionnaireController>();
            audioRecorder = GameObject.Find("FollowHead").GetComponent<AudioRecorder>();
            ExperimentEvents.OnBlockFinished += ExperimentEvents_OnBlockFinished;
            questionnaireController.OnQuestionnaireBegin?.AddListener(QuestionnaireBegin);
            questionnaireController.OnQuestionnaireEnd?.AddListener(QuestionnaireEnd);
            questionnaireController.OnTaskStarted?.AddListener(TaskStarted);

            Debug.Log("Task Controller registered");
        }

        private void TaskStarted(QuestionnaireTaskEventArgs questionnaireTaskEventArgs)
        {
            StartBlock(questionnaireTaskEventArgs.TaskId);
        }

        private void QuestionnaireBegin(QuestionnaireNavigationEventArgs questionnaireNavigationEventArgs)
        {
            Debug.Log("Start Recording");
            audioRecorder.StartRecording();
            VideoRecorder.Record();
        }
        private void QuestionnaireEnd(QuestionnaireNavigationEventArgs questionnaireNavigationEventArgs)
        {
            audioRecorder.StopRecording();
            VideoRecorder.Stop();
            Session.instance.End();
        }

        private void ExperimentEvents_OnBlockFinished(BlockEventArgs obj)
        {
            questionnaireController.TaskComplete(obj.Block.number);
        }

        public void StartBlock(int block)
        {
            //Session.instance.BeginNextTrial();
            //var block = Session.instance.CurrentBlock; 
            //ExperimentEvents.OnBlockFinished += BlockFinished;
            var b = Session.instance.GetBlock(block);
            ExperimentEvents.RequestBlockStart(new BlockEventArgs { Block = b });
        }

/*    public void StartTask(int taskId)
    {
        StartBlock(taskId);
    }*/

        public void TaskCompleted(int taskId)
        {
            questionnaireController.TaskComplete(taskId);
            // ExperimentEvents.OnBlockFinished -= BlockFinished;
        }
    }
}
