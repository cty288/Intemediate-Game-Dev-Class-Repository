using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class LevelEndDoor : MonoBehaviour {
        private bool interacted = false;

        private void Start() {
            
        }

        private void Update() {
            GetComponentInChildren<InfoUI>().Activate(true);
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                if (Input.GetKey(KeyCode.F) && !interacted) {
                    interacted = true;
                    GameManager.Singleton.EndLevel();
                }
            }
        }
    }
}
