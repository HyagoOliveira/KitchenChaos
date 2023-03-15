using UnityEngine;
using ActionCode.UI;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuOptions mainMenuOptions;
        [SerializeField] private SoundPopup soundPopup;
        [SerializeField] private Popup creditsPopup;

        private void Reset()
        {
            creditsPopup = GetComponentInChildren<Popup>();
            soundPopup = GetComponentInChildren<SoundPopup>();
            mainMenuOptions = GetComponentInChildren<MainMenuOptions>();
        }

        private void OnEnable()
        {
            mainMenuOptions.OnOpenCreditsRequested += HandleOpenCreditsRequested;
            mainMenuOptions.OnOpenSoundsOptionsRequested += HandleOpenSoundsOptionsRequested;

            soundPopup.OnHidden += HandleSoundPopupHidden;
            creditsPopup.OnHidden += HandleCreditsPopupHidden;
        }

        private void OnDisable()
        {
            mainMenuOptions.OnOpenCreditsRequested -= HandleOpenCreditsRequested;
            mainMenuOptions.OnOpenSoundsOptionsRequested -= HandleOpenSoundsOptionsRequested;

            soundPopup.OnHidden -= HandleSoundPopupHidden;
            creditsPopup.OnHidden -= HandleCreditsPopupHidden;
        }

        private void HandleOpenCreditsRequested()
        {
            mainMenuOptions.Hide();
            creditsPopup.Show();
        }

        private void HandleOpenSoundsOptionsRequested()
        {
            mainMenuOptions.Hide();
            soundPopup.Show();
        }

        private void HandleSoundPopupHidden() => mainMenuOptions.Show();
        private void HandleCreditsPopupHidden() => mainMenuOptions.Show();
    }
}