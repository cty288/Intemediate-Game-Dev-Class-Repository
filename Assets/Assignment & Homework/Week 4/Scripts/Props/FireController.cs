using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class FireController : MonoBehaviour {
        [SerializeField] 
        private float period = 2f;

        private bool isOn = false;
        private Animator animator;
        private float timer = 0;

        private bool gameEnds = false;
        private void Awake() {
            animator = GetComponent<Animator>();
        }

        private void Start() {
            SimpleEventSystem.OnBossDie += OnBossDie;
        }

        private void OnDestroy() {
            SimpleEventSystem.OnBossDie -= OnBossDie;
        }

        private void OnBossDie() {
            gameEnds = true;
        }

        private void Update() {
            if (!gameEnds) {
                timer += Time.deltaTime;
                if (timer >= period)
                {
                    timer = 0;
                    isOn = !isOn;
                    if (isOn)
                    {
                        animator.SetTrigger("On");
                    }
                    else
                    {
                        animator.SetTrigger("Off");
                    }
                }
            }
            else {
                isOn = false;
                animator.SetTrigger("Off");
            }
            
        }

        private void OnTriggerStay2D(Collider2D other) {
            if (isOn) {
                if (other.gameObject.name == "Player") {
                    GameManager.Singleton.GetPlayer().KillPlayer();
                }
            }
        }
    }
}
