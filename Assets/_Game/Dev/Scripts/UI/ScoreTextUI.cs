using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GolfMaster.UI
{
    public class ScoreTextUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text pointText;
        [SerializeField] private RectTransform rect;

        private Vector3 _worldPosition;

        private void Update()
        {
            rect.position = Camera.main.WorldToScreenPoint(_worldPosition);
        }

        public void Init(int point, Vector3 worldPosition)
        {
            pointText.text = "+" + point;
            _worldPosition = worldPosition;

            this.DelayedCall(2, DestroySelf);
        }

        private void DestroySelf()
        {
            Destroy(this.gameObject);
        }
    }
}
