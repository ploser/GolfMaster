using GolfMaster.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GolfMaster.UI
{
    public class MenuPanel : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button exitButton;

        private void OnEnable()
        {
            resumeButton.gameObject.SetActive(GameManager.Instance != null && GameManager.Instance.CurrentPlayer != null);
        }

        private void Start()
        {
            resumeButton.onClick.AddListener(delegate { OnResumeButtonClicked(); });
            newGameButton.onClick.AddListener(delegate { OnNewGameButtonClicked(); });
            exitButton.onClick.AddListener(delegate { OnExitButtonClicked(); });
        }

        private void OnResumeButtonClicked()
        {
            GUIManager.Instance.ToggleMenu(false);
        }

        private void OnNewGameButtonClicked()
        {
            GameManager.Instance.StartNewGame();
        }

        private void OnExitButtonClicked()
        {
            Application.Quit();
        }
    }
}
