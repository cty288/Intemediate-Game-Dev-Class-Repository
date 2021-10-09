using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class InfoUI : MonoBehaviour
    {

        [SerializeField]
        private float lerp = 0.3f;
        
        private Vector3 activeScale;
        private Vector3 targetScale;
        private Transform child;
        private bool activated = false;
        public bool Activated => activated;
        private void Awake() {
            child = transform.Find("Info");
        }

        private void Start() {
            activeScale = child.localScale;
            child.localScale = Vector3.zero;
            targetScale = new Vector3(0,0,0);
        }

        private void Update() {
            child.localScale = Vector3.Lerp(child.localScale, targetScale, lerp);
        }

        public void Activate(bool state) {
            activated = state;
            targetScale = activated ? activeScale : Vector3.zero;
        }

    }
}
