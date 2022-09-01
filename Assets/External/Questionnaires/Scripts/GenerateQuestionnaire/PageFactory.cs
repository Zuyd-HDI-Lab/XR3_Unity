using System.Collections.Generic;
using Questionnaire;
using Questionnaires.Scripts.Helpers;
using UnityEngine;
using SimpleJSON;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine.UI;

/// <summary>
/// PageFactory.class
/// 
/// version 1.0
/// date: July 1st, 2020
/// authors: Martin Feick & Niko Kleer
/// </summary>

namespace VRQuestionnaireToolkit
{
    public class PageFactory : MonoBehaviour
    {
        public int NumPages;
        public int CurrentPage;

        public List<GameObject> PageList;
        public List<List<GameObject>> QuestionList; //contains all questions which belong to one page

        public RectTransform PageParent;
        public GameObject PagePrefab;
        public GameObject RadioHorizontalPrefab;
        public GameObject RadioGridPrefab;
        public GameObject Checkbox;
        public GameObject CheckboxGrid;
        public GameObject Dropdown;
        public GameObject LinearGrid;
        public GameObject LinearSlider;
        public GameObject TextInput;
        public GameObject LastPagePrefab;
        public GameObject TaskPrefab;
        public GameObject TextPrefab;
        public GameObject NumericInputPrefab;
        public GameObject LikertPrefab;

        private GameObject _newPage;
        private const int QuestionPerPage = 4;

        public enum QuestionType
        {
            Radio,
            RadioGrid,
            Checkbox,
            CheckboxGrid,
            Dropdown,
            LinearScale,
            TextInput,
            Task
        }

        private QuestionType _type;

        /*
         * This method adds 1..n pages to a questionnaire
        */
        public void AddPage(string qId, string qType, string qInstructions, JSONArray _qData, JSONArray qConditions, JSONArray qOptions, string pId)
        {
            if (_qData != null && qOptions != null)
            {
                _newPage.SetActive(false); //do only keep one page enabled

                _newPage = Instantiate(PagePrefab);
                _newPage.name = "page_" + PageList.Count;

                //ensuring correct placement and scaling in the UI
                RectTransform pageRec = _newPage.GetComponent<RectTransform>();
                pageRec.SetParent(PageParent);
                SetRec(pageRec);

                //ensuring the anchor of q_panel is centered
                GameObject q_panel = GameObject.Find("Q_Panel");
                RectTransform qPanelRect = q_panel.GetComponent<RectTransform>();
                CenterRec(qPanelRect);

                //display instruction on page
                GameObject q_header = GameObject.Find("Q_Header");
                TextMeshProUGUI descriptionText = q_header.GetComponentInChildren<TextMeshProUGUI>();
                descriptionText.text = qInstructions;

                PageList.Add(_newPage);
                NumPages++;

                DetermineQuestionType(qType, qId, qInstructions, _qData, qConditions, qOptions, pId);

                // Debug.Log(qId + " " + qType + " " + qInstructions + " " + qText + " " + qOptions);
                // Debug.Log(qOptions.Count);
            }
        }

        public void InitSetup()
        {
            CurrentPage = 0;

            for (int i = 1; i < NumPages - 1; i++)
                PageList[i].SetActive(false);

            PageList[CurrentPage].SetActive(true);

            GameObject q_footer = GameObject.Find("Q_Footer");
            Button[] nextButton = q_footer.GetComponentsInChildren<Button>();
            nextButton[1].gameObject.SetActive(false);
        }

        /*
         * This method instantiates the required components based on the JSON Input
         */
        public void DetermineQuestionType(string qType, string qId, string qInstructions, JSONArray _qData,
            JSONArray qConditions, JSONArray qOptions, string pId)
        {
            GameObject temp;
            RectTransform radioHorizontalRec;
            GameObject q_main;
            TextMeshProUGUI text;
            TextMeshProUGUI[] textArray;

            switch (qType)
            {
                case QuestionnaireTypeNames.Radio:
                    if (_qData.Count < QuestionPerPage)
                    {
                        for (int i = 0; i < _qData.Count; i++)
                        {
                            temp = Instantiate(RadioHorizontalPrefab);
                            temp.name = "radioHorizontal_" + i;

                            radioHorizontalRec = temp.GetComponent<RectTransform>();
                            q_main = GameObject.Find("Q_Main");
                            radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                            //ensuring correct placement and scaling in the UI
                            text = temp.GetComponentInChildren<TextMeshProUGUI>();

                            // If question mandatory -> add " * "
                            if (_qData[i][2].AsBool)
                                text.text = _qData[i][1] + " *";
                            else
                                text.text = _qData[i][1];

                            text.transform.localPosition = new Vector3(0, 120 - (i * 92), text.transform.localPosition.z);
                            SetRec(radioHorizontalRec);

                            QuestionList.Add(temp.GetComponent<Radio>().CreateRadioQuestion(qId, qType, qInstructions, _qData[i][0], _qData[i][1], _qData[i][2].AsBool, _qData[i][3].AsArray, i, radioHorizontalRec, pId));
                        }
                    }
                    else
                    {
                        Debug.LogError("We currently only support up to 5 radioquestions per page");
                    }
                    break;
                case QuestionnaireTypeNames.RadioGrid:
                    if (qConditions.Count < 8)
                    {
                        for (int i = 0; i < qConditions.Count; i++)
                        {
                            temp = Instantiate(RadioGridPrefab);
                            temp.name = "radioGrid_" + i;

                            radioHorizontalRec = temp.GetComponent<RectTransform>();
                            q_main = GameObject.Find("Q_Main");
                            radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                            //ensuring correct placement and scaling in the UI
                            textArray = temp.GetComponentsInChildren<TextMeshProUGUI>();
                            textArray[0].text = "";

                            if (i == 0)
                            {

                                // If question mandatory -> add " * " to question in UI
                                if (_qData[i][2].AsBool)
                                    textArray[0].text = _qData[0][1] + " *";
                                else
                                    textArray[0].text = _qData[0][1];

                            }

                            //Differentiate between 5-Point and 7-Point Likert Scale
                            int temp2 = 0;
                            foreach (var option in qOptions)
                            {
                                if (option.Value != "")
                                    temp2++;
                            }

                            if (temp2 == 5)
                            {
                                textArray[1].text = qConditions[i][1];
                                textArray[1].transform.localPosition = new Vector3(-195, 17 - (i * 50), textArray[1].transform.localPosition.z);
                            }
                            else
                            {
                                textArray[1].text = qConditions[i][1];
                                textArray[1].transform.localPosition = new Vector3(-245, 17 - (i * 50), textArray[1].transform.localPosition.z);
                            }

                            textArray[0].transform.localPosition = new Vector3(0, 120, textArray[0].transform.localPosition.z);

                            SetRec(radioHorizontalRec);
                            QuestionList.Add(temp.GetComponent<RadioGrid>().CreateRadioGridQuestion(qId, qType, qInstructions, _qData[0][0],
                                _qData[0][1], _qData[0][2].AsBool, qOptions, qConditions[i][1], i, radioHorizontalRec, pId, qConditions[i][0]));
                        }
                    }
                    else
                    {
                        Debug.LogError("We currently only support up to 8 conditions");
                    }
                    break;
                case QuestionnaireTypeNames.Checkbox:

                    for (int i = 0; i < _qData.Count; i++)
                    {
                        temp = Instantiate(Checkbox);
                        temp.name = "checkbox_" + i;

                        radioHorizontalRec = temp.GetComponent<RectTransform>();
                        q_main = GameObject.Find("Q_Main");
                        radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                        //ensuring correct placement and scaling in the UI
                        text = temp.GetComponentInChildren<TextMeshProUGUI>();
                        text.text = _qData[i][1];
                        text.transform.localPosition = new Vector3(10, 110 - (i * 50), text.transform.localPosition.z);
                        SetRec(radioHorizontalRec);

                        QuestionList.Add(temp.GetComponent<Checkbox>().CreateCheckboxQuestion(qId, qType, qInstructions, _qData[i][0], _qData[i][1], qOptions, i, radioHorizontalRec, pId));
                    }
                    break;
                case QuestionnaireTypeNames.CheckboxGrid:
                    Debug.LogError("Checkboxgrid is not supported ATM");
                    break;
                case QuestionnaireTypeNames.LinearGrid:
                    for (int i = 0; i < _qData.Count; i++)
                    {
                        temp = Instantiate(LinearGrid);
                        temp.name = "linearGrid_" + i;
                        radioHorizontalRec = temp.GetComponent<RectTransform>();
                        q_main = GameObject.Find("Q_Main");
                        radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                        //ensuring correct placement and scaling in the UI
                        text = temp.GetComponentInChildren<TextMeshProUGUI>();

                        if (_qData[i][2].AsBool)
                            text.text = _qData[i][1] + " *";
                        else
                            text.text = _qData[i][1];

                        text.transform.localPosition = new Vector3(0, 100 - (i * 95), text.transform.localPosition.z);
                        SetRec(radioHorizontalRec);

                        QuestionList.Add(temp.GetComponent<LinearGrid>().CreateLinearGridQuestion(qId, qType, qInstructions, _qData[i][0], _qData[i][1], qOptions, i, radioHorizontalRec, _qData[i][2].AsBool, _qData[i][3], _qData[i][5], _qData[i][4], _qData[i][6], pId));
                    }
                    break;
                case QuestionnaireTypeNames.LinearSlider:
                    for (int i = 0; i < _qData.Count; i++)
                    {
                        temp = Instantiate(LinearSlider);
                        temp.name = "linearSlider_" + i;
                        radioHorizontalRec = temp.GetComponent<RectTransform>();
                        q_main = GameObject.Find("Q_Main");
                        radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                        //ensuring correct placement and scaling in the UI
                        text = temp.GetComponentInChildren<TextMeshProUGUI>();
                        text.text = _qData[i][1];
                        text.transform.localPosition = new Vector3(0, 100 - (i * 100), text.transform.localPosition.z);
                        SetRec(radioHorizontalRec);
                        text.text = _qData[i][1];

                        QuestionList.Add(temp.GetComponent<Slider>().CreateSliderQuestion(qId, qType, qInstructions, _qData[i][0], _qData[i][1], qOptions, i, radioHorizontalRec, _qData[i][2], _qData[i][4], _qData[i][3], _qData[i][5], pId));
                    }
                    break;
                case QuestionnaireTypeNames.Dropdown:
                    for (int i = 0; i < _qData.Count; i++)
                    {
                        temp = Instantiate(Dropdown);
                        temp.name = "dropdown_" + i;
                        radioHorizontalRec = temp.GetComponent<RectTransform>();
                        q_main = GameObject.Find("Q_Main");
                        radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                        //ensuring correct placement and scaling in the UI
                        text = temp.GetComponentInChildren<TextMeshProUGUI>();
                        text.text = _qData[i][1];
                        text.transform.localPosition = new Vector3(0, 120 - (i * 90), text.transform.localPosition.z);
                        SetRec(radioHorizontalRec);

                        QuestionList.Add(temp.GetComponent<Dropdown>().CreateDropdownQuestion(qId, qType, qInstructions, _qData[i][0], _qData[i][1], _qData[i][2].AsArray, i, radioHorizontalRec, pId));
                    }
                    break;
                case QuestionnaireTypeNames.TextInput:
                    Debug.LogError("TextInput is not supported ATM");
                    break;
                case QuestionnaireTypeNames.Task:
                    for (int i = 0; i < _qData.Count; i++)
                    {
                        temp = Instantiate(TaskPrefab);
                        temp.name = "task_" + i;
                        radioHorizontalRec = temp.GetComponent<RectTransform>();
                        q_main = GameObject.Find("Q_Main");
                        radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                        //ensuring correct placement and scaling in the UI
                        text = temp.GetComponentInChildren<TextMeshProUGUI>();
                        text.text = _qData[i][1];
                        text.transform.localPosition = new Vector3(0, 0, text.transform.localPosition.z);
                        SetTextRec(radioHorizontalRec);
                        text.ForceMeshUpdate();

                        QuestionList.Add(temp.GetComponent<TaskPage>().CreateTaskPage(qId, qType, qInstructions, _qData[i][0], string.IsNullOrWhiteSpace(text.text) ? 0 : text.textBounds.size.y, _qData[i][2], _qData[i][3].AsInt, radioHorizontalRec, pId));
                    }
                    break;
                case QuestionnaireTypeNames.NumericInput:
                    for (int i = 0; i < _qData.Count; i++)
                    {
                        temp = Instantiate(NumericInputPrefab);
                        temp.name = "numericInput_" + i;
                        radioHorizontalRec = temp.GetComponent<RectTransform>();
                        q_main = GameObject.Find("Q_Main");
                        radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                        //ensuring correct placement and scaling in the UI
                        text = temp.GetComponentInChildren<TextMeshProUGUI>();
                        text.text = _qData[i][1];
                        text.transform.localPosition = new Vector3(0, 120 - (i * 90), text.transform.localPosition.z);
                        SetTextRec(radioHorizontalRec);

                        QuestionList.Add(temp.GetComponent<NumericInputPage>().CreateNumericInputPage(qId, qType, qInstructions, _qData[i][0], _qData[i][1], radioHorizontalRec, pId));
                    }
                    break;
                case QuestionnaireTypeNames.Text:
                    for (int i = 0; i < _qData.Count; i++)
                    {
                        temp = Instantiate(TextPrefab);
                        temp.name = "text_" + i;
                        radioHorizontalRec = temp.GetComponent<RectTransform>();
                        q_main = GameObject.Find("Q_Main");
                        radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                        //ensuring correct placement and scaling in the UI
                        text = temp.GetComponentInChildren<TextMeshProUGUI>();
                        text.text = _qData[i][1];
                        text.transform.localPosition = new Vector3(0, 0, text.transform.localPosition.z);
                        SetTextRec(radioHorizontalRec);
                        text.ForceMeshUpdate();

                        QuestionList.Add(temp.GetComponent<TextPage>().CreateTextPage(qId, qType, qInstructions, _qData[i][0], string.IsNullOrWhiteSpace(text.text) ? 0: text.textBounds.size.y, _qData[i][2], radioHorizontalRec, pId));
                    }
                    break;
                case QuestionnaireTypeNames.Likert:
                    for (int i = 0; i < _qData.Count; i++)
                    {
                        temp = Instantiate(LikertPrefab);
                        temp.name = "likert_" + i;

                        radioHorizontalRec = temp.GetComponent<RectTransform>();
                        q_main = GameObject.Find("Q_Main");
                        radioHorizontalRec.SetParent(q_main.GetComponent<RectTransform>());

                        //ensuring correct placement and scaling in the UI
                        textArray = temp.GetComponentsInChildren<TextMeshProUGUI>();
                        var questionText = textArray[0];
                        var conditionText = textArray[1];
                        questionText.text = "";

                        // If question mandatory -> add " * " to question in UI
                        /*if (_qData[i][2].AsBool)
                            questionText.text = _qData[0][1] + " *";
                        else*/
                            questionText.text = _qData[0][1];

                        questionText.ForceMeshUpdate();

                        var conditionOffset = string.IsNullOrWhiteSpace(questionText.text) ? 0 : questionText.textBounds.size.y;

                        //Differentiate between 5-Point and 7-Point Likert Scale
                        var t2 = 0;
                        foreach (var option in qOptions)
                        {
                            if (option.Value != "")
                                t2++;
                        }

                        if (t2 == 5)
                        {
                            conditionText.text = qConditions[i][1];
                            conditionText.transform.localPosition = new Vector3(0, -conditionOffset, 0);
                        }
                        else
                        {
                            conditionText.text = qConditions[i][1];
                            conditionText.transform.localPosition = new Vector3(0, -conditionOffset, 0);
                        }
                        conditionText.ForceMeshUpdate();

                        var radioButtonOffset = conditionOffset + conditionText.textBounds.size.y;
                        questionText.transform.localPosition = Vector3.zero;

                        SetTextRec(radioHorizontalRec);
                        QuestionList.Add(temp.GetComponent<LikertPage>().CreateLikertQuestion(qId, qType, qInstructions,
                            _qData[0][0],
                            _qData[0][1], _qData[0][2].AsBool, qOptions, qConditions[i][1], i, radioHorizontalRec, pId,
                            qConditions[i][0], radioButtonOffset));
                    }
                    break;
                default:
                    Debug.LogError("We do not support this questiontype");
                    break;
            }
        }

        #region SetParent
        private void SetRec(RectTransform rec)
        {
            rec.localPosition = new Vector3(0, 0, 0);
            rec.localRotation = Quaternion.identity;
            var localScale = rec.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            rec.localScale = localScale;
            // rec.localScale = new Vector3(1, 1, 1);
        }        
        private void SetTextRec(RectTransform rec)
        {
            rec.localPosition = Vector3.zero;
            rec.anchoredPosition = new Vector3(20, -20);
            rec.localRotation = Quaternion.identity;

            var localScale = rec.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            rec.localScale = localScale;
            // rec.localScale = new Vector3(1, 1, 1);
        }


        //centering the anchor and the position of the RectTransform
        private void CenterRec(RectTransform rec)
        {
            rec.anchorMax = new Vector2(0.5f, 0.5f);
            rec.anchorMin = new Vector2(0.5f, 0.5f);
            rec.pivot = new Vector2(0.5f, 0.5f);
            rec.localPosition = new Vector3(0, 0, 0);
        }


        #endregion

        #region FirstAndLastPage
        public void GenerateAndDisplayFirstAndLastPage(bool firstPage, string qText, string qInstructions)
        {
            GameObject temp;

            if (firstPage)
            {
                _newPage = Instantiate(PagePrefab);
                _newPage.name = "page_first";
                QuestionList = new List<List<GameObject>>();
                PageList = new List<GameObject>();

            }
            else
            {
                _newPage.SetActive(false);
                _newPage = Instantiate(PagePrefab);
                _newPage.name = "page_final";
            }

            //ensuring correct placement and scaling in the UI
            RectTransform pageRec = _newPage.GetComponent<RectTransform>();
            pageRec.SetParent(PageParent);
            SetRec(pageRec);

            //ensuring the anchor of q_panel is centered
            GameObject q_panel = GameObject.Find("Q_Panel");
            RectTransform qPanelRect = q_panel.GetComponent<RectTransform>();
            CenterRec(qPanelRect);

            //display instruction on page
            GameObject q_header = GameObject.Find("Q_Header");
            TextMeshProUGUI descriptionText = q_header.GetComponentInChildren<TextMeshProUGUI>();
            descriptionText.text = qInstructions;

            if (!firstPage)
            {
                GameObject q_footer = GameObject.Find("Q_Footer");
                q_footer.SetActive(false);
            }

            PageList.Add(_newPage);
            NumPages++;

            if (!firstPage)
            {
                temp = Instantiate(LastPagePrefab) as GameObject;
                temp.name = "final";
            }
            else
            {
                temp = Instantiate(LastPagePrefab) as GameObject;
                temp.name = "first";
            }

            RectTransform pageFinalRec = temp.GetComponent<RectTransform>();
            GameObject q_main = GameObject.Find("Q_Main");
            pageFinalRec.SetParent(q_main.GetComponent<RectTransform>());

            //ensuring correct placement and scaling in the UI
            TextMeshProUGUI text = temp.GetComponentInChildren<TextMeshProUGUI>();
            text.text = qText;
            text.transform.localPosition = new Vector3(0, 60, text.transform.localPosition.z);
            SetRec(pageFinalRec);

            _newPage.SetActive(false);
        }


        #endregion

    }
}