using GolfMaster.Common;
using GolfMaster.Events;
using GolfMaster.InGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace GolfMaster.Managers
{
    public class MapManager : SingleMonoBehaviour<MapManager>
    {
        [SerializeField] private GolfBall golfBallTemplate;
        [SerializeField] private Terrain tarrain;
        [SerializeField] private LayerMask groundLayers;
        [SerializeField] private GolfCart golfCart;

        public Vector3 GolfCartPosition => !golfCart ? Vector3.zero : golfCart.transform.position;

        private List<GolfBall> _currentBalls = new List<GolfBall>();
        private Transform _ballContainer;

        private void OnEnable()
        {
            GameEventManager.On<GameStarted>(OnGameStarted);
        }

        private void OnDisable()
        {
            GameEventManager.Off<GameStarted>(OnGameStarted);
        }

        private void GenerateBalls()
        {
            if (tarrain == null || golfBallTemplate == null)
                return;

            if (_ballContainer == null)
            {
                _ballContainer = new GameObject("BallContainer").transform;
                _ballContainer.parent = this.transform;
            }

            var i = 0;
            var tryCount = 0;
            var priorityEnumList = Enum.GetValues(typeof(GolfBallPriority));

            while (i < GameSettings.Instance.MainSettings.GolfBallCount && tryCount < GameSettings.Instance.MainSettings.GolfBallCount * 3)
            {
                var randomPoint = new Vector3(Random.Range(1f, tarrain.terrainData.size.x - 1f), 20, Random.Range(1f, tarrain.terrainData.size.z - 1f));
                var pos = randomPoint;

                if (CheckBallPosition(randomPoint, ref pos))
                {
                    var golfBall = Instantiate(golfBallTemplate, pos, Quaternion.identity, _ballContainer);
                    golfBall.Priority = (GolfBallPriority)priorityEnumList.GetValue(Random.Range(0, priorityEnumList.Length));
                    _currentBalls.Add(golfBall);
                    i++;
                }

                tryCount++;
            }
        }

        private bool CheckBallPosition(Vector3 checkPos, ref Vector3 pos)
        {
            if (Physics.Raycast(checkPos, Vector3.down, out RaycastHit hit, 100, groundLayers) &&
                NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 0.5f, NavMesh.AllAreas))
            {
                pos = hit.point;
                return true;
            }

            return false;
        }

        private void CleanBalls()
        {
            foreach (var ball in _currentBalls)
            {
                Destroy(ball.gameObject);
            }

            _currentBalls = new List<GolfBall>();
        }

        public bool TryGetOptimalBall(Vector3 refPoint, float maxDistance, out GolfBall targetBall)
        {
            targetBall = null;
            var pointPerDistance = 0f;

            foreach (var ball in _currentBalls)
            {
                var dist = Vector3.Distance(ball.transform.position, refPoint);
                if (dist > maxDistance)
                    continue;

                var point = ball.Priority == GolfBallPriority.Low ? GameSettings.Instance.MainSettings.LowPoint :
                            ball.Priority == GolfBallPriority.Medium ? GameSettings.Instance.MainSettings.MediumPoint :
                                                                        GameSettings.Instance.MainSettings.HighPoint;

                var pd = point / dist;
                if (pd > pointPerDistance)
                {
                    pointPerDistance = pd;
                    targetBall = ball;
                }
            }

            if (targetBall != null)
                _currentBalls.Remove(targetBall);

            return targetBall != null;

        }

        public void ReturnBall(GolfBall ball)
        {
            _currentBalls.Add(ball);
        }

        private void OnGameStarted(GameStarted e)
        {
            CleanBalls();
            GenerateBalls();
        }
    }
}
