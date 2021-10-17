using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Week4
{
    public class LevelEndDoorNeedDiamond : InteractableObject {
        [SerializeField] 
        private int diamondNeeded = 30;

        private InfoUI infoUi;

        private bool interacted = false;

        protected override void Awake() {
            base.Awake();
            infoUi = GetComponentInChildren<InfoUI>();
        }

        protected override void Update() {
            base.Update();
            infoUi.GetComponentInChildren<TMP_Text>().text = $"x{diamondNeeded}";
            if (GameManager.Singleton.Diamond < diamondNeeded) {
                if (dialogues.Count == 0) {
                    dialogues.Add("P: I don't have enough diamond! The strange villain will be angry!");
                }
                else {
                    dialogues[0] = "P: I don't have enough diamond! The strange villain will be angry!";
                }
            }
            else {
                dialogues.Clear();
            }
        }

        protected override void OnStageChanged(int stage) {
            
        }

        protected override void OnInteract(int stage) {
            if (GameManager.Singleton.Diamond >= diamondNeeded) {
                if (!interacted) {
                    interacted = true;
                    GameManager.Singleton.AddDiamond(-diamondNeeded);
                    GameManager.Singleton.EndLevel();
                }
                
            }
        }

    }
}
