using UnityEngine;
using ActionCode.UI;
using AudioSettings = ActionCode.Audio.AudioSettings;
using KitchenChaos.Save;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class SoundPopup : Popup
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private DelayedButton resetButton;
        [SerializeField] private AudioSettings audioSettings;
        [SerializeField] private SaveSettings saveSettings;

        protected override void OnEnable()
        {
            base.OnEnable();
            resetButton.onClick.AddListener(HandleResetButtonClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            resetButton.onClick.RemoveListener(HandleResetButtonClick);
        }

        protected override void HandleCloseButtonClick()
        {
            canvasGroup.interactable = false;
            saveSettings.OnSaveFinished += HandleSaveFinished;

            saveSettings.Save();
        }

        private void HandleResetButtonClick() => audioSettings.ResetVolumes();

        private void HandleSaveFinished()
        {
            saveSettings.OnSaveFinished -= HandleSaveFinished;

            canvasGroup.interactable = true;
            base.HandleCloseButtonClick();
        }
    }
}