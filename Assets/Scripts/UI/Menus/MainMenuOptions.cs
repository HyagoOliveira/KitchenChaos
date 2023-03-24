using System;
using UnityEngine;
using ActionCode.UI;
using KitchenChaos.Scenes;

namespace KitchenChaos.UI
{
    [DisallowMultipleComponent]
    public sealed class MainMenuOptions : AbstractMenu
    {
        [SerializeField] private SceneSettings sceneSettings;

        [Header("Buttons")]
        [SerializeField] private DelayedButton playButton;
        [SerializeField] private DelayedButton tutorialButton;
        [SerializeField] private DelayedButton soundsButton;
        [SerializeField] private DelayedButton creditsButton;

        public event Action OnOpenCreditsRequested;
        public event Action OnOpenSoundsOptionsRequested;

        protected override void BindButtonsEvents()
        {
            playButton.onClick.AddListener(PlayGame);
            tutorialButton.onClick.AddListener(PlayTutorial);
            soundsButton.onClick.AddListener(RequestOpenSoundOptions);
            creditsButton.onClick.AddListener(RequestOpenCredits);
        }

        protected override void UnBindButtonsEvents()
        {
            playButton.onClick.RemoveListener(PlayGame);
            tutorialButton.onClick.RemoveListener(PlayTutorial);
            soundsButton.onClick.RemoveListener(RequestOpenSoundOptions);
            creditsButton.onClick.RemoveListener(RequestOpenCredits);
        }

        private void PlayGame() => sceneSettings.GoToGame();
        private void PlayTutorial() => sceneSettings.GoToTutorial();
        private void RequestOpenCredits() => OnOpenCreditsRequested?.Invoke();
        private void RequestOpenSoundOptions() => OnOpenSoundsOptionsRequested?.Invoke();
    }
}