using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public enum DialogueType {
        Bubble,
        Textbox
    }
    public abstract class InteractableObject : MonoBehaviour {
        protected InfoUI infoUI;
        [SerializeField]
        protected DialogueType dialogueType;

        [SerializeField] 
        protected List<string> dialogues = new List<string>();

        protected virtual void Awake() {
            infoUI = transform.GetComponentInChildren<InfoUI>();
            if (!infoUI)
            {
                Debug.LogError("Could not find InfoUI!");
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                ActivateInfoUI(true);
            }
        }

        protected virtual void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                ActivateInfoUI(false);
            }
        }

        protected virtual void ActivateInfoUI(bool state) {
            infoUI.Activate(state);
        }
    }
}
