using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Bat : Enemy {
        [SerializeField] 
        private float stayAtX;

        [SerializeField] 
        private float fireballShootPeriod = 2f;

        [SerializeField] private GameObject fireballPrefab;

        private float timer = 0;
        protected override void Start() {
            base.Start();
        }

        protected override void MovementControl() {
            float distanceToTargetX = 0;

            if (state == EnemyState.Patrol) {
                targetX = stayAtX;
                distanceToTargetX = targetX - transform.position.x;

                if (Mathf.Abs(distanceToTargetX) <= 1) {
                    moveSpeed = 0;
                }else {
                    moveSpeed = patrolSpeed;

                }
            }
            else
            {
                moveSpeed = chasingSpeed;
                targetX = player.transform.position.x + 2;
                distanceToTargetX = targetX - transform.position.x;
                if (Mathf.Abs(distanceToTargetX) <= 0.1) {
                    moveSpeed = 0;
                }
                OnChasingPlayer();
            }

            mRigidbody.velocity = new Vector2(Mathf.Sign(distanceToTargetX) * moveSpeed, mRigidbody.velocity.y);
        }

        protected override void OnChasingPlayer() {
            timer += Time.deltaTime;
            if (timer >= fireballShootPeriod) {
                timer = 0;
                ShootFireBall();
            }
        }

        private void ShootFireBall() {
            Fireball fireBall = Instantiate(fireballPrefab, GetComponentInChildren<Transform>().position,
                Quaternion.identity).GetComponent<Fireball>();
            Vector2 angleDiff = Vector3.Normalize(player.transform.position - transform.position);
            float angle = Mathf.Atan2(angleDiff.y, angleDiff.x);
            float rotateAngle = angle * 180 / Mathf.PI;

            Debug.Log(rotateAngle);
            fireBall.transform.Rotate(0,0,rotateAngle);
            fireBall.Speed = 8;
            fireBall.Shooter = this.gameObject;
        }

        protected override void OnStateChanged(EnemyState lastState, EnemyState newState) {
            
        }

        protected override void DetectPlayer() {
           
        }

        protected override void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name == "Player" && Alive) {
                state = EnemyState.Chasing;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.gameObject.name == "Player" && Alive)
            {
                state = EnemyState.Patrol;
            }
        }

        protected override void AnimationControl() {
            float distanceToTargetX = 0;

            if (state == EnemyState.Patrol)
            {
                animator.SetBool("ChasePlayer", false);
                targetX = stayAtX;
                distanceToTargetX = targetX - transform.position.x;

                if (Mathf.Abs(distanceToTargetX) <= 1)
                {
                    animator.SetTrigger("Idle");
                }
                else
                {
                    animator.SetTrigger("PlayerLeave");

                }
            }
            else
            {
                animator.SetBool("ChasePlayer",true);
            }
        }

        protected override void ChangeDirection() {
            float distanceToTargetX = 0; //targetX - transform.position.x;
            switch (state) {
                case EnemyState.Chasing:
                    distanceToTargetX = player.transform.position.x - transform.position.x;
                    break;
                case EnemyState.Patrol:
                    distanceToTargetX = targetX - transform.position.x;
                    break;
            }

            if (distanceToTargetX > 0)
            {
                transform.rotation = Quaternion.Euler(0, -180, 0);
            }

            if (distanceToTargetX < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
