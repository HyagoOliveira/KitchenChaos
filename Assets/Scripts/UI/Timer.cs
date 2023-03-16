using TMPro;
using UnityEngine;
using KitchenChaos.Levels;
using System;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class Timer : MonoBehaviour
    {
        [SerializeField] private MatchSettings levelSettings;
        [SerializeField] private GameObject panel;
        [SerializeField] private TMP_Text timer;
        [SerializeField] private string timerFormat = @"mm\:ss";

        private void Reset()
        {
            timer = GetComponentInChildren<TMP_Text>();
        }

        private void OnEnable()
        {
            levelSettings.CountDown.OnStarted += HidePanel;
            levelSettings.TimeLimit.OnStarted += ShowPanel;
            levelSettings.TimeLimit.OnUpdated += HandleTimeLimitUpdated;
        }

        private void OnDisable()
        {
            levelSettings.CountDown.OnStarted -= HidePanel;
            levelSettings.TimeLimit.OnStarted -= ShowPanel;
            levelSettings.TimeLimit.OnUpdated -= HandleTimeLimitUpdated;
        }

        private void HidePanel() => panel.SetActive(false);
        private void ShowPanel() => panel.SetActive(true);

        private void HandleTimeLimitUpdated(uint seconds)
        {
            var time = TimeSpan.FromSeconds(seconds);
            timer.text = time.ToString(timerFormat);
        }
    }
}