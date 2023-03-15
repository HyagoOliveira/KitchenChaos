using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenChaos.Players
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = EditorPaths.SO + "Player Settings", order = 110)]
    public sealed class PlayerSettings : ScriptableObject
    {
        [SerializeField] private PlayerType first;

        public event Action OnPlayerEnabled;

        public Player Current { get; private set; }

        private Dictionary<PlayerType, Player> players;

        /// <summary>
        /// Switches into the next Player.
        /// <para>It checks if switch is possible.</para>
        /// </summary>
        public void Switch() => Switch(GetNextPlayerType());

        /// <summary>
        /// Switches into the given player if available.
        /// </summary>
        /// <param name="type">The Player to switch.</param>
        public void Switch(PlayerType type) => EnablePlayer(players[type]);

        internal void Initialize() => FindPlayersInstances();

        internal void EnableFirstPlayer() => EnablePlayer(players[first]);

        private void FindPlayersInstances()
        {
            var index = 0;
            var instances = FindObjectsOfType<Player>();

            players = new Dictionary<PlayerType, Player>(instances.Length);

            foreach (var instance in instances)
            {
                instance.Index = index++;
                players.Add(instance.Type, instance);
            }
        }

        private void EnablePlayer(Player player)
        {
            DisableAllPlayers();

            Current = player;
            Current.SetActive(true);

            OnPlayerEnabled?.Invoke();
        }

        private void DisableAllPlayers()
        {
            foreach (var player in players.Values)
            {
                player.SetActive(false);
            }
        }

        public PlayerType GetNextPlayerType()
        {
            var index = Current.Index;
            if (++index >= players.Count) index = 0;
            return players.Keys.ElementAt(index);
        }
    }
}