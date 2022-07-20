using System.Collections;
using System.Collections.Generic;
using Questionnaire;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Slider.class
/// 
/// version 1.0
/// date: July 1st, 2020
/// authors: Martin Feick & Niko Kleer
/// </summary>

namespace VRQuestionnaireToolkit
{
    public class Slider : QuestionnairePage
    {
        public int NumSlider;
        public int QMin;
        public int QMax;

        private string _qMinLabel;
        private string _qMaxLabel;
        private Sprite _sprite;

        public GameObject Sliders;
        public JSONArray QOptions;

        public List<GameObject> SliderList; //contains all radiobuttons which correspond to one question

        public List<GameObject> CreateSliderQuestion(string questionnaireId, string qType, string qInstructions, string qId, string qText, JSONArray qOptions, int numberQuestion, RectTransform questionRec, int qMin, int qMax, string qMinxLabel, string qMaxLabel, string pId)
        {
            PId = pId;
            QuestionnaireId = questionnaireId;
            QId = qId;
            QType = qType;
            QInstructions = qInstructions;
            QText = qText;
            QOptions = qOptions;
            NumSlider = numberQuestion;
            _questionRecTest = questionRec;
            QMin = qMin;
            QMax = qMax;
            _qMaxLabel = qMaxLabel;
            _qMinLabel = qMinxLabel;

            SliderList = new List<GameObject>();

            // generate sliders and corresponding labels on a single page
            if (QText == "") return SliderList;
            
            if (NumSlider <= 7)
                InitSlider(NumSlider);
            else
            {
                Debug.LogError("We currently only support up to 7 sliders on one page");
            }

            return SliderList;
        }

        private void InitSlider(int numQuestions)
        {
            // Instantiate slider prefabs
            var temp = Instantiate(Sliders);
            temp.name = "slider_" + numQuestions;

            // Use this for initialization
            _sprite = LoadSprite(QMax);
            temp.GetComponentInChildren<UnityEngine.UI.Slider>().GetComponent<Image>().sprite = _sprite;

            // Set required slider properties
            temp.GetComponentInChildren<UnityEngine.UI.Slider>().minValue = QMin;
            temp.GetComponentInChildren<UnityEngine.UI.Slider>().maxValue = QMax;
            /*temp.GetComponentInChildren<UnityEngine.UI.Slider>().GetComponentsInChildren<TextMeshProUGUI>()[0].text =
                _qMinLabel;
            temp.GetComponentInChildren<UnityEngine.UI.Slider>().GetComponentsInChildren<TextMeshProUGUI>()[1].text =
                _qMaxLabel;*/

            //Set Slider start value
            temp.GetComponentInChildren<UnityEngine.UI.Slider>().value = QMax % 2 == 0 ? (int)QMax / 2 : 0;

            // Place in hierarchy 
            var sliderRec = temp.GetComponent<RectTransform>();
            sliderRec.SetParent(_questionRecTest);
            sliderRec.localPosition = new Vector3(0, 80 - (numQuestions * 100), 0);
            sliderRec.localRotation = Quaternion.identity;
            var localScale = sliderRec.localScale;
            localScale = new Vector3(localScale.x * 0.01f, localScale.y * 0.01f, localScale.z * 0.01f);
            sliderRec.localScale = localScale;

            SliderList.Add(temp);
        }

        private Sprite LoadSprite(int numberTicks)
        {
            var load = "Sprites/Slider_" + (numberTicks + 1);
            var temp = Resources.Load<Sprite>(load);

            return temp;
        }

    }
}