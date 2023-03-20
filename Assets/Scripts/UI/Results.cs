using TMPro;
using System;
using UnityEngine;
using ActionCode.UI;
using KitchenChaos.Score;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class Results : MonoBehaviour
    {
        [SerializeField] private ScoreSettings scoreSettings;
        [SerializeField] private DelayedButton continueButton;

        [SerializeField] private TMP_Text failedDeliveries;
        [SerializeField] private TMP_Text totalFailedDeliveries;

        [SerializeField] private TMP_Text successfulDeliveries;
        [SerializeField] private TMP_Text totalSuccessfulDeliveries;

        [SerializeField] private TMP_Text tips;
        [SerializeField] private TMP_Text totalScore;

        private void Start()
        {
            const string failedOrdersFormat = "Failed Orders x {0:D2}";
            const string deliveredOrdersFormat = "Delivered Orders x {0:D2}";

            failedDeliveries.text = string.Format(failedOrdersFormat, scoreSettings.FailedDeliveries);
            totalFailedDeliveries.text = scoreSettings.GetTotalFailedDeliveries().ToString();

            successfulDeliveries.text = string.Format(deliveredOrdersFormat, scoreSettings.SuccessfulDeliveries);
            totalSuccessfulDeliveries.text = scoreSettings.GetTotalSuccessfulDeliveries().ToString();

            tips.text = scoreSettings.Tips.ToString();
            totalScore.text = scoreSettings.Score.ToString();
        }

        private void OnEnable() => continueButton.onClick.AddListener(HandleContinueButtonClick);
        private void OnDisable() => continueButton.onClick.RemoveListener(HandleContinueButtonClick);

        private void HandleContinueButtonClick()
        {
            throw new NotImplementedException();
        }
    }
}