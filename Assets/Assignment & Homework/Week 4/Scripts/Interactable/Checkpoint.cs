using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class RespawnInfo {
        public Vector2 RespawnPoint;
    }

    public class Checkpoint : InteractableObject {
        //[SerializeField] private bool showInfo=false;

        private Transform respawnPoint;

        protected override void Awake() {
            base.Awake();
            respawnPoint = transform.Find("RespawnPoint");
            if (!respawnPoint) {
                Debug.LogError("No Respawn Point attached to this object! Add a new game object as" +
                               "its child");
            }

        }

        protected override void OnStageChanged(int stage) {
            
        }

        protected override void OnInteract(int stage) {
            GameManager.Singleton.SetRespawnInfo(respawnPoint.position);
            infoUI.gameObject.SetActive(false);
        }
    }
}
