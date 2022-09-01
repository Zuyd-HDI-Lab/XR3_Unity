using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Countdown timer with pause and reset option
/// </summary>
public class CountdownTimer : MonoBehaviour
{
    /// <summary>
    /// Fires when countdown reaches zero
    /// </summary>
    public UnityEvent onTimeElapsed;

    /// <summary>
    /// Seconds to count down
    /// </summary>
    public float countdownTimeInSeconds;

    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Image radialImage;

    private bool countdown;
    private float timeLeft;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        onTimeElapsed = new UnityEvent();
    }

    /// <summary>
    /// Start countdown
    /// </summary>
    public void StartCountdown()
    {
        timeLeft = countdownTimeInSeconds;
        countdown = true;
    }

    /// <summary>
    /// Pause countdown
    /// </summary>
    public void PauseCountdown()
    {
        countdown = false;
    }

    /// <summary>
    /// Reset the timer to initial time
    /// </summary>
    /// <param name="start">start after reset</param>
    public void ResetCountdown(bool start = false)
    {
        countdown = start;
        timeLeft = countdownTimeInSeconds;
    }

    public void ShowTimer()
    {
        canvas.enabled = true;
    }

    public void HideTimer()
    {
        canvas.enabled = false;
    }

    private void Update()
    {
        if (countdown)
            timeLeft -= Time.deltaTime;

        timerText.text = $"Starts in {timeLeft} seconds";
        radialImage.fillAmount = timeLeft / countdownTimeInSeconds;

        if (!countdown || !(timeLeft <= 0f)) return;

        countdown = false;
        timeLeft = 0f;
        onTimeElapsed?.Invoke();
    }
}