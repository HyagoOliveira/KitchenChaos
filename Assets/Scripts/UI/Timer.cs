using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using KitchenChaos.Matches;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(Animation))]
    [RequireComponent(typeof(AudioSource))]
    public sealed class Timer : MonoBehaviour
    {
        [SerializeField] private MatchSettings matchSettings;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Animation animation;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private Slider slider;
        [SerializeField] private string timerFormat = @"mm\:ss";
        [SerializeField] private Color finalSecondsColor = Color.red;

        private bool isFinalSeconds;

        private void Reset()
        {
            canvas = GetComponent<Canvas>();
            animation = GetComponent<Animation>();
            audioSource = GetComponent<AudioSource>();
            timer = GetComponentInChildren<TMP_Text>();
            slider = GetComponentInChildren<Slider>();
        }

        private void OnEnable()
        {
            matchSettings.CountDown.OnStarted += Hide;
            matchSettings.TimeLimit.OnStarted += Show;
            matchSettings.TimeLimit.OnUpdated += HandleTimeLimitUpdated;
            matchSettings.OnFinalSecondsStarted += HandleFinalSecondsStarted;
        }

        private void OnDisable()
        {
            matchSettings.CountDown.OnStarted -= Hide;
            matchSettings.TimeLimit.OnStarted -= Show;
            matchSettings.TimeLimit.OnUpdated -= HandleTimeLimitUpdated;
            matchSettings.OnFinalSecondsStarted -= HandleFinalSecondsStarted;
        }

        private void Hide() => canvas.enabled = false;
        private void Show() => canvas.enabled = true;

        private void HandleTimeLimitUpdated(uint seconds)
        {
            var time = TimeSpan.FromSeconds(seconds);
            timer.text = time.ToString(timerFormat);

            var normilizedTime = (float)seconds / matchSettings.TimeLimit.totalTime;
            slider.value = normilizedTime;

            if (isFinalSeconds) PlayFinalSecondsEffects();
        }

        private void HandleFinalSecondsStarted()
        {
            isFinalSeconds = true;
            timer.color = finalSecondsColor;
        }

        private void PlayFinalSecondsEffects()
        {
            animation.Play();
            audioSource.Play();
        }
    }
}