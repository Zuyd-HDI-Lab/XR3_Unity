using Constants;
using UnityEngine;

namespace Questionnaire
{
    public class QuestionnairePage : MonoBehaviour
    {
        protected QuestionnaireController questionnaireController;
        protected RectTransform _questionRecTest;

        public string PId;
        public string QuestionnaireId;
        public string QId;
        public string QType;
        public string QInstructions;
        public string QText;
        public bool QMandatory;
        
        public float StartTime;
        public float EndTime;

        private bool started = false;

        private void Start()
        {
            // TODO improve retrieval of questionnaire controller script
            questionnaireController = GameObject.Find(GameObjectNames.QuestionnaireController)?.GetComponent<QuestionnaireController>();
        }

        private void OnDisable()
        {
            EndTime = Time.time;
        }
    }
}
