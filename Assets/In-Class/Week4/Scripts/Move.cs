using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Week4 {
    public class Move : MonoBehaviour {
        private Rigidbody2D rigidbody;

        [SerializeField] 
        private float speed=1, jumpForce=5;

        private bool grounded = false;

        private float moveX=0;

        private void Awake() {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            rigidbody.velocity += new Vector2(moveX * speed, 0);
        }

        private void Update() {
            Time.timeScale = 1;
            PlayerControl();
        }

        private void PlayerControl() {
            moveX =  Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }
        }

        private void Jump() {
            if (grounded) {
                rigidbody.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            }
        }

        private void OnCollisionEnter2D(Collision2D other) {
            grounded = true;
        }

        private void OnCollisionExit2D(Collision2D other) {
            grounded = false;
        }
    }
}

