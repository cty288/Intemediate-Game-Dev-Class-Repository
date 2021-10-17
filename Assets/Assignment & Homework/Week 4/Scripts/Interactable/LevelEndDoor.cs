using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class LevelEndDoor : MonoBehaviour {
        protected bool interacted = false;

        protected InfoUI infoUi;
        private void Awake() {
            infoUi = GetComponentInChildren<InfoUI>();
        }

        protected virtual void Update() {
           infoUi.Activate(true);
        }

        protected virtual void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                if (Input.GetKey(KeyCode.F) && !interacted) {
                    interacted = true;
                    GameManager.Singleton.EndLevel();
                }
            }
        }
    }
}
