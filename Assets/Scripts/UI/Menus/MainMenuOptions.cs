using System;
using UnityEngine;
using ActionCode.UI;
using ActionCode.SceneManagement;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class MainMenuOptions : AbstractMenu
    {
        [SerializeField] private SceneManager sceneManager;
        [SerializeField, Scene] private string gameScene;

        [Header("Buttons")]
        [SerializeField] private DelayedButton playButton;
        [SerializeField] private DelayedButton soundsButton;
        [SerializeField] private DelayedButton creditsButton;

        public event Action OnOpenCreditsRequested;
        public event Action OnOpenSoundsOptionsRequested;

        protected override void BindButtonsEvents()
        {
            playButton.onClick.AddListener(PlayGame);
            soundsButton.onClick.AddListener(RequestOpenSoundOptions);
            creditsButton.onClick.AddListener(RequestOpenCredits);
        }

        protected override void UnBindButtonsEvents()
        {
            playButton.onClick.RemoveListener(PlayGame);
            soundsButton.onClick.RemoveListener(RequestOpenSoundOptions);
            creditsButton.onClick.RemoveListener(RequestOpenCredits);
        }

        private void PlayGame() => _ = sceneManager.LoadScene(gameScene);
        private void RequestOpenCredits() => OnOpenCreditsRequested?.Invoke();
        private void RequestOpenSoundOptions() => OnOpenSoundsOptionsRequested?.Invoke();
    }
}