using GolfMaster.Managers;
using GolfMaster.PlayerObjects;
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
    public class PlayerHealthUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Image innerHPBar;

        private void Update()
        {
            SetHeathText();
        }

        private void SetHeathText()
        {
            if (GameManager.Instance.CurrentPlayer == null)
                return;

            healthText.text = Mathf.RoundToInt(GameManager.Instance.CurrentPlayer.Health).ToString();
            innerHPBar.fillAmount = GameManager.Instance.CurrentPlayer.Health / 100f;
        }
    }
}
