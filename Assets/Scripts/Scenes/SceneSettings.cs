using UnityEngine;
using ActionCode.SceneManagement;

namespace KitchenChaos.Scenes
{
    [CreateAssetMenu(fileName = "SceneSettings", menuName = EditorPaths.SO + "Scene Settings", order = 110)]
    public sealed class SceneSettings : ScriptableObject
    {
        [SerializeField] private SceneManager manager;

        [Header("Scenes")]
        [SerializeField, Scene] private string mainMenu = "MainMenu";
        [SerializeField, Scene] private string game = "Game";
        [SerializeField, Scene] private string results = "Results";

        public void GoToMainMenu() => _ = manager.LoadScene(mainMenu);
        public void GoToGame() => _ = manager.LoadScene(game);
        public void GoToResults() => _ = manager.LoadScene(results);
    }
}