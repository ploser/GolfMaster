using GolfMaster.PlayerObjects;
using UnityEngine;

namespace GolfMaster.Events
{
    public class GameStarted : GameEvent 
    { 
       public PlayerController Player { get; set; }
    }

    public class PlayerStateChanged : GameEvent
    {
        public PlayerController Player { get; set; }
        public PlayerState OldState { get; set; }
        public PlayerState NewState { get; set; }
    }

    public class GolfBallCollected : GameEvent
    {
        public GolfBallPriority Priority { get; set; }
        public Vector3 Position { get; set; }
    }

    public class PlayerReturnedToCart : GameEvent
    {
        public int CollectedPoint { get; set; }
    }
}
