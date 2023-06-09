using System;
using UnityEngine;
using KitchenChaos.Players;
using System.Collections;

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

        protected string GetCollectButtonSprite() =>
            SurrounButtonNameWithSpriteTag(PlayerInput.GetCollectItemButtonSpriteName());

        protected string GetInteractWithEnvironmentButtonSprite() =>
                SurrounButtonNameWithSpriteTag(PlayerInput.GetInteractWithEnvironmentButtonSpriteName());

        protected string SurrounButtonNameWithSpriteTag(string button) => $"<sprite name=\"{button}\">";

        private IEnumerator CompleteDescriptionAndInvokeRoutine(Action action)
        {
            yield return manager.CompleteDescriptionRoutine();
            action?.Invoke();
        }
    }
}