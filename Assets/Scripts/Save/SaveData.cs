using System;
using ActionCode.Audio;

namespace KitchenChaos.Save
{
    [Serializable]
    public sealed class SaveData
    {
        public string Language { get; internal set; }
        public AudioData Audio { get; internal set; }
        public DateTime CreationTime { get; internal set; }
        public DateTime LastUpdateTime { get; internal set; }

        public SaveData() : this(string.Empty, new AudioData(), DateTime.Now, default) { }

        public SaveData(
            string language,
            AudioData audio,
            DateTime creationTime,
            DateTime lastUpdateTime
        )
        {
            Language = language;
            Audio = audio;
            CreationTime = creationTime;
            LastUpdateTime = lastUpdateTime;
        }
    }
}