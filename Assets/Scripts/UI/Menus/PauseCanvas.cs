using UnityEngine;
using ActionCode.UI;
using ActionCode.PauseSystem;
using ActionCode.SceneManagement;
using KitchenChaos.Scenes;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class PauseCanvas : CanvasViewer
    {
        [SerializeField] private PauseSettings pauseSettings;
        [SerializeField] private SceneSettings sceneSettings;
        [SerializeField, Scene] private string mainMenu;

        [SerializeField] private PauseOptions pauseOptions;

        [Header("Popups")]
        [SerializeField] private SoundPopup soundPopup;
        [SerializeField] private ConfirmationPopup restartPopup;
        [SerializeField] private ConfirmationPopup exitPopup;

        private void Reset()
        {
            soundPopup = GetComponentInChildren<SoundPopup>();
            pauseOptions = GetComponentInChildren<PauseOptions>();
        }

        private void OnEnable()
        {
            exitPopup.OnHidden += HandleAnyPopupHidden;
            soundPopup.OnHidden += HandleAnyPopupHidden;
            restartPopup.OnHidden += HandleAnyPopupHidden;

            pauseOptions.OnResumeClicked += HandleResumeClicked;
            pauseOptions.OnRestartClicked += HandleRestartClicked;
            pauseOptions.OnSoundClicked += HandleSoundClicked;
            pauseOptions.OnExitClicked += HandleExitClicked;

            exitPopup.OnConfirmed += HandleExitConfirmed;
            restartPopup.OnConfirmed += HandleRestartConfirmed;

            pauseOptions.Hide();
            soundPopup.Hide();
        }

        private void OnDisable()
        {
            exitPopup.OnHidden -= HandleAnyPopupHidden;
            soundPopup.OnHidden -= HandleAnyPopupHidden;
            restartPopup.OnHidden -= HandleAnyPopupHidden;

            pauseOptions.OnResumeClicked -= HandleResumeClicked;
            pauseOptions.OnRestartClicked -= HandleRestartClicked;
            pauseOptions.OnSoundClicked -= HandleSoundClicked;
            pauseOptions.OnExitClicked -= HandleExitClicked;

            exitPopup.OnConfirmed -= HandleExitConfirmed;
            restartPopup.OnConfirmed -= HandleRestartConfirmed;
        }

        private void HandleAnyPopupHidden() => pauseOptions.Show();

        private void HandleResumeClicked() => pauseSettings.Resume();

        private void HandleRestartClicked()
        {
            pauseOptions.Hide();
            restartPopup.Show();
        }

        private void HandleSoundClicked()
        {
            pauseOptions.Hide();
            soundPopup.Show();
        }

        private void HandleExitClicked()
        {
            pauseOptions.Hide();
            exitPopup.Show();
        }

        private void HandleRestartConfirmed()
        {
            Time.timeScale = 1F;
            sceneSettings.GoToGame();
        }

        private void HandleExitConfirmed()
        {
            Time.timeScale = 1F;
            sceneSettings.GoToMainMenu();
        }
    }
}