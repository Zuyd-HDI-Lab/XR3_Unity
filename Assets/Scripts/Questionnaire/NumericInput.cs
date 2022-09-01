using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Questionnaire
{
    public class NumericInput : MonoBehaviour
    {
        public string Value { get; set; }

        public static string ZeroValue = "0";

        private TextMeshProUGUI TextValue;

        //public UnityEvent<string> OnSubmit;

        private void Start()
        {
            //OnSubmit = new UnityEvent<string>();            
            // TODO fix
            TextValue = GetComponentInChildren<TextMeshProUGUI>();
            Value = ZeroValue;
        }

        private void Update()
        {
            TextValue.text = Value;
        }

        public void ClearInput()
        {
            Value = ZeroValue;

            SubmitInput();
        }
        
        public void AddNumber(string number)
        {
            if (Value == ZeroValue)
            {
                Value = number;
            }
            else
            {
                Value += number;
            }
            SubmitInput();
        }

        public void SubmitInput()
        {
            // OnSubmit?.Invoke(Value);
            EventManager.Trigger<NumericInputSubmitted>(new NumericInputSubmitted
            {
                Value = Value,
                Sender = this.gameObject
            });
        }
    }

    public class NumericInputSubmitted
    {
        public GameObject Sender { get; set; }
        public string Value { get; set; }
    }
}
