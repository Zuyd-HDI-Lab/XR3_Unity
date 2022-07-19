using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Questionnaire
{
    /// <summary>
    /// Questionnaire page that displays only text
    /// </summary>
    public class TextPage : QuestionnairePage
    {
        public GameObject Text;
        private List<GameObject> texts;
        public float BoldTextHeight;
        public float BoldTextOffset = 20;

        public List<GameObject> CreateTextPage(string questionnaireId, string qType, string qInstructions, string qId, float boldTextHeight, string qText,
            RectTransform questionRec, string pId)
        {
            PId = pId;
            QuestionnaireId = questionnaireId;
            QId = qId;
            QType = qType;
            QInstructions = qInstructions;
            QText = qText;
            BoldTextHeight = boldTextHeight;
            _questionRecTest = questionRec;
            QMandatory = false;

            texts = new List<GameObject>();
            InitText();
            return texts;
        }

        public void InitText()
        {        
            // Task page has no child objects, create empty
            var temp = Instantiate(Text);
            var text = temp.GetComponent<TextMeshProUGUI>();
            text.text = QText;

            var textRect = temp.GetComponent<RectTransform>();
            textRect.SetParent(_questionRecTest);
            textRect.localPosition = new Vector3(0, - (BoldTextHeight > 0 ? BoldTextHeight + BoldTextOffset : 0) , 0);
            textRect.localRotation = Quaternion.identity;
        
            var localScale = textRect.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            textRect.localScale = localScale;

            texts.Add(temp);
        }
    }
}
