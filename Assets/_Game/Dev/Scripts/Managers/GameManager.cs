using GolfMaster.Common;
using GolfMaster.Events;
using GolfMaster.PlayerObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace GolfMaster.Managers
{
    public class GameManager : SingleMonoBehaviour<GameManager>
    {
        [SerializeField] private PlayerController playerPrefab;

        public int GameScore { get; set; }
        public PlayerController CurrentPlayer { get; private set; }

        private void OnEnable()
        {
            GameEventManager.On<PlayerReturnedToCart>(OnPlayerReturnedToCart, 100);
        }

        private void OnDisable()
        {
            GameEventManager.Off<PlayerReturnedToCart>(OnPlayerReturnedToCart);
        }

        public void StartNewGame()
        {
            GameScore = 0;

            if (CurrentPlayer == null)
                CurrentPlayer = Instantiate(playerPrefab);

            GameEventManager.Fire(new GameStarted() { Player = CurrentPlayer });
        }

        private void OnPlayerReturnedToCart(PlayerReturnedToCart e)
        {
            GameScore += e.CollectedPoint;
        }
    }
}
