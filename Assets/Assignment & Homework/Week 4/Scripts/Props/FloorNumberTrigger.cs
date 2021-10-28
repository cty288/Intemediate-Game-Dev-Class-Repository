using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class FloorNumberTrigger : MonoBehaviour {
        [SerializeField] private int floor = 1;
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name == "Player") {
                BossLevelManager.Singleton.Floor = floor;
            }
        }
    }
}
