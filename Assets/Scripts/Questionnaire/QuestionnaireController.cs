using System;
using System.Data;
using Constants;
using Helpers;
using Questionnaires.Scripts.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace Questionnaire
{
    /// <summary>
    /// Questionnaire controller
    /// </summary>
    public class QuestionnaireController : MonoBehaviour
    {
        private void Start()
        {
            EventManager.Listen<VisibilityChangeRequest>(OnVisibilityChanged);
        }

        private void OnVisibilityChanged(VisibilityChangeRequest data)
        {
            if (data.Show)
                ShowQuestionnaire();
            else 
                HideQuestionnaire();
        }

        /// <summary>
        /// Event(s) to trigger when the questionnaire begins. Can pass the instance of the Session as a dynamic argument
        /// </summary>
        /// <returns></returns>
        [Tooltip("Items in this event will be triggered when the questionnaire begins.")]
        public QuestionnaireNavigationEvent OnQuestionnaireBegin = new QuestionnaireNavigationEvent();

        /// <summary>
        /// Event(s) to trigger when the questionnaire ends. Can pass the instance of the Session as a dynamic argument
        /// </summary>
        /// <returns></returns>
        [Tooltip("Items in this event will be triggered when the questionnaire ends.")]
        public QuestionnaireNavigationEvent OnQuestionnaireEnd = new QuestionnaireNavigationEvent();

        /// <summary>
        /// Event(s) to trigger when the questionnaire navigates to a new page. Can pass the instance of the Session as a dynamic argument
        /// </summary>
        /// <returns></returns>
        [Tooltip("Items in this event will be triggered when the questionnaire begins.")]
        public QuestionnaireNavigationEvent OnQuestionnaireNavigate = new QuestionnaireNavigationEvent();

        /// <summary>
        /// Event(s) to trigger when the questionnaire navigates to a new page. Can pass the instance of the Session as a dynamic argument
        /// </summary>
        /// <returns></returns>
        [Tooltip("Items in this event will be triggered when the questionnaire begins.")]
        public QuestionnaireTaskEvent OnTaskCompleted = new QuestionnaireTaskEvent();

        /// <summary>
        /// Event(s) to trigger when the questionnaire navigates to a new page. Can pass the instance of the Session as a dynamic argument
        /// </summary>
        /// <returns></returns>
        [Tooltip("Items in this event will be triggered when the questionnaire begins.")]
        public QuestionnaireTaskEvent OnTaskStarted = new QuestionnaireTaskEvent();

        /// <summary>
        /// Completed task
        /// </summary>
        /// <param name="taskId"></param>
        public void TaskComplete(int taskId)
        {
            OnTaskCompleted?.Invoke(new QuestionnaireTaskEventArgs { TaskId = taskId });
        }

        /// <summary>
        /// Start task
        /// </summary>
        /// <param name="taskId"></param>
        public void StartTask(int taskId)
        {
            OnTaskStarted?.Invoke(new QuestionnaireTaskEventArgs { TaskId = taskId });
        }

        /// <summary>
        /// Enable the questionnaire
        /// </summary>
        public void EnableQuestionnaire()
        {
            transform.Find(GameObjectNames.VRQuestionnaireToolkit).gameObject.SetActive(true);
        }

        /// <summary>
        /// Hide the questionnaire from view
        /// Disables the canvas
        /// </summary>
        public void HideQuestionnaire()
        {
            transform.Find(GameObjectNames.VRQuestionnaireToolkit).GetComponent<Canvas>().enabled = false;
        }

        /// <summary>
        /// Hide the questionnaire from view
        /// Shows the canvas
        /// </summary>
        public void ShowQuestionnaire()
        {
            transform.Find(GameObjectNames.VRQuestionnaireToolkit).GetComponent<Canvas>().enabled = true;
        }
    }

    public class VisibilityChangeRequest
    {
        public bool Show { get; set; }

        // TODO loosely couple questionnaire to task
        // public VisibilityTarget Target { get; set; }
    }
}
