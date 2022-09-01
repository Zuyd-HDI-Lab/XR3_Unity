using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Globalization;
using Constants;
using Experiment;
using Questionnaire;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Application = UnityEngine.Application;
using UnityEngine.Networking;

namespace VRQuestionnaireToolkit
{
    /// <summary>
    /// ExportToCSV.class
    /// 
    /// version 1.0
    /// date: July 1st, 2020
    /// authors: Martin Feick & Niko Kleer
    /// modified by Robert Vissers
    /// data: 2022-07-01
    /// </summary>
    public class ExportToCSV : MonoBehaviour
    {
        public string FileName;
        public string Delimiter;
        public enum FileType
        {
            Csv,
            Txt
        }
        public FileType Filetype;

        [Header("Configure if you want to save the results to local storage:")]
        [Tooltip("Save results locally on this device.")]
        private bool SaveToLocal = true;
        /*public string StorePath;
        public bool UseGlobalPath;*/

        /*
        [Header("Configure if you want to save the results to remote server:")]
        public bool SaveToServer = false;
        [Tooltip("The target URI to send the results to")]
        public string TargetURI = "http://www.example-server.com/survey-results.php";*/

        private List<string[]> _csvRows;
        private GameObject _pageFactoryGameObject;
        private GameObject _vrQuestionnaireToolkit;
        private StudySetup _studySetup;
        private string _folderPath;
        private string _fileType;
        private string _questionnaireID;
        private string[] csvTitleRow = new string[8];

        public UnityEvent QuestionnaireFinishedEvent;

        // Use this for initialization
        void Start()
        {
            var experiment = GameObject.Find(GameObjectNames.Experiment).GetComponent<IExperiment>();
            _vrQuestionnaireToolkit = GameObject.FindGameObjectWithTag("VRQuestionnaireToolkit");
            _studySetup = _vrQuestionnaireToolkit.GetComponent<StudySetup>();
            // _folderPath = UseGlobalPath ? StorePath : Application.dataPath + StorePath;
            _folderPath = Path.Combine(experiment.OutputLocation, "Questionnaire");

            if (QuestionnaireFinishedEvent == null)
                QuestionnaireFinishedEvent = new UnityEvent();

            if (Filetype == 0)
                _fileType = "csv";
            else
            {
                _fileType = "txt";
            }

            /*
            if (!(SaveToLocal /*| SaveToServer#1#)) // if neither of the box is checked, warn the user that the data won't be saved.
            {
                Debug.LogError("You have chosen to save the results NEITHER locally NOR remotely. Please consider going to the inspector of ExportToCSV and check one of the save-to options, otherwise your data will be lost!!");
            }
            else
            {
                if (SaveToLocal)
                {
                    try // create a new folder if the specified folder does not exist.
                    {
                        if (!Directory.Exists(_folderPath))
                        {
                            Directory.CreateDirectory(_folderPath);
                            Debug.LogWarning("Local folder path does not exist! New folder created at " + _folderPath);
                        }
                    }
                    catch (IOException ex)
                    {
                        Debug.Log(ex.Message);
                    }
                }

                /*if (SaveToServer)
                {
                    // check if the provided uri is valid
                    StartCoroutine(CheckURIValidity(TargetURI));
                }#1#
            }*/
        }

        public void Save()
        {
            try // create a new folder if the specified folder does not exist.
            {
                if (!Directory.Exists(_folderPath))
                {
                    Directory.CreateDirectory(_folderPath);
                    Debug.LogWarning("Local folder path does not exist! New folder created at " + _folderPath);
                }
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }

            // TODO never used
            var currentQuestionnaire = 1;
            var generateQuestionnaire = _vrQuestionnaireToolkit.GetComponent<GenerateQuestionnaire>();
            for (int i = 0; i < generateQuestionnaire.Questionnaires.Count; i++)
            {
                if (generateQuestionnaire.Questionnaires[i].gameObject.activeSelf)
                    currentQuestionnaire = i + 1;
            }


            _pageFactoryGameObject = GameObject.FindGameObjectWithTag("QuestionnaireFactory");
            var pageFactory = _pageFactoryGameObject.GetComponent<PageFactory>();
            _csvRows = new List<string[]>();

            // create title rows
            csvTitleRow[0] = "PageId";
            csvTitleRow[1] = "QuestionType";
            csvTitleRow[2] = "Question";
            csvTitleRow[3] = "QuestionID";
            csvTitleRow[4] = "ConditionID";
            csvTitleRow[5] = "Answer";
            csvTitleRow[6] = "StartTime";
            csvTitleRow[7] = "EndTime";
            _csvRows.Add(csvTitleRow);

            // enable all GameObjects (except the first and last page) in order to read the responses
            for (var i = 1; i < pageFactory.NumPages - 1; i++)
                pageFactory.PageList[i].SetActive(true);

            #region CONSTRUCTING RESULTS
            // read participants' responses 
            // TODO name questions??? Is it a list of questsions?
            foreach (var questions in pageFactory.QuestionList)
            {
                if (questions == null) continue;
                
                var csvTemp = new string[8];

                if (questions[0].GetComponentInParent<Radio>() != null)
                {
                    var radio = questions[0].GetComponentInParent<Radio>();
                    _questionnaireID = radio.QuestionnaireId;
                    csvTemp[0] = radio.PId;
                    csvTemp[1] = radio.QType;
                    csvTemp[2] = radio.QText;
                    csvTemp[3] = radio.QId;
                    //TODO FORMAT
                    csvTemp[6] = radio.StartTime.ToString("0.######", CultureInfo.InvariantCulture);
                    csvTemp[7] = radio.EndTime.ToString("0.######", CultureInfo.InvariantCulture);

                    for (var j = 0;
                         j < radio.RadioList.Count;
                         j++)
                    {
                        if (!questions[j].GetComponentInChildren<Toggle>().isOn) continue;
                        
                        if (_questionnaireID != "SSQ")
                        {
                            csvTemp[4] = questions[j].GetComponentInChildren<TextMeshProUGUI>().text;
                            csvTemp[5] = "" + (j + 1);
                        }
                        else
                        {
                            csvTemp[5] = "" + j;
                        }
                    }
                    _csvRows.Add(csvTemp);
                }
                else if (questions[0].GetComponentInParent<LinearGrid>() != null)
                {
                    var linearGrid = questions[0].GetComponentInParent<LinearGrid>();
                    _questionnaireID = linearGrid.QuestionnaireId;
                    csvTemp[0] = linearGrid.PId;
                    csvTemp[1] = linearGrid.QType;
                    csvTemp[2] = linearGrid.QText;
                    csvTemp[3] = linearGrid.QId;
                    //TODO FORMAT
                    csvTemp[6] = linearGrid.StartTime.ToString("0.######", CultureInfo.InvariantCulture);
                    csvTemp[7] = linearGrid.EndTime.ToString("0.######", CultureInfo.InvariantCulture);


                    for (var j = 0;
                         j < linearGrid.LinearGridList.Count;
                         j++)
                    {
                        if (questions[j].GetComponentInChildren<Toggle>().isOn)
                        {
                            csvTemp[4] = questions[j].GetComponentInChildren<TextMeshProUGUI>().text;
                            csvTemp[5] = "" + (j + 1);
                        }
                    }
                    _csvRows.Add(csvTemp);
                }
                else if (questions[0].GetComponentInParent<RadioGrid>() != null)
                {
                    var radioGrid = questions[0].GetComponentInParent<RadioGrid>();
                    _questionnaireID = radioGrid.QuestionnaireId;
                    csvTemp[0] = radioGrid.PId;
                    csvTemp[1] = radioGrid.QType;
                    //csvTemp[2] = radioGrid.QConditions + "_" + radioGrid.QText;
                    csvTemp[2] = radioGrid.QText;
                    csvTemp[3] = radioGrid.QId;
                    //TODO FORMAT
                    csvTemp[6] = radioGrid.StartTime.ToString("0.######", CultureInfo.InvariantCulture);
                    csvTemp[7] = radioGrid.EndTime.ToString("0.######", CultureInfo.InvariantCulture);

                    for (var j = 0;
                         j < radioGrid.RadioList.Count;
                         j++)
                    {
                        if (questions[j].GetComponentInChildren<Toggle>().isOn)
                        {
                            csvTemp[4] = radioGrid.CId;
                            csvTemp[5] = "" + (j + 1);
                        }
                    }
                    _csvRows.Add(csvTemp);
                }
                else if (questions[0].GetComponentInParent<Checkbox>() != null)
                {
                    var checkbox = questions[0].GetComponentInParent<Checkbox>();
                    _questionnaireID = checkbox.QuestionnaireId;

                    for (var j = 0;
                         j < checkbox.CheckboxList.Count;
                         j++)
                    {
                        csvTemp = new string[8];
                        csvTemp[0] = checkbox.PId;
                        csvTemp[1] = checkbox.QType;
                        csvTemp[2] = checkbox.QText;
                        csvTemp[3] = checkbox.QId;
                        csvTemp[4] = questions[j].GetComponentInParent<Checkbox>().QOptions[j];
                        csvTemp[5] = (questions[j].GetComponentInChildren<Toggle>().isOn ? ("" + 1) : ""); // 1 if checked, blank if unchecked
                        //TODO FORMAT
                        csvTemp[6] = checkbox.StartTime.ToString("0.######", CultureInfo.InvariantCulture);
                        csvTemp[7] = checkbox.EndTime.ToString("0.######", CultureInfo.InvariantCulture);
                        _csvRows.Add(csvTemp);
                    }
                }
                else if (questions[0].GetComponentInParent<Slider>() != null)
                {
                    var slider = questions[0].GetComponentInParent<Slider>();
                    _questionnaireID = slider.QuestionnaireId;
                    csvTemp[0] = slider.PId;
                    csvTemp[1] = slider.QType;
                    csvTemp[2] = slider.QText;
                    csvTemp[3] = slider.QId;
                    csvTemp[4] = ""; // Has no conditionID
                    //TODO FORMAT
                    csvTemp[6] = slider.StartTime.ToString("0.######", CultureInfo.InvariantCulture);
                    csvTemp[7] = slider.EndTime.ToString("0.######", CultureInfo.InvariantCulture);

                    for (var j = 0;
                         j < slider.SliderList.Count;
                         j++)
                    {
                        csvTemp[5] = "" + questions[j].GetComponentInChildren<UnityEngine.UI.Slider>().value;
                    }
                    _csvRows.Add(csvTemp);
                }
                else if (questions[0].GetComponentInParent<Dropdown>() != null)
                {
                    var dropdown = questions[0].GetComponentInParent<Dropdown>();
                    _questionnaireID = dropdown.QuestionnaireId;
                   csvTemp[0] = dropdown.PId;
                   csvTemp[1] = dropdown.QType;
                   csvTemp[2] = dropdown.QText;
                    csvTemp[3] = dropdown.QId;
                    //TODO FORMAT
                    csvTemp[6] = dropdown.StartTime.ToString("0.######", CultureInfo.InvariantCulture);
                    csvTemp[7] = dropdown.EndTime.ToString("0.######", CultureInfo.InvariantCulture);

                    for (var j = 0;
                         j < dropdown.DropdownList.Count;
                         j++)
                    {
                        var dropdownValue = questions[j].GetComponentInChildren<TMP_Dropdown>().value;

                        csvTemp[4] = questions[j].GetComponentInParent<Dropdown>().QOptions[dropdownValue];
                        csvTemp[5] = dropdownValue.ToString();
                    }
                    _csvRows.Add(csvTemp);
                }
                else if (questions[0].GetComponentInParent<TaskPage>() != null)
                {
                    var taskPage = questions[0].GetComponentInParent<TaskPage>();
                    _questionnaireID = taskPage.QuestionnaireId;
                    csvTemp[0] = taskPage.PId;
                    csvTemp[1] = taskPage.QType;
                    csvTemp[2] = taskPage.QText;
                    csvTemp[3] = taskPage.QId;
                    csvTemp[4] = ""; // Has no conditionID
                    csvTemp[5] = $"{taskPage.TaskId}: TODO RESULTS";
                    //TODO FORMAT
                    csvTemp[6] = taskPage.StartTime.ToString( "0.######", CultureInfo.InvariantCulture);
                    csvTemp[7] = taskPage.EndTime.ToString("0.######", CultureInfo.InvariantCulture);
                    _csvRows.Add(csvTemp);
                }
                else if (questions[0].GetComponentInParent<NumericInputPage>() != null)
                {
                    var numericInputPage = questions[0].GetComponentInParent<NumericInputPage>();
                    _questionnaireID = numericInputPage.QuestionnaireId;
                    csvTemp[0] = numericInputPage.PId;
                    csvTemp[1] = numericInputPage.QType;
                    csvTemp[2] = numericInputPage.QText;
                    csvTemp[3] = numericInputPage.QId;
                    csvTemp[4] = ""; // Has no conditionID
                    csvTemp[5] = numericInputPage.Value;
                    //TODO FORMAT
                    csvTemp[6] = numericInputPage.StartTime.ToString("0.######", CultureInfo.InvariantCulture);
                    csvTemp[7] = numericInputPage.EndTime.ToString("0.######", CultureInfo.InvariantCulture);
                    _csvRows.Add(csvTemp);
                }
                else if (questions[0].GetComponentInParent<LikertPage>() != null)
                {
                    var likert = questions[0].GetComponentInParent<LikertPage>();
                    _questionnaireID = likert.QuestionnaireId;
                    csvTemp[0] = likert.PId;
                    csvTemp[1] = likert.QType;
                    //csvTemp[2] = radioGrid.QConditions + "_" + radioGrid.QText;
                    csvTemp[2] = likert.QText;
                    csvTemp[3] = likert.QId;
                    //TODO FORMAT
                    csvTemp[6] = likert.StartTime.ToString("0.######", CultureInfo.InvariantCulture);
                    csvTemp[7] = likert.EndTime.ToString("0.######", CultureInfo.InvariantCulture);

                    for (var j = 0;
                         j < likert.RadioList.Count;
                         j++)
                    {
                        if (questions[j].GetComponentInChildren<Toggle>().isOn)
                        {
                            csvTemp[4] = likert.CId;
                            csvTemp[5] = "" + (j + 1);
                        }
                    }
                    _csvRows.Add(csvTemp);
                }
            }
            #endregion

            // disable all GameObjects (except the last page) 
            for (int i = 1; i < pageFactory.NumPages - 1; i++)
                pageFactory.PageList[i].SetActive(false);


            //-----Processing responses into the specified data format-----//

            string _completeFileName = "questionnaireID_" + _questionnaireID + "_participantID_" + _studySetup.ParticipantId + "_condition_" + _studySetup.Condition + "_" + FileName + "." + _fileType;
            string _completeFileName_allResults = "questionnaireID_" + _questionnaireID + "_ALL_" + FileName + "." + _fileType;
            string _path = Path.Combine(_folderPath, _completeFileName);
            string _path_allResults = Path.Combine(_folderPath, _completeFileName_allResults);


            string[][] output = new string[_csvRows.Count][];
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = _csvRows[i]; // copy all data to a 2d-array of string
            }

            StringBuilder contentOfResult = new StringBuilder();

            for (int index = 0; index < output.GetLength(0); index++)
                contentOfResult.AppendLine(string.Join(Delimiter, output[index]));

            /* WRITING RESULTS TO LOCAL STORAGE */
            if (SaveToLocal)
            {
                WriteToLocal(_path, contentOfResult);
            }

            /* SENDING RESULTS TO REMOTE SERVER */
            /*if (SaveToServer)
            {
                StartCoroutine(SendToServer(TargetURI, _completeFileName, contentOfResult.ToString()));
            }*/

            /* CONSOLIDATING RESULTS */
            if (_studySetup.AlsoConsolidateResults)
            {
                StringBuilder content_all_results = GetConsolidatedContent(_path_allResults, output);
                
                if (SaveToLocal)
                {
                    WriteToLocal(_path_allResults, content_all_results);
                }

                /*
                if (SaveToServer)
                {
                    StartCoroutine(SendToServer(TargetURI, _completeFileName_allResults, content_all_results.ToString()));
                }*/
            }

            if (_studySetup.CopyQuestionsToOutput)
            {
                // TODO suppport multiple in a decent way in questionnaire
                var fileName = Path.GetFileName(generateQuestionnaire.JsonInputPath_1);
                File.Copy(generateQuestionnaire.JsonInputPath_1, Path.Combine(_folderPath, fileName), true);
            }

            QuestionnaireFinishedEvent.Invoke(); //notify 
        }

        /// <summary>
        /// Consolidate all results to a StringBuilder, written to be directly written.
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="newData"></param>
        /// <returns></returns>
        StringBuilder GetConsolidatedContent(string filepath, string[][] newData)
        {
            StringBuilder sb_all_content = new StringBuilder();

            string header = "Answer_Participant_" + _studySetup.ParticipantId + "_condition_" + _studySetup.Condition; // header for this current participant

            try
            {
                if (!File.Exists(filepath))
                {
                    sb_all_content.AppendLine(csvTitleRow[0] + Delimiter + csvTitleRow[1] + Delimiter + csvTitleRow[2] + Delimiter + header); // first row being the headers
                    for (int row = 1; row < newData.GetLength(0); row++) // from the second row
                    {
                        sb_all_content.AppendLine(string.Join(Delimiter, newData[row]));
                    }
                }
                else
                {
                    StreamReader sr = new StreamReader(filepath);
                    sb_all_content.AppendLine(sr.ReadLine() + Delimiter + header); // copy the first row in the existing file and add a header for the new data
                    for (int row = 1; row < newData.GetLength(0); row++) // from the second row
                    {
                        sb_all_content.AppendLine(sr.ReadLine() + Delimiter + newData[row][3]); // copy old data and add new data
                    }
                    sr.Close();
                }
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }
            return sb_all_content;
        }

        /// <summary>
        /// Write a StringBuilder to a local file.
        /// </summary>
        /// <param name="localPath"></param>
        /// <param name="content"></param>
        void WriteToLocal(string localPath, StringBuilder content)
        {
            print("Answers stored in path: " + localPath);
            try
            {
                content.Replace("\n\n", " "); // replace newline with space to avoid newline in the csv file
                StreamWriter outStream = System.IO.File.CreateText(localPath);
                outStream.WriteLine(content);
                outStream.Close();
            }
            catch (IOException ex)
            {
                Debug.Log(ex.Message);
            }
        }

        // TODO reimplement when network transfer becomes a thing
        /*/// <summary>
        /// Post data to a specific server location.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="filename"></param>
        /// <param name="inputData"></param>
        /// <returns></returns>
        IEnumerator SendToServer(string uri, string filename, string inputData)
        {
            WWWForm form = new WWWForm();
            form.AddField("fileName", filename);
            form.AddField("inputData", inputData);

            using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
            {
                yield return www.SendWebRequest();

                if (www.isHttpError || www.isNetworkError)
                {
                    Debug.LogError(www.error + "\nPlease check the validity of the server URI.");
                }
                else
                {
                    string responseText = www.downloadHandler.text;
                    Debug.Log("Message from the server: " + responseText);
                }
            }
        }*/

        /*/// <summary>
        /// Check if the provided server URI is valid.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        IEnumerator CheckURIValidity(string uri)
        {
            UnityWebRequest www = new UnityWebRequest(uri);
            yield return www.SendWebRequest();

            if (www.isHttpError || www.isNetworkError)
            {
                Debug.LogError(www.error + "\nPlease check the validity of the server URI.");
            }
        }*/
    }
}

