using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Week4
{
    public class HealthbarBossUI : MonoBehaviour
    {
        private Slider healthSlider;
        [SerializeField]
        private Healthbar healthbar;
        [SerializeField]
        private float lerp = 0.2f;
        private void Awake()
        {
            healthSlider = GetComponent<Slider>();
        }

        private void Update() {
            healthSlider.value = Mathf.Lerp(healthSlider.value, healthbar.TargetValue, lerp);
        }
    }
}
