using UnityEngine;
using KitchenChaos.Scenes;
using System.Collections;

namespace KitchenChaos.Serialization
{
    [DisallowMultipleComponent]
    public sealed class Loader : MonoBehaviour
    {
        [SerializeField] private GameDataSettings settings;
        [SerializeField] private SceneSettings sceneSettings;
        [SerializeField, Min(0)] private float timeAfterLoad = 4f;

        private void OnEnable() => settings.OnLoadFinished += HandleLoadFinished;
        private void OnDisable() => settings.OnLoadFinished -= HandleLoadFinished;

        private void HandleLoadFinished() => StartCoroutine(GoToMainMenuAfterLoadTimeRoutine());

        private IEnumerator GoToMainMenuAfterLoadTimeRoutine()
        {
            yield return new WaitForSeconds(timeAfterLoad);
            sceneSettings.GoToMainMenu();
        }
    }
}