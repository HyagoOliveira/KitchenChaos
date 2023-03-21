using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using KitchenChaos.Matches;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    public sealed class Timer : MonoBehaviour
    {
        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private Slider slider;
        [SerializeField] private string timerFormat = @"mm\:ss";

        private void Reset()
        {
            canvas = GetComponent<Canvas>();
            timer = GetComponentInChildren<TMP_Text>();
            slider = GetComponentInChildren<Slider>();
        }

        private void OnEnable()
        {
            matchSettings.CountDown.OnStarted += Hide;
            matchSettings.TimeLimit.OnStarted += Show;
            matchSettings.TimeLimit.OnUpdated += HandleTimeLimitUpdated;
        }

        private void OnDisable()
        {
            matchSettings.CountDown.OnStarted -= Hide;
            matchSettings.TimeLimit.OnStarted -= Show;
            matchSettings.TimeLimit.OnUpdated -= HandleTimeLimitUpdated;
        }

        private void Hide() => canvas.enabled = false;
        private void Show() => canvas.enabled = true;

        private void HandleTimeLimitUpdated(uint seconds)
        {
            var time = TimeSpan.FromSeconds(seconds);
            timer.text = time.ToString(timerFormat);

            var normilizedTime = (float)seconds / matchSettings.TimeLimit.totalTime;
            slider.value = normilizedTime;
        }
    }
}