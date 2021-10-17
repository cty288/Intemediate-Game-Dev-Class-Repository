using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Week4
{
    public class Saw : MonoBehaviour {
        [SerializeField] 
        private float speed = 1f;

        [SerializeField] 
        private float minimumWaitTime = 2f;

        [SerializeField] 
        private bool moveUp = false;

        private bool down = true;
        private float startHeight;
        private float endHeight;

        private float waitTime = 0;
        private float timer = 0;
        private float targetHeight;

        private PlayerControl player;

        private void Awake() {
            
        }

        private void Start() {
            player = GameObject.Find("Player").GetComponent<PlayerControl>();
            endHeight = transform.position.y;
            if (moveUp) {
                startHeight = endHeight + 0.7f;
            }
            else {
                startHeight = endHeight - 0.7f;
            }
            
            GenerateNewWaitTime();
        }

        private void Update() {
            timer += Time.deltaTime;
            if (timer >= waitTime) {
                down = !down;
                timer = 0;
                GenerateNewWaitTime();
            }

            targetHeight = down ? startHeight : endHeight;

            float y = Mathf.Lerp(transform.position.y, targetHeight, speed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        private void GenerateNewWaitTime() {
            waitTime = minimumWaitTime + Random.Range(0f, 3f);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name=="Player") {
                if (!down) {
                    player.PlayerState = PlayerState.Dead;
                }
                
            }
        }
    }
}
