using System;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Questionnaire
{
    public class NumericInputPage : QuestionnairePage
    {
        [SerializeField]
        private GameObject NumericInput;
        private List<GameObject> inputs;

        /// <summary>
        /// Value set on numpad
        /// </summary>
        public string Value = "0";

        /// <summary>
        /// Value is submitted
        /// </summary>
        public bool Submitted;

        private Guid inputGuid = Guid.NewGuid();

        private void Start()
        {
        
        }

        // Start is called before the first frame update
        public List<GameObject> CreateNumericInputPage(string questionnaireId, string qType, string qInstructions, string qId, string qText,
            RectTransform questionRec, string pId)
        {
            PId = pId;
            QuestionnaireId = questionnaireId;
            QId = qId;
            QType = qType;
            QInstructions = qInstructions;
            QText = qText;
            _questionRecTest = questionRec;
            QMandatory = true;

            inputs = new List<GameObject>();
            InitTask();
            return inputs;
        }

        public void InitTask()
        {
            // Task page has no child objects, create empty
            var temp = Instantiate(NumericInput);
            temp.name = inputGuid.ToString();
            var numericInput = temp.GetComponent<NumericInput>();
            EventManager.Listen<NumericInputSubmitted>(InputSubmitted);
/*        numericInput.OnSubmit.AddListener(InputSubmitted);*/

            var buttonRect = temp.GetComponent<RectTransform>();
            buttonRect.SetParent(_questionRecTest);
            buttonRect.localPosition = new Vector3(-170 + (1 * 140), 60 - (1 * 30), 0);
            buttonRect.localRotation = Quaternion.identity;

            var localScale = buttonRect.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            buttonRect.localScale = localScale;

            inputs.Add(temp);
        }

        private void InputSubmitted(NumericInputSubmitted inputValue)
        {
            if (inputValue.Sender.name != inputGuid.ToString()) return;
            
            Submitted = !inputValue.Value.Equals(Questionnaire.NumericInput.ZeroValue);
            Value = inputValue.Value;
        }

        private void OnDestroy()
        {
            var numericInput = GetComponentInChildren<NumericInput>();
            //numericInput.OnSubmit.RemoveListener(InputSubmitted);
            EventManager.RemoveListener<NumericInputSubmitted>(InputSubmitted);
        }
    }
}
