using System;
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

        private const string saveFileName = nameof(GameData);

        public async void Save()
        {
            var audio = Data.Audio;
            audioSettings.Write(ref audio);

            Data.Audio = audio;
            Data.LastUpdateTime = DateTime.Now;

            var wasSaved = await settings.Save(Data, saveFileName);

            OnSaveFinished?.Invoke();
        }

        private async void LoadOrCreate()
        {
            Data = await settings.Load<GameData>(saveFileName);

            var isEmpty = Data == null;
            if (isEmpty) Data = new GameData();

            audioSettings.Load(Data.Audio);
            OnLoadFinished?.Invoke();
        }
    }
}