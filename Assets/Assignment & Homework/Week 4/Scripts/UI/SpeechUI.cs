using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class SpeechUI : MonoBehaviour
    {
        [SerializeField]
        private float lerp = 0.3f;

        
        private Vector3 targetScale;
        private Transform child;
        private bool activated = false;
        public bool Activated => activated;
        private void Awake()
        {
            child = transform.Find("SpeechUIRoot/SpeechUI");
        }

        private void Start()
        {
            child.localScale = Vector3.zero;
            targetScale = new Vector3(0, 0, 0);
        }

        private void Update()
        {
            child.localScale = Vector3.Lerp(child.localScale, targetScale, lerp);
        }

        public void Activate(bool state, Vector2 position)
        {
            activated = state;
            targetScale = activated ? new Vector3(1,1,1) : Vector3.zero;
            transform.position = position;

            if (state) {
                GetComponentInChildren<Animation>().Play();
            }
            else {
                GetComponentInChildren<Animation>().Stop();
            }
        }

        public void Activate(bool state)
        {
            activated = state;
            targetScale = activated ? new Vector3(1, 1, 1) : Vector3.zero;
            
            if (state)
            {
                GetComponentInChildren<Animation>().Play();
            }
            else
            {
                GetComponentInChildren<Animation>().Stop();
            }
        }
    }
}
