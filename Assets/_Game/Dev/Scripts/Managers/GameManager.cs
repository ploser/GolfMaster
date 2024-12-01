using GolfMaster.Common;
using GolfMaster.Events;
using GolfMaster.PlayerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GolfMaster.Managers
{
    public class GameManager : SingleMonoBehaviour<GameManager>
    {
        public int GameScore { get; set; }
        public GameState State { get; private set; }
        public PlayerController CurrentPlayer { get; private set; }

        private void OnEnable()
        {
            GameEventManager.On<PlayerStateChanged>(OnPlayerStateChanged);
            GameEventManager.On<GolfBallCollected>(OnGolfBallCollected);
        }

        private void OnDisable()
        {
            GameEventManager.Off<PlayerStateChanged>(OnPlayerStateChanged);
            GameEventManager.Off<GolfBallCollected>(OnGolfBallCollected);
        }

        private void Start()
        {
            ChangeState(GameState.Game);
        }

        public void ChangeState(GameState newState)
        {
            var oldState = State;
            State = newState;

            GameEventManager.Fire(new GameStateChanged()
            {
                OldState = oldState,
                NewState = newState
            });
        }

        private void OnPlayerStateChanged(PlayerStateChanged e)
        {
            if (e.NewState == PlayerState.Ready)
                CurrentPlayer = e.Player;
        }

        private void OnGolfBallCollected(GolfBallCollected e)
        {
            var point = e.Priority == GolfBallPriority.Low ? GameSettings.Instance.MainSettings.LowPoint :
                        e.Priority == GolfBallPriority.Medium ? GameSettings.Instance.MainSettings.MediumPoint :
                                                                   GameSettings.Instance.MainSettings.HighPoint;
            GameScore += point;
        }
    }
}
