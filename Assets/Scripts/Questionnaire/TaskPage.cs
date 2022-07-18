using System.Collections.Generic;
using Constants;
using Questionnaires.Scripts.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRQuestionnaireToolkit;

namespace Questionnaire
{
    /// <summary>
    /// Questionnaire page that requires a UXF task to be completed
    /// </summary>
    public class TaskPage : QuestionnairePage
    {
        public int TaskId; // TODO public?
        public bool TaskCompleted;
        public GameObject Task;    
        private List<GameObject> tasks;

        private Button startButton;

        public List<GameObject> CreateTaskPage(string questionnaireId, string qType, string qInstructions, string qId, string qText,
            int taskId, RectTransform questionRec, string pId)
        {
            PId = pId;
            QuestionnaireId = questionnaireId;
            QId = qId;
            QType = qType;
            QInstructions = qInstructions;
            QText = qText;
            _questionRecTest = questionRec;
            TaskId = taskId;
            QMandatory = true;

            tasks = new List<GameObject>();
            InitTask();
            return tasks;
        }

        public void InitTask()
        {        
            // Task page has no child objects, create empty
            var temp = Instantiate(Task);
            startButton = temp.GetComponent<Button>();
            startButton.onClick.AddListener(StartTask);
            var text = startButton.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "Start";

            var buttonRect = temp.GetComponent<RectTransform>();
            buttonRect.SetParent(_questionRecTest);
            buttonRect.localPosition = new Vector3(-170 + (1 * 140), 60 - (1 * 30), 0);
            buttonRect.localRotation = Quaternion.identity;
        
            var localScale = buttonRect.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            buttonRect.localScale = localScale;

            tasks.Add(temp);
        }

        public void StartTask()
        {
            questionnaireController.HideQuestionnaire();
            questionnaireController.StartTask(TaskId);
            questionnaireController.OnTaskCompleted.AddListener(OnTaskCompleted);
            startButton.interactable = false;
        }

        private void OnTaskCompleted(QuestionnaireTaskEventArgs args)
        {
            TaskCompleted = true;
            questionnaireController.OnTaskCompleted.RemoveListener(OnTaskCompleted);
            startButton.onClick.RemoveListener(StartTask);

            // Auto progress
            var nextButtonPageController = GameObject.Find(GameObjectNames.ButtonNext).GetComponent<PageController>();
            nextButtonPageController.GoToNextPage();
            questionnaireController.ShowQuestionnaire();
        }
    }
}
