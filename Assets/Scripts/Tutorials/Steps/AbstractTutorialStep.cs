using UnityEngine;
using KitchenChaos.Players;
using System.Collections;
using System;

namespace KitchenChaos.Tutorials
{
    public abstract class AbstractTutorialStep : MonoBehaviour
    {
        protected Player CurrentPlayer => manager.PlayerSettings.Current;
        protected PlayerInputSettings PlayerInput => manager.PlayerInputSettings;

        protected TutorialManager manager;

        internal void Initialize(TutorialManager manager) => this.manager = manager;

        internal abstract void Begin();
        internal abstract string GetDescription();

        protected void Complete() => manager.CompleteStep();

        protected void CompleteDescriptionAndInvoke(Action action) =>
            StartCoroutine(CompleteDescriptionAndInvokeRoutine(action));

        protected string GetCollectButtonDisplayName() =>
            SurrounButtonNameWithTag(PlayerInput.GetCollectItemButtonDisplayName());

        protected string GetInteractWithEnvironmentButtonDisplayName() =>
                SurrounButtonNameWithTag(PlayerInput.GetInteractWithEnvironmentButtonDisplayName());

        protected string SurrounButtonNameWithTag(string button) => $"<color=white>{button}</color>";

        private IEnumerator CompleteDescriptionAndInvokeRoutine(Action action)
        {
            yield return manager.CompleteDescriptionRoutine();
            action?.Invoke();
        }
    }
}