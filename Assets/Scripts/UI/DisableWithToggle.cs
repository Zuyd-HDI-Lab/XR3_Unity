using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

public class DisableWithToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle toggle;
    [SerializeField]
    public List<GameObject> objectsToToggle;
    // Start is called before the first frame update
    void Awake()
    {
        if (!toggle)
            toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(ToggleAllGameObjects);

    }

    private void Start()
    {
        ToggleAllGameObjects(toggle.isOn);
    }

    private void ToggleAllGameObjects(bool value)
    {
        foreach (var go in objectsToToggle)
        {
            var selectable = go.GetComponent<Selectable>();

            if (selectable)
            {
                selectable.interactable = value;
            }
            else
            {
                go.SetActive(value);
            }
        }
    }

    private void OnDestroy()
    {
        toggle.onValueChanged.RemoveAllListeners();
    }
}
