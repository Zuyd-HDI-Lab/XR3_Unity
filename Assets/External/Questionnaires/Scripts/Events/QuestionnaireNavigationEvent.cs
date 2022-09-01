using System;
using UnityEngine;
using UnityEngine.Events;

namespace Questionnaires.Scripts.Events
{
    [Serializable]
    public class QuestionnaireNavigationEvent: UnityEvent<QuestionnaireNavigationEventArgs>
    {
    }
    [Serializable]
    public class QuestionnaireNavigationEventArgs : EventArgs
    {
        public GameObject From { get; set; }
        public GameObject To { get; set; }
    }
}
