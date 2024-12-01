using GolfMaster.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GolfMaster.InGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 lookDirection;
        [SerializeField] private float lookAngle;

        private Transform _targetTransform;

        private void OnEnable()
        {
            GameEventManager.On<PlayerStateChanged>(OnPlayerStateChanged);
        }

        private void OnDisable()
        {
            GameEventManager.Off<PlayerStateChanged>(OnPlayerStateChanged);
        }

        private void Start()
        {
            StartFollowing();
        }

        private void StartFollowing()
        {
            this.RepeatEachFrame((deltaTime) =>
            {
                if (_targetTransform == null)
                    return true;

                var targetPos = _targetTransform.position + lookDirection;
                this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * 5);
                this.transform.rotation = Quaternion.Euler(lookAngle, 0, 0);

                return true;
            });
        }

        private void OnPlayerStateChanged(PlayerStateChanged e)
        {
            if (e.Player.State == PlayerState.Ready)
                _targetTransform = e.Player.transform;
        }
    }
}
