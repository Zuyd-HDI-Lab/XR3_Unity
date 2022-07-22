# VR3 Manual
## Researcher UI
When running the application by pressing Play in the unity editor you will first be greeted by the researcher UI. This UI lets you set some thing required to run the experiment.
On the left are all the fields that are required to run the experiment.

| Item | Type | description |
|--|--|--|
| Experiment settings profile | dropdown | Contains all the experiment tasks that can be run |
|Participant ID| text | Participant identifier |
|Session number| text | If the participant has multiple sessions, this number separates them |
|Eye tracking enable| checkbox | Enables eye tracking, checking this will start eye tracking support and will start the required SRAnipal pc application |
|Eye tracking calibrate | Button | Starts the eye tracking calibration software on the HDM, make sure the participant is wearing the headset when running calibration |

On the right is a short welcome message and explanation of the settings on the left.
On the button is a checkbox to indicate the participant has given his consent for data gathering (checked by default) and when everything is set, click Begin to start the experiment.
Note that the participant has to be on the footprint marker before the questionnaire is displayed and the data collection is started.

# File locations
## Input
### UXF/task configuration
All data related to tasks is stored in:
~/VR3_Unity\Assets\StreamingAssets
 - \<experiment name>.json // Contains session and block level settings
 - \<experiment name>_specification.csv //Contains trial level settings

### Questionnaire
Questionnaires are stored in
~/VR3_Unity\Assets\External\Questionnaires\Data\Questions
 - \<experiment name>.json \\ questionnaire questions

## Output
The output is written to ~/VR3_Unity/data_output/<experiment name>/<participant id>
The ouput path for participant id 1 would be:
~/VR3_Unity/data_output/Sonification/1
The folder contains

 - Minimap
	 - \<Timestamp>.json // The minimap data
 - <S###, the session id>
	 - other
		 - Multiple files containing raw tracker data
	 - Questionnaire 
		 - questionnaireID_Soni_participantID_id2_condition_AB_answers.csv // the questionnaire answers
		 - \<questionnairename>.json // the questionnaire data used
	 - session_info
		 - log.csv // all unity logmessages
		 - participant_details.csv // aditional participant details (currently eye tracking enabled state and timestamp offset from start of application to start of session)
		 - settings.json // uxf settings file used
	 - trial_results.csv // The results of the trial tasks
 - Participant audio.wav // The audio the participant heard during the experiment
 - Participant view.mp4 // The video feed from the participant perspective.

# Questionnaire
## Questionnaire example

    {
      "qTitle": "Welcome",
      "qInstructions": "Welcome to the experiment \n\n You are participating in an experiment about location detection in VR.",
      "qId": "Soni",
      "questions": [
        {
          "pId": "p1",
          "qType": "text",
          "qInstructions": "Welcome",
          "qData": [
            {
              "qId": "w1",
              "qTextBold": "",
              "qText": "Before we begin you will be asked to complete a short tutorial in order to get used to the tool. \n\nLet’s get started.",
              "qMandatory": "false"
            }
          ]
        },
        {
          "pId": "p2",
          "qType": "text",
          "qInstructions": "Tutorial",
          "qData": [
            {
              "qId": "tt1",
              "qTextBold": "",
              "qText": "You see eight sphere around you on the horizontal plain. \n\nFrom the controller in your hand, you can see a laser beam. ",
              "qMandatory": "false"
            }
          ]
        },
        {
          "pId": "p3",
          "qType": "task",
          "qInstructions": "Tutorial",
          "qData": [
            {
              "qId": "tt2",
              "qTextBold": "",
              "qText": "To move to the next step, direct the beam onto any of the sphere around you and press the button with your pointer finger to \n\nselect the sphere.",
              "taskId": 9
            }
          ]
        },
        {
          "pId": "p4",
          "qType": "text",
          "qInstructions": "Tutorial",
          "qData": [
            {
              "qId": "tt3",
              "qTextBold": "",
              "qText": "Great. You now know how to select a sphere. \n\nOnce you go to the next step you will hear a sound. Select the sphere that you believe the sound came from.",
              "qMandatory": "false"
            }
          ]
        },
        {
          "pId": "p5",
          "qType": "task",
          "qInstructions": "Tutorial",
          "qData": [
            {
              "qId": "tt4",
              "qTextBold": "",
              "qText": "Please select the sphere that the sound originated from.",
              "taskId": 10
            }
          ]
        },
        {
          "pId": "p6",
          "qType": "text",
          "qInstructions": "Tutorial",
          "qData": [
            {
              "qId": "tt5",
              "qTextBold": "",
              "qText": "Great. You are now ready to start the experiment.  \n\nIf you have any questions left, please ask the researcher now. If you feel comfortable with the tool please move to the next step.",
              "qMandatory": "false"
            }
          ]
        },
        {
          "pId": "p9",
          "qType": "linearSlider",
          "qInstructions": "Questionnaire",
          "qData": [
            {
              "qId": "q1",
              "qText": "How old are you?",
              "qMin": "0",
              "qMinLabel": "",
              "qMax": "100",
              "qMaxLabel": ""
            }
          ]
        },
        {
          "pId": "p10",
          "qType": "dropdown",
          "qInstructions": "Questionnaire",
          "qData": [
            {
              "qId": "q2",
              "qText": "What is your gender?",
              "qOptions": [
                "Male",
                "Female",
                "Prefer not to say"
              ]
            }
          ]
        },
        {
          "pId": "p11",
          "qType": "numericInput",
          "qInstructions": "Questionnaire",
          "qData": [
            {
              "qId": "q3",
              "qText": "What is your height?",
              "qMandatory": "true"
            }
          ]
        },
        {
          "pId": "p12",
          "qType": "dropdown",
          "qInstructions": "Questionnaire",
          "qData": [
            {
              "qId": "q4",
              "qText": "Do you have a sight impairment?",
              "qOptions": [
                "No",
                "Yes, Nearsighted",
                "Yes, Farsighted",
                "Yes, Other"
              ]
            }
          ]
        },
        {
          "pId": "p16",
          "qType": "likert",
          "qInstructions": "Questionnaire",
          "qOptions": [
            "Strongly Disagree",
            "Disagree",
            "Somewhat Disagree",
            "Neutral",
            "Somewhat Agree",
            "Agree",
            "Strongly Agree"
          ],
          "qConditions": [
            {
              "qId": "q7.1",
              "qText": "Learning to operate this application would be easy for me."
            }
          ],
          "qData": [
            {
              "qId": "q7",
              "qText": "Based on your perception, to what extent do you agree with the following statements",
              "qMandatory": "true"
            }
          ]
        }
      ],
      "qMessage": "Thank you for your participation",
      "qAcknowledgments": "You can now remove your glasses."
    }

## Format
The questionnaire input file consist of a number of pages with a question type on each.
The qTitle (header) and qInstructions (content)d on the root node are displayed on the first page (introduction)
and qMessage (header) and qAcknowledgments (content) on the last page (thanks/end).

A question in has the following base format
| Property | type | description |
|--|--|--|
| pId  | string  | Page id | 
| qType | string | Question type, supported are text, task, dropdown, likert, radioGrid, numericInput, linearSlider|
|qInstructions| string| header text|
|qData| object| type specific data|

### Type specific
#### qData
| Property | type | description |
|--|--|--|
| qId  | string  | Question id | 
| qText| string | Question text|
| qMandatory | string | Answer has to be given to proceed |
| qTextBold | string | Text displayed in bold above the qText |
| taskId | string | Id of the task to be run when leaving the page |

 - qId, qText and qTextBold  are supported by all types.
 - qMandatory is supported by all types, except Task and Text which ignore this value. 
	 - Task is always mandatory, as the task must be completed in order to proceed
	 - Text is never mandatory as it has no input
 - taskId is only supproted by Task

The amount of text supported by qText and qTextBold varies depending on multiple factors including type, conditions and number of types on a page.
qTextBold is always displayed on the top of the page, and displaces qText downward.

#### qOptions
qOptions is an array of options to display

 - In a likert: A horizontal list of options from which one can be chosen, either 5 or 7 long. Generates a radio button for eacht with the text displayed below the radio button.
 - In a radioGrid: A horizontal list of options from which one can be chosen, either 5 or 7 long. Generates a list of radio buttons for each condition, with the text above the first line of radio buttons.
 - In a dropdown a vertical list of options from which one can be chosen.

#### qConditions
| Property | type | description |
|--|--|--|
| qId  | string  | Question id | 
| qText| string | Question text|

qConditions is an array of questions to display.

 - In a likert. This displays the question above the radio buttons, supports only one condition
 - In a radioGrid this generates a line of radio buttons (based on the number of options), with the question in front of the radio buttons supports up to four. The space is limited to single word questions without it overflowing.

# Tasks
## Task types
Currently two task types are supported
Tutorial and Spheres.
### Tutorial task
The tutorial task will display all spheres and not hide the questionnaire. It will also display the controller button the participant has to press with a call out and button highlight.
It does not matter what sphere the participant chooses and the results are not stored.

It is possible to either have the spheres make a sound by specifying an existing sphere Id or if no sound should be heard -1 as id.

### Spheres task
The spheres task displays all spheres as specified in the trial specification. They can either be visible, visible with the correct sphere highlighted and invisible. The target sphere will make a sound if the sphere ID is provided. Spheres will remain in the last state (visible/highlighted) they were after a block but will not be clickable and the laser pointer will not collide with the spheres anymore.

After each trial, results are collected and stored.

Tasks are specified in the UXF configuration files. It currently expects there to be a session file (\<experiment name>.json) and a trial file (\<experiment name>_specification.csv).
## Session file

     {
      "trial_specification_name": "sonification_specification.csv",
      "trials_per_block": 5,
      "delay_time": 5,
      "block1_task_type": "Spheres",
      "block2_task_type": "Spheres",
      "block3_task_type": "Spheres",
      "block4_task_type": "Spheres",
      "block5_task_type": "Spheres",
      "block6_task_type": "Spheres",
      "block7_task_type": "Spheres",
      "block8_task_type": "Spheres",
      "block9_task_type": "Tutorial"
      "block10_task_type": "Tutorial"
    }

| Property | type | description |
|--|--|--|
| trial_specification_name | string  | name of the trial specification file| 
| trials_per_block | int| currently not used, information is derived from trials file|
|delay_time|int|time in seconds to wait (and display timer) before a task starts|
|block1_task_type|string| Type of task a block has, supported Spheres, Tutorial|

The block task format is important it should be block\<blocknumber>_task_type and the blocknumber has to exists in the specification file. If there is no task type defined, Tutorial will be assumed.

## Trial specification file

    block_num,visible,target_id,highlighted
    1,TRUE,1,FALSE
    1,TRUE,3,FALSE
    1,TRUE,5,FALSE
    2,TRUE,6,TRUE
    3,FALSE,3,FALSE
    4,TRUE,2,FALSE
    5,TRUE,6,FALSE
    6,TRUE,7,FALSE
    7,TRUE,1,FALSE
    8,TRUE,4,FALSE
    9,TRUE,-1,FALSE
    10,TRUE,1,FALSE
The specification file contains the data needed for each trial. Currently the following is required.

|Column | type |  description |
|--|--|--|
| block_num  | int | block number |
| visible | bool | Spheres visible |
| target_id| int| Sphere that is the target (0 - 7)|
| Highlighted | bool | Target sphere is highlighted |

The block number is also the task number

Each line in the file represents a trial, and by specifying the block number trials are grouped in blocks.
So if you want task 1 to repeat 5 times, you need 5 lines starting with 1 as block number. This way each repeated trial can be unique.

# Linking tasks to questionnaire
To start a task from the questionnaire, you have to have a question of type task and specify the block number as the task id. 

When the task is started, all the settings for the block and trials will be read based on that and the type of the task based on the task type specified for the block.

# Changing the questionnaire
To change the active questionnaire, you currently have to change it on the object in the editor.

In the hierachy (usually on the left side of the scene\game view) find 

 - Experiment
	 - QuestionnaireController
		 - VRQuestionnaireToolkit

On the VRQuestionnaireToolkit in the inspector (usually on the right of the scene\game view) find the **GenerateQuestionnaire** component which has a Json Input Path_1 text field.
The default is 
Assets/External/Questionnaires/Data/Questions/sonification.json
And can be changed to any other file (taken in to account the rootpath is the unity VR3 project folder).


