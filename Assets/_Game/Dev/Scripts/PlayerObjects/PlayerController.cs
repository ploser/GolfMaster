﻿using GolfMaster.Common;
using GolfMaster.Events;
using GolfMaster.InGame;
using GolfMaster.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace GolfMaster.PlayerObjects
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerState State { get; private set; }
        public float Health { get; set; } = 100;

        public Transform TargetObject;

        [SerializeField] private Animator anim;
        [SerializeField] NavMeshAgent agent;

        private float _currentSpeed;
        private Vector3 _prePosition;
        private Vector3 _targetPosition;
        private Vector3 _startingPoint;
        private GolfBall _targetBall;
        private int _speedHash = Animator.StringToHash("speed");
        private int _gatherHash = Animator.StringToHash("gather");
        private int _collectedPoint;

        private void Awake()
        {
            _startingPoint = this.transform.position;
        }

        private void OnEnable()
        {
            GameEventManager.On<GameStarted>(OnGameStarted);
        }

        private void OnDisable()
        {
            GameEventManager.Off<GameStarted>(OnGameStarted);
        }

        private void Start()
        {
            _prePosition = this.transform.position;
        }

        private void Update()
        {
            CheckStates();
            CalculateSpeed();
            CalculateHealth();
        }

        private void LateUpdate()
        {
            anim.SetFloat(_speedHash, Mathf.Clamp01(_currentSpeed / 2f));
        }

        private void CheckStates()
        {
            if (State == PlayerState.Gathering)
            {
                if (Health <= 0)
                    ReturnToCart();
                else if (HasTargetReached())
                    GatherTargetBall(SetTarget);
            }
            else if (State == PlayerState.Returning)
            {
                if (HasCartReached())
                {
                    GameEventManager.Fire(new PlayerReturnedToCart() { CollectedPoint = _collectedPoint });
                    ResetValues();
                    ChangeState(PlayerState.Gathering);
                    SetTarget();
                }
                else
                    agent.SetDestination(MapManager.Instance.GolfCartPosition);
            }
        }

        private void ChangeState(PlayerState newState)
        {
            var oldState = State;
            State = newState;

            GameEventManager.Fire(new PlayerStateChanged()
            {
                Player = this,
                OldState = oldState,
                NewState = newState
            });
        }

        private void CalculateHealth()
        {
            Health = Mathf.Max(Health - Time.deltaTime * GameSettings.Instance.PlayerSettings.PlayerHealthSpeed, 0);
        }

        private void CalculateSpeed()
        {
            var dif = Vector3.Distance(this.transform.position, _prePosition);
            _currentSpeed = dif / Time.deltaTime;
            _prePosition = this.transform.position;
        }

        private void GatherTargetBall(Action finishCallback = null)
        {
            if (_targetBall == null)
                return;

            var gatheringBall = _targetBall;
            var point = gatheringBall.Priority == GolfBallPriority.Low ? GameSettings.Instance.MainSettings.LowPoint :
                            gatheringBall.Priority == GolfBallPriority.Medium ? GameSettings.Instance.MainSettings.MediumPoint :
                                                                        GameSettings.Instance.MainSettings.HighPoint;
            _collectedPoint += point;
            gatheringBall.Collect();

            _targetBall = null;

            StopPlayer();
            anim.Play("Gathering", 1);

            this.DelayedCall(1, () =>
            {
                finishCallback?.Invoke();
            });
        }

        private void ResetValues()
        {
            Health = 100;
            _collectedPoint = 0;
        }

        private void ReturnToCart()
        {
            ChangeState(PlayerState.Returning);
            agent.SetDestination(MapManager.Instance.GolfCartPosition);

            if (_targetBall)
            {
                MapManager.Instance.ReturnBall(_targetBall);
                _targetBall = null;
            }
        }

        private void SetTarget()
        {
            var predictedRange = agent.speed * 0.5f * Health / GameSettings.Instance.PlayerSettings.PlayerHealthSpeed;

            if (MapManager.Instance.TryGetOptimalBall(this.transform.position, predictedRange, out var ball))
            {
                agent.SetDestination(ball.transform.position);
                _targetBall = ball;
            }
            else
            {
                ReturnToCart();
                _targetBall = null;
            }
        }

        private bool HasTargetReached()
        {
            if (_targetBall == null)
                return true;

            return Vector3.Distance(this.transform.position, _targetBall.transform.position) <=
                GameSettings.Instance.MainSettings.ReachDistanceForGolfBall;
        }

        private bool HasCartReached()
        {
            return Vector3.Distance(this.transform.position, MapManager.Instance.GolfCartPosition) <= 3f;
        }

        private void StopPlayer()
        {
            agent.SetDestination(this.transform.position);
        }

        private void GoStartingPoint()
        {
            this.transform.position = _startingPoint;
        }

        private void OnGameStarted(GameStarted e)
        {
            ResetValues();
            GoStartingPoint();
            ChangeState(PlayerState.Ready);

            this.DelayedCall(0.5f, () =>
            {
                ChangeState(PlayerState.Gathering);
                SetTarget();
            });
        }
    }
}
