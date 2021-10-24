using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class DieTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name == "Player") {
                GameManager.Singleton.GetPlayer().PlayerState = PlayerState.Dead;
            }
        }
    }
}
