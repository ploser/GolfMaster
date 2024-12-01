using GolfMaster.Events;
using GolfMaster.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GolfMaster.UI
{
    public class ScoreBarUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image scoreImage;

        private void Start()
        {
            GameEventManager.On<PlayerReturnedToCart>(OnPlayerReturnedToCart);
            GameEventManager.On<GameStarted>(OnGameStarted);
        }


        private void Update()
        {
            SetSize();
        }

        private void SetScoreText()
        {
            scoreText.text = Mathf.RoundToInt(GameManager.Instance.GameScore).ToString();
        }

        private void SetSize()
        {
            scoreImage.transform.localScale = Vector3.Lerp(scoreImage.transform.localScale, Vector3.one, Time.deltaTime);
        }

        private void OnPlayerReturnedToCart(PlayerReturnedToCart e)
        {
            scoreImage.transform.localScale = Vector3.one * 1.2f;
            SetScoreText();
        }

        private void OnGameStarted(GameStarted e)
        {
            SetScoreText();
        }
    }
}
