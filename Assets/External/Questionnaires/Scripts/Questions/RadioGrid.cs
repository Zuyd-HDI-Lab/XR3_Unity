using System;
using System.Collections;
using System.Collections.Generic;
using Questionnaire;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;

/// <summary>
/// RadioGrid.class
/// 
/// version 1.0
/// date: July 1st, 2020
/// authors: Martin Feick & Niko Kleer
/// </summary>

namespace VRQuestionnaireToolkit
{
    public class RadioGrid : QuestionnairePage
    {
        public int NumRadioButtons;

        public GameObject RadioButtons;
        public JSONArray QOptions;
        public string QConditions;

        public List<GameObject> RadioList; //contains all radiobuttons which correspond to one question

        //qText look how many q in one file >4 deny
        public List<GameObject> CreateRadioGridQuestion(string questionnaireId, string qType, string qInstructions, string qId, string qText, bool Mandatory, JSONArray qOptions, string qConditions, int numberConditions, RectTransform questionRec, string pId)
        {
            PId = pId;
            QuestionnaireId = questionnaireId;
            QId = qId;
            QType = qType;
            QInstructions = qInstructions;
            QText = qText;
            QOptions = qOptions;
            QConditions = qConditions;
            NumRadioButtons = qOptions.Count;
            _questionRecTest = questionRec;
            QMandatory = Mandatory;

            RadioList = new List<GameObject>();

            // generate radioGrid and corresponding text labels on a single page
            for (var j = 0; j < qOptions.Count; j++)
            {
                if (qOptions[j] == "") continue;
                if (NumRadioButtons <= 7)
                    InitRadioGridButtons(numberConditions, j);
                else
                {
                    Debug.LogError("We currently only support up to 7 options (e.g., 7 point-likert scale)");
                }
            }
            return RadioList;
        }

        private void InitRadioGridButtons(int numConditions, int numOptions)
        {
            //Instantiate radioGridbuttons
            var temp = Instantiate(RadioButtons);
            temp.name = "radioGrid_" + numOptions;

            // only generate one (top) row of labels
            if (numConditions == 0)
            {
                var text = temp.GetComponentInChildren<TextMeshProUGUI>();
                text.text = QOptions[numOptions];
            }

            // Place in hierarchy 
            var radioGridRec = temp.GetComponent<RectTransform>();
            radioGridRec.SetParent(_questionRecTest);

            radioGridRec.localPosition = new Vector3(-100 + (numOptions * 70), 35 - (numConditions * 50), 0);
            radioGridRec.localRotation = Quaternion.identity;
            var localScale = radioGridRec.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            radioGridRec.localScale = localScale;

            // Set radiobutton group
            var radioGridScript = temp.GetComponentInParent<RadioGrid>();
            temp.GetComponentInChildren<Toggle>().group = radioGridScript.gameObject.GetComponent<ToggleGroup>();

            RadioList.Add(temp);
        }
    }
}