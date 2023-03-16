using System;
using ActionCode.Audio;

namespace KitchenChaos.Serialization
{
    [Serializable]
    public sealed class GameData
    {
        public string Language { get; internal set; }
        public AudioData Audio { get; internal set; }
        public DateTime CreationTime { get; internal set; }
        public DateTime LastUpdateTime { get; internal set; }

        public GameData() : this(string.Empty, new AudioData(), DateTime.Now, default) { }

        public GameData(
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