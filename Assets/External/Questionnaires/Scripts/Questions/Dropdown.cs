using System.Collections;
using System.Collections.Generic;
using Questionnaire;
using SimpleJSON;
using TMPro;
using UnityEngine;

/// <summary>
/// Dropdown.class
/// 
/// version 1.0
/// date: July 1st, 2020
/// authors: Martin Feick & Niko Kleer
/// </summary>

namespace VRQuestionnaireToolkit
{
    public class Dropdown : QuestionnairePage
    {
        public int NumDropDown;

        public GameObject Dropbdown;
        public JSONArray QOptions;
        
        public List<GameObject> DropdownList; //contains all radiobuttons which correspond to one question

        //qText look how many q in one file >4 deny
        public List<GameObject> CreateDropdownQuestion(string questionnaireId, string qType, string qInstructions, string qId, string qText, JSONArray qOptions, int numberQuestion, RectTransform questionRec, string pId)
        {
            PId = pId;
            this.QuestionnaireId = questionnaireId;
            this.QId = qId;
            this.QType = qType;
            this.QInstructions = qInstructions;
            this.QText = qText;
            this.QOptions = qOptions;
            this.NumDropDown = numberQuestion;
            this._questionRecTest = questionRec;

            DropdownList = new List<GameObject>();

            // generate dropdowns and corresponding text labels on a single page
            if (QText == "") return DropdownList;
            
            if (NumDropDown <= 7)
                InitDropdown(NumDropDown);
            else
            {
                Debug.LogError("We currently only support up to 7 dropdown questions on a single page");
            }

            return DropdownList;
        }

        private void InitDropdown(int numQuestions)
        {
            // Instantiate dropdown prefabs
            var temp = Instantiate(Dropbdown);
            temp.name = "dropdown" + numQuestions;

            var dropdown = temp.GetComponentInChildren<TMP_Dropdown>();

            for (var i = 0; i < QOptions.Count; i++)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(QOptions[i].Value));
            }

            /*for (int i = 0; i < QOptions.Count; i++)
                dropdown.options[i].text = QOptions[i].Value;*/

            // Place in hierarchy 
            var dropbDownRec = temp.GetComponent<RectTransform>();
            dropbDownRec.SetParent(_questionRecTest);
            dropbDownRec.localPosition = new Vector3(0, 80 - (numQuestions * 90), 0);
            
            var localScale = dropbDownRec.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            dropbDownRec.localScale = localScale;

            DropdownList.Add(temp);
        }
    }
}