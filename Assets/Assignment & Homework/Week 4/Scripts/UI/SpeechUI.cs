using System.Collections;
using System.Collections.Generic;
using Codice.CM.Client.Differences.Merge;
using TMPro;
using UnityEngine;


namespace Week4
{
    public class SpeechUI : MonoBehaviour
    {
        [SerializeField]
        private float lerp = 0.3f;

        [SerializeField] private float timePerCharacter = 0.1f;

        private float characterTimer = 0;
        private int currentCharacterPos = -1;

        private Vector3 targetScale;
        private Transform child;
        private bool activated = false;
        private TMP_Text speechText;
        private string printerTargetText;
        public bool Activated => activated;
        private void Awake()
        {
            child = transform.Find("SpeechUIRoot/SpeechUI");
            speechText = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            child.localScale = Vector3.zero;
            targetScale = new Vector3(0, 0, 0);
        }

        private void Update()
        {
            child.localScale = Vector3.Lerp(child.localScale, targetScale, lerp);
            if (activated) {
                characterTimer += Time.deltaTime;
                if (characterTimer > timePerCharacter) {
                    characterTimer = 0;
                    currentCharacterPos++;
                    
                    if (currentCharacterPos < printerTargetText.Length) {
                        speechText.text = printerTargetText.Substring(0, currentCharacterPos + 1);
                    }
                }
            }
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

        public void SetText(string text) {
            printerTargetText = text;
            characterTimer = 0;
            currentCharacterPos = -1;
        }

        public void ResetMsg() {
            printerTargetText = "";
            speechText.text = "";
            characterTimer = 0;
            currentCharacterPos = -1;
        }
        
    }
}
