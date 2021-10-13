using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Week4
{
    public class MessageBox : MonoBehaviour {
       
        [SerializeField]
        private float lerp = 0.3f;

        [SerializeField] private float timePerCharacter = 0.1f;

        [SerializeField]
        private GameObject root;

        private float characterTimer = 0;
        private int currentCharacterPos = -1;

        private bool activated = false;
        private TMP_Text messageText;
        private string printerTargetText;
        public bool Activated => activated;

        private void Awake() {
           
            messageText = root.transform.Find("Message").GetComponent<TMP_Text>();
            root.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (activated)
            {
                characterTimer += Time.deltaTime;
                if (characterTimer > timePerCharacter)
                {
                    characterTimer = 0;
                    currentCharacterPos++;

                    if (currentCharacterPos < printerTargetText.Length)
                    {
                        messageText.text = printerTargetText.Substring(0, currentCharacterPos + 1);
                    }
                }
            }
        }

        public void Activate(string message)
        {
            activated = true;
            root.gameObject.SetActive(true);
            GetComponentInChildren<Animation>().Play();
            SetText(message);
        }

        public void Disactivate()
        {
            activated = false;
            GetComponentInChildren<Animation>().Stop();
            ResetMsg();
            root.gameObject.SetActive(false);
        }

        private void SetText(string text)
        {
            printerTargetText = text;
            characterTimer = 0;
            currentCharacterPos = -1;
        }

        private void ResetMsg()
        {
            printerTargetText = "";
            messageText.text = "";
            characterTimer = 0;
            currentCharacterPos = -1;
        }

        public void SetColor(Color color)
        {
            messageText.color = color;
        }
    }
}
