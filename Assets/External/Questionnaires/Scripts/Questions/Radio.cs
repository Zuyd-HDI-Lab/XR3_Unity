using System.Collections.Generic;
using Questionnaire;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;

/// <summary>
/// Radio.class
/// 
/// version 1.0
/// date: July 1st, 2020
/// authors: Martin Feick & Niko Kleer
/// </summary>

namespace VRQuestionnaireToolkit
{
    public class Radio : QuestionnairePage
    {
        public int NumRadioButtons;

        public GameObject RadioButtons;
        public JSONArray QOptions;

        private bool _isOdd;
        public List<GameObject> RadioList; //contains all radiobuttons which correspond to one question

        //qText look how many q in one file >4 deny
        public List<GameObject> CreateRadioQuestion(string questionnaireId, string qType, string qInstructions, string qId, string qText, bool qMandatory, JSONArray qOptions, int numberQuestion, RectTransform questionRec, string pId)
        {
            PId = pId;
            QuestionnaireId = questionnaireId;
            QId = qId;
            QType = qType;
            QInstructions = qInstructions;
            QText = qText;
            QOptions = qOptions;
            NumRadioButtons = CountRadioButtons(qOptions);
            _questionRecTest = questionRec;
            QMandatory = qMandatory;

            RadioList = new List<GameObject>();

            // generate radio and corresponding text labels on a single page
            for (var j = 0; j < qOptions.Count; j++)
            {
                if (qOptions[j] == "") continue;
                
                if (NumRadioButtons <= 7)
                    InitRadioButtonsHorizontal(numberQuestion, j, (NumRadioButtons % 2) != 0);

                else
                {
                    Debug.LogError("We currently only support up to 7 options");
                }
            }
            return RadioList;
        }

        private int CountRadioButtons(JSONArray qOptions)
        {
            var counter = 0;
            for (var i = 0; i < qOptions.Count; i++)
            {
                if (qOptions[i] != "")
                    counter++;
            }

            return counter;
        }

        private void InitRadioButtonsHorizontal(int numQuestions, int numOptions, bool isOdd)
        {
            // Instantiate radio prefabs
            var temp = Instantiate(RadioButtons);
            temp.name = "radio_" + numOptions;

            // Set radiobutton label 
            var text = temp.GetComponentInChildren<TextMeshProUGUI>();
            text.text = QOptions[numOptions];

            // Place in hierarchy 
            var radioRec = temp.GetComponent<RectTransform>();
            radioRec.SetParent(_questionRecTest);

            radioRec.localPosition = isOdd ? new Vector3(-190 + (numOptions * 85), 91 - (numQuestions * 92), 0) : new Vector3(-150 + (numOptions * 85), 90 - (numQuestions * 100), 0);

            radioRec.localRotation = Quaternion.identity;
            var localScale = radioRec.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            radioRec.localScale = localScale;

            // Set radiobutton group
            var radioScript = temp.GetComponentInParent<Radio>();
            temp.GetComponentInChildren<Toggle>().group = radioScript.gameObject.GetComponent<ToggleGroup>();

            RadioList.Add(temp);
        }
    }
}