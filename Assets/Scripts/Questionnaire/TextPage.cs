using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Questionnaire
{
    /// <summary>
    /// Questionnaire page that displays only text
    /// </summary>
    public class TextPage : QuestionnairePage
    {
        public GameObject Text;
        private List<GameObject> tasks;

        public List<GameObject> CreateTextPage(string questionnaireId, string qType, string qInstructions, string qId, string qText,
            RectTransform questionRec, string pId)
        {
            PId = pId;
            QuestionnaireId = questionnaireId;
            QId = qId;
            QType = qType;
            QInstructions = qInstructions;
            QText = qText;
            _questionRecTest = questionRec;
            QMandatory = false;

            tasks = new List<GameObject>();
            InitTask();
            return tasks;
        }

        public void InitTask()
        {        
            // Task page has no child objects, create empty
            var temp = Instantiate(Text);
            var text = temp.GetComponent<TextMeshProUGUI>();
            text.text = QText;

            var buttonRect = temp.GetComponent<RectTransform>();
            buttonRect.SetParent(_questionRecTest);
            buttonRect.localPosition = new Vector3(-170 + (1 * 140), 60 - (1 * 30), 0);
            buttonRect.localRotation = Quaternion.identity;
        
            var localScale = buttonRect.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            buttonRect.localScale = localScale;

            tasks.Add(temp);
        }
    }
}
