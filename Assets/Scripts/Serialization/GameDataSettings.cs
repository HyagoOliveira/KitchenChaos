using System;
using System.Collections;
using UnityEngine;
using ActionCode.Persistence;
using AudioSettings = ActionCode.Audio.AudioSettings;

namespace KitchenChaos.Serialization
{
    [CreateAssetMenu(fileName = "GameDataSettings", menuName = EditorPaths.SO + "Game Data Settings", order = 110)]
    public sealed class GameDataSettings : ScriptableObject
    {
        [SerializeField] private PersistenceSettings settings;
        [SerializeField] private AudioSettings audioSettings;

        public event Action OnSaveFinished;
        public event Action OnLoadFinished;

        public GameData Data { get; private set; }

        private GameDataManager manager;
        private const string saveFileName = nameof(GameData);

        internal void Initialize(GameDataManager manager)
        {
            this.manager = manager;
            LoadOrCreate();
        }

        public void Save() => manager.StartCoroutine(SaveRoutine());

        private void LoadOrCreate() => manager.StartCoroutine(LoadRoutine());

        private IEnumerator SaveRoutine()
        {
            var audio = Data.Audio;
            audioSettings.Write(ref audio);

            Data.Audio = audio;
            Data.LastUpdateTime = DateTime.Now;

            var savingTask = settings.Save(Data, saveFileName);

            yield return new WaitUntil(() => savingTask.IsCompleted);

            var wasSaved = savingTask.Result;
            Debug.Log("wasSaved: " + wasSaved);

            OnSaveFinished?.Invoke();
        }

        private IEnumerator LoadRoutine()
        {
            var loadingTask = settings.Load<GameData>(saveFileName);

            yield return new WaitUntil(() => loadingTask.IsCompleted);

            Data = loadingTask.Result;

            var isEmpty = Data == null;
            if (isEmpty) Data = new GameData();

            audioSettings.Load(Data.Audio);
            OnLoadFinished?.Invoke();
        }
    }
}