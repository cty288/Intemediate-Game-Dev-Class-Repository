using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Diamond : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                GameManager.Singleton.AddDiamond(1);
                Destroy(this.gameObject);
            }
        }
    }
}
