using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class RespawnPointTrigger : MonoBehaviour
    {
        private void OnTriggerStay2D(Collider2D other) {
            if (other.gameObject.name == "Player") {
                GameManager.Singleton.SetRespawnInfo(transform.position);
            }
        }
    }
}
