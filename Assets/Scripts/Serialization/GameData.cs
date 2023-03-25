using System;
using ActionCode.Audio;

namespace KitchenChaos.Serialization
{
    [Serializable]
    public sealed class GameData
    {
        public string Language { get; internal set; }
        public uint HighScore { get; internal set; }
        public AudioData Audio { get; internal set; }
        public DateTime CreationTime { get; internal set; }
        public DateTime LastUpdateTime { get; internal set; }

        public GameData() : this(string.Empty, 0, new AudioData(), DateTime.Now, default) { }

        public GameData(
            string language,
            uint highScore,
            AudioData audio,
            DateTime creationTime,
            DateTime lastUpdateTime
        )
        {
            Language = language;
            HighScore = highScore;
            Audio = audio;
            CreationTime = creationTime;
            LastUpdateTime = lastUpdateTime;
        }
    }
}