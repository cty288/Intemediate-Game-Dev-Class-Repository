using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Diamond : MonoBehaviour {

        public bool useRigidbody = false;

        private void Update() {
            GetComponent<Rigidbody2D>().bodyType = useRigidbody ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.collider.gameObject.name == "Player")
            {
                GameManager.Singleton.AddDiamond(1);
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name=="Player")
            {
                GameManager.Singleton.AddDiamond(1);
                Destroy(this.gameObject);
            }
        }
    }
}
