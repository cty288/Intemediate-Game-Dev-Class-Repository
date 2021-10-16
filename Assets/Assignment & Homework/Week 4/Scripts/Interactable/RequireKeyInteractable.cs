using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class RequireKeyInteractable : InteractableObject {
        [SerializeField] protected List<string> defaultDialogues = new List<string>();

        private Animation anim;

        private bool activated = false;

        protected override void Start() {
            base.Start();
            SimpleEventSystem.OnKeyChange += OnPlayerKeyChanged;
            anim = GetComponent<Animation>();
            dialogues = defaultDialogues;
        }

        private void OnPlayerKeyChanged(int old, int cur) {
            if (cur == 0) {
                dialogues = defaultDialogues;
            }
            else {
                dialogues.Clear();
            }
        }

        protected override void OnStageChanged(int stage) {
            
        }

        protected override void OnInteract(int stage) {
            if (GameManager.Singleton.Key > 0 && !activated) {
                GameManager.Singleton.ChangeKey(-1);
                activated = true;
                infoUI.gameObject.SetActive(false);
                if (anim) {
                    anim.Play();
                }
            }
        }

        
    }
}
