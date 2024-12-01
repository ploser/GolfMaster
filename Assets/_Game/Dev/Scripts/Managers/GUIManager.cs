using GolfMaster.Common;
using GolfMaster.Events;
using GolfMaster.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GolfMaster.Managers
{
    public class GUIManager : SingleMonoBehaviour<GUIManager>
    {
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private ScoreTextUI scoreTextTemplate;
        [SerializeField] private GameObject inGamePanel;
        [SerializeField] private MenuPanel menuPanel;

        private bool _showMenu = true;

        private void OnEnable()
        {
            GameEventManager.On<GolfBallCollected>(OnGolfBallCollected);
            GameEventManager.On<GameStarted>(OnGameStarted);
        }

        private void OnDisable()
        {
            GameEventManager.Off<GolfBallCollected>(OnGolfBallCollected);
            GameEventManager.On<GameStarted>(OnGameStarted);
        }

        private void Start()
        {
            SetPanels();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ToggleMenu(!_showMenu);
        }

        public void ToggleMenu(bool show)
        {
            _showMenu = show;
            SetPanels();
        }

        private void SetPanels()
        {
            inGamePanel.SetActive(!_showMenu);
            menuPanel.gameObject.SetActive(_showMenu);
        }

        private void OnGolfBallCollected(GolfBallCollected e)
        {
            var point = e.Priority == GolfBallPriority.Low ? GameSettings.Instance.MainSettings.LowPoint :
                        e.Priority == GolfBallPriority.Medium ? GameSettings.Instance.MainSettings.MediumPoint :
                                                                   GameSettings.Instance.MainSettings.HighPoint;

            var scoreText = Instantiate(scoreTextTemplate, mainCanvas.transform);
            scoreText.Init(point, e.Position);
        }

        private void OnGameStarted(GameStarted e)
        {
            _showMenu = false;
            SetPanels();
        }
    }
}

