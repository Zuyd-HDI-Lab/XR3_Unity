using System;
using System.Collections;
using System.Collections.Generic;
using Constants;
using UnityEngine;
using UnityEngine.UI;
using ViveSR.anipal.Eye;

public class EyeTrackingFormElement : MonoBehaviour
{
    private Button calibrateButton;
    // Start is called before the first frame update
    private void Awake()
    {
        var eyeTracking = FindObjectOfType<SRanipal_Eye_Framework>(true);//GameObject.Find(GameObjectNames.EyeTracking);
        var eyeTrackingLoggerComponent = eyeTracking.GetComponent<EyeDataLogger>();
        calibrateButton = GetComponentInChildren<Button>();
        var disableWithToggle = GetComponentInChildren<DisableWithToggle>();

        calibrateButton.onClick.AddListener(() =>
        {
            eyeTrackingLoggerComponent.StartCalibration();
        });

        disableWithToggle.objectsToToggle.Add(eyeTracking.gameObject);
    }

    private void OnDestroy()
    {
        calibrateButton.onClick.RemoveAllListeners();
    }
}
