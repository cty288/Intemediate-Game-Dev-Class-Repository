using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Week4
{
    public class Healthbar : MonoBehaviour {
        private Slider healthSlider;
        private IDamageable parent;

        [SerializeField] 
        private float lerp = 0.2f;
        
        private void Awake() {
            healthSlider = GetComponentInChildren<Slider>();
            parent = GetComponentInParent<Enemy>();
        }

        private void Update() {
            float targetValue = (float)parent.Health / parent.MaxHealth;
            healthSlider.value = Mathf.Lerp(healthSlider.value, targetValue, lerp);
        }
    }
}
