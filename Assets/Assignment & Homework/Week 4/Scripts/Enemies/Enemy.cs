using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Week4
{
    public enum EnemyState {
        Patrol,
        Chasing
    }
    public class Enemy : MonoBehaviour {
        protected Animator animator;
        protected EnemyState state = EnemyState.Patrol;
        protected EnemyState lastState = EnemyState.Patrol;
        [SerializeField]
        protected float targetX;
        protected Rigidbody2D rigidbody;
        protected PlayerControl player;

        private bool directionSwitched = false;
       

        [SerializeField] 
        protected int health = 100;

        [SerializeField] 
        protected Vector2 posXLimit;

        [SerializeField] 
        protected float viewDistance = 5;

        [SerializeField] 
        protected float moveSpeed = 2f;

        [SerializeField] 
        protected Transform playerDetectorTransform;

        [SerializeField] 
        protected Transform groundDetectorTransform;
        
        private void Awake() {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        }

        private void Start() {
            targetX = posXLimit.y;
           
        }

        private void Update() {
            DetectPlayer();
            CheckStateSwitch();
            MovementControl();
            ChangeDirection();
        }

        private void CheckStateSwitch() {
            if (lastState != state) {
                OnStateChanged(lastState,state);
                lastState = state;
            }
        }

        private void OnStateChanged(EnemyState lastState, EnemyState newState) {
            switch (newState) {
                case EnemyState.Patrol:
                    targetX = posXLimit.x;
                    break;
                case EnemyState.Chasing:
                    break;
            }
        }

        private void ChangeDirection() {
            float distanceToTargetX = targetX - transform.position.x;
            if (distanceToTargetX > 0) {
                transform.rotation = Quaternion.Euler(0,-180,0);
            }

            if (distanceToTargetX < 0) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }

        private float blindTimer = 0;
        private void DetectPlayer() {
            blindTimer -= Time.deltaTime;
            RaycastHit2D hit =  Physics2D.Raycast(playerDetectorTransform.position, -transform.right, viewDistance);

            if (hit.collider) {
                if (hit.collider.CompareTag("Player")) {
                    //check grounded
                    RaycastHit2D hitGround = Physics2D.Raycast(groundDetectorTransform.position,
                        Vector3.Normalize(-transform.right + new Vector3(0, -1, 0)),
                        0.5f);
                    if (hitGround.collider && blindTimer<=0) {
                        Debug.Log("Hit Player");
                        state = EnemyState.Chasing;
                        OnChasingPlayer();
                    }
                    else {
                        Debug.Log("Detected player, but can't reach him");
                        state = EnemyState.Patrol;
                        blindTimer = 0.5f;
                    }

                }
            }
            else {
                state = EnemyState.Patrol;
            }
        }

        protected virtual void OnChasingPlayer() {

        }

        private void MovementControl() {
            float distanceToTargetX = 0;

            if (state == EnemyState.Patrol)
            {
                 distanceToTargetX = targetX - transform.position.x;

                if (Mathf.Abs(distanceToTargetX) <= 1 && !directionSwitched)
                {
                    directionSwitched = true;
                    targetX = Mathf.Abs(targetX - posXLimit.x) <= 0.1
                        ? posXLimit.y : posXLimit.x;
                }
                else
                {
                    directionSwitched = false;
                }
            }
            else {
                targetX = player.transform.position.x;
                distanceToTargetX = targetX - transform.position.x;
            }

            rigidbody.velocity = new Vector2(Mathf.Sign(distanceToTargetX) * moveSpeed, rigidbody.velocity.y);
        }
    }
}
