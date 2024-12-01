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
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image scoreImage;

        private void OnEnable()
        {
            GameEventManager.On<GolfBallCollected>(OnGolfBallCollected);
        }

        private void OnDisable()
        {
            GameEventManager.Off<GolfBallCollected>(OnGolfBallCollected);
        }

        private void Update()
        {
            SetScoreText();
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

        private void OnGolfBallCollected(GolfBallCollected e)
        {
            scoreImage.transform.localScale = Vector3.one * 1.2f;
        }
    }
}
