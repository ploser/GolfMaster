using GolfMaster.PlayerObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GolfMaster.PlayerObjects
{
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Transform innerBar;
        [SerializeField] private PlayerController player;

        private void Update()
        {
            this.transform.LookAt(Camera.main.transform.position);
            SetBarValue();
        }

        private void SetBarValue()
        {
            if (!player || !innerBar)
                return;

            var nValue = Mathf.Clamp01(player.Health / 100f);
            var scale = innerBar.localScale;
            scale.x = Mathf.Lerp(scale.x, nValue, Time.deltaTime * 10);
            innerBar.localScale = scale;
        }
    }
}
