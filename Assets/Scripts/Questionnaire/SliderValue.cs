using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SliderValue : MonoBehaviour
{
    private TextMeshProUGUI valueText;
    // Start is called before the first frame update
    void Start()
    {
        valueText = GetComponent<TextMeshProUGUI>();
    }


    public void UpdateValue(float value)
    {
        if (valueText)
            valueText.text = ((int)value).ToString();
    }
}
