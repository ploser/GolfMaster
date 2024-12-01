using GolfMaster.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GolfMaster.Common
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "GolfMaster/Game Settings", order = 0)]
    public class GameSettings : SingleScriptableObject<GameSettings>
    {
        public MainSettings MainSettings;
        public PlayerSettings PlayerSettings;
    }

    [Serializable]
    public class MainSettings
    {
        public int GolfBallCount = 100;
        public float ReachDistanceForGolfBall = 2f;
        public int LowPoint = 5;
        public int MediumPoint = 10;
        public int HighPoint = 15;
    }

    [Serializable]
    public class PlayerSettings
    {
        public float PlayerSpeed = 1;
        public float PlayerHealthSpeed = 1;
    }
}
