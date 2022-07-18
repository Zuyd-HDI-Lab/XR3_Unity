using System;
using UnityEngine;
using UnityEngine.Events;

namespace Questionnaires.Scripts.Events
{
    [Serializable]
    public class QuestionnaireTaskEvent: UnityEvent<QuestionnaireTaskEventArgs>
    {
    }
    [Serializable]
    public class QuestionnaireTaskEventArgs : EventArgs
    {
        public int TaskId { get; set; }

        public string TaskResult { get; set; }
    }
}
