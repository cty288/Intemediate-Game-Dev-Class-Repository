using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class MovingPlatform : MonoBehaviour {
        [SerializeField] 
        private Vector2 XRange;

        [SerializeField] 
        private float speed;

        private float currentSpeed;

        private void Start() {
            currentSpeed = speed;
        }

        private void Update() {
            
            transform.position = new Vector3(transform.position.x+currentSpeed*Time.deltaTime, transform.position.y);

            if (transform.position.x >= XRange.y) {
                currentSpeed = -speed;
            }

            if (transform.position.x <= XRange.x) {
                currentSpeed = speed;
            }
            
        }

        private void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.name == "Player") {
                other.gameObject.transform.SetParent(this.transform);
            }
        }

        private void OnCollisionExit2D(Collision2D other) {
            if (other.gameObject.name == "Player")
            {
                other.gameObject.transform.SetParent(null);
                //other.gameObject.GetComponent<Rigidbody2D>().velocity += new Vector2(currentSpeed, 0);
            }
        }
    }
}
