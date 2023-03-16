using System;
using UnityEngine;
using ActionCode.Persistence;
using AudioSettings = ActionCode.Audio.AudioSettings;

namespace KitchenChaos.Serialization
{
    [CreateAssetMenu(fileName = "GameDataSettings", menuName = EditorPaths.SO + "Game Data Settings", order = 110)]
    public sealed class GameDataSettings : ScriptableObject
    {
        [field: SerializeField] public PersistenceSettings Settings { get; private set; }

        [SerializeField] private AudioSettings audioSettings;

        public event Action OnSaveFinished;
        public event Action OnLoadFinished;

        public GameData Data { get; set; }

        private const string saveFileName = nameof(GameData);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Instantiate()
        {
            var settings = Resources.Load<GameDataSettings>(nameof(GameDataSettings));
            settings.LoadOrCreate();
        }

        public async void Save()
        {
            var audio = Data.Audio;
            audioSettings.Write(ref audio);

            Data.Audio = audio;
            Data.LastUpdateTime = DateTime.Now;

            var wasSaved = await Settings.Save(Data, saveFileName);

            OnSaveFinished?.Invoke();
        }

        private async void LoadOrCreate()
        {
            Data = await Settings.Load<GameData>(saveFileName);

            var isEmpty = Data == null;
            if (isEmpty) Data = new GameData();

            audioSettings.Load(Data.Audio);
            OnLoadFinished?.Invoke();
        }
    }
}