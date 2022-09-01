using System.Collections;
using System.Collections.Generic;
using Questionnaire;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRQuestionnaireToolkit
{
    public class LinearGrid : QuestionnairePage
    {
        public int NumRadioButtons;
        public int QMin;
        public int QMax;
        public int NumGrid;

        private string _qMinLabel;
        private string _qMaxLabel;
        private Sprite _sprite;

        public GameObject LinearGridButton;
        public JSONArray QOptions;
        
        private bool _isOdd;
        public List<GameObject> LinearGridList; //contains all radiobuttons which correspond to one question

        //qText look how many q in one file >4 deny
        public List<GameObject> CreateLinearGridQuestion(string questionnaireId, string qType, string qInstructions, string qId, string qText, JSONArray qOptions, int numberQuestion, RectTransform questionRec, bool qMandatory, int qMin, int qMax, string qMinxLabel, string qMaxLabel, string pId)
        {
            PId = pId;
            QuestionnaireId = questionnaireId;
            QId = qId;
            QType = qType;
            QInstructions = qInstructions;
            QText = qText;
            QOptions = qOptions;
            NumGrid = numberQuestion;
            _questionRecTest = questionRec;
            QMin = qMin;
            QMax = qMax;
            _qMaxLabel = qMaxLabel;
            _qMinLabel = qMinxLabel;
            QMandatory = qMandatory;

            LinearGridList = new List<GameObject>();


            // generate radio and corresponding text labels on a single page
            for (var j = 0; j < QMax; j++)
            {
                if (qOptions[j] == "") continue;
                
                if (NumRadioButtons <= 20)
                    GenerateLinearGrid(numberQuestion, j); //use odd number layout

                else
                {
                    Debug.LogError("We currently only support up to 20 gridCells");
                }
            }
            return LinearGridList;
        }

        private void GenerateLinearGrid(int numQuestions, int numOptions)
        {
            // Instantiate radio prefabs
            var temp = Instantiate(LinearGridButton);
            temp.name = "linearGrid_" + numOptions;

            // Place in hierarchy 
            var radioRec = temp.GetComponent<RectTransform>();
            radioRec.SetParent(_questionRecTest);
            radioRec.localPosition = new Vector3(-120 + (numOptions * 20), 90 - (numQuestions * 95), 0);
            radioRec.localRotation = Quaternion.identity;
            var localScale = radioRec.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            radioRec.localScale = localScale;


            if (numOptions == 0)
            {
                temp.GetComponentInChildren<Toggle>().GetComponentsInChildren<TextMeshProUGUI>()[0].text =
                    _qMinLabel;
                temp.GetComponentInChildren<Toggle>().GetComponentsInChildren<TextMeshProUGUI>()[1].text =
                    _qMaxLabel;
            }
            else
            {
                temp.GetComponentInChildren<Toggle>().GetComponentsInChildren<TextMeshProUGUI>()[0].text =
                    "";
                temp.GetComponentInChildren<Toggle>().GetComponentsInChildren<TextMeshProUGUI>()[1].text =
                    "";
                temp.GetComponentInChildren<Toggle>().GetComponentsInChildren<Image>()[2].gameObject.SetActive(false);
            }

            // Set radiobutton group
            var linearGridScript = temp.GetComponentInParent<LinearGrid>();
            temp.GetComponentInChildren<Toggle>().group = linearGridScript.gameObject.GetComponent<ToggleGroup>();

            LinearGridList.Add(temp);
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
        private void SetMiddle()
        {
            _sprite = LoadSprite();
        }

        private Sprite LoadSprite()
        {
            const string load = "stick";
            var temp = Resources.Load<Sprite>(load);

            return temp;
        }
    }
}