using GolfMaster.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GolfMaster.InGame
{
    public class GolfBall : MonoBehaviour
    {
        public GolfBallPriority Priority { get; set; }

        public void Collect()
        {
            GameEventManager.Fire(new GolfBallCollected()
            {
                Priority = this.Priority,
                Position = this.transform.position    
            });

            this.DelayedCall(0.5f, DestroySelf);
        }

        private void DestroySelf()
        {
            Destroy(this.gameObject);
        }
    }
}
