using UnityEngine;
using ActionCode.Persistence;
using AudioSettings = ActionCode.Audio.AudioSettings;
using System;

namespace KitchenChaos.Save
{
    [CreateAssetMenu(fileName = "SaveSettings", menuName = EditorPaths.SO + "Save Settings", order = 110)]
    public sealed class SaveSettings : ScriptableObject
    {
        [field: SerializeField] public PersistenceSettings Settings { get; private set; }

        [SerializeField] private AudioSettings audioSettings;

        public event Action OnSaveFinished;
        public event Action OnLoadFinished;

        public SaveData Data { get; set; }

        private const string saveFileName = "GameData";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Instantiate()
        {
            var settings = Resources.Load<SaveSettings>(nameof(SaveSettings));
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
            Data = await Settings.Load<SaveData>(saveFileName);

            var isEmpty = Data == null;
            if (isEmpty) Data = new SaveData();

            audioSettings.Load(Data.Audio);
            OnLoadFinished?.Invoke();
        }
    }
}