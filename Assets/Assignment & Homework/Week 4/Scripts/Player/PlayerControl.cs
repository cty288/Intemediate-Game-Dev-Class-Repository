using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4{
    public enum PlayerState {
        Idle,
        Run,
        Jump,
        Talking,
        Dead
    }

    public class PlayerControl : MonoBehaviour {
        
        [SerializeField]
        private float jumpForce = 5;

        [SerializeField] 
        private float acceleration = 0.5f;
        [SerializeField] 
        private float maxSpeed = 3;
        [SerializeField] 
        private float friction = 0.3f;


        private Rigidbody2D rigidbody;
        private float moveX = 0;
        [SerializeField]
        private float speed = 0;
        public float Speed => speed;

        private TriggerCheck jumpCheck;

        [SerializeField] private PlayerState playerState;
        private PlayerState lastState;

        public PlayerState PlayerState {
            get => playerState;
            set => playerState = value;
        }

        public static PlayerControl PlayerSingleton;
 
        

        public bool Grounded {
            get {
                return jumpCheck.Triggered;
            }
        }

        private void Awake() {
            PlayerSingleton = this;
            rigidbody = GetComponent<Rigidbody2D>();
            jumpCheck = transform.Find("JumpCheck").GetComponent<TriggerCheck>();
        }

        private void Start() {
            playerState = PlayerState.Idle;
            lastState = playerState;
        }

        private void FixedUpdate() {

            speed += moveX * acceleration;
            speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
            if (moveX == 0) {
                speed *= friction;
            }

            if (Mathf.Abs(rigidbody.velocity.x) >= 0.5 || Grounded) {
                rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
            }
           
        }



        private void Update()
        {
            Time.timeScale = 1;
            MovementControl();
            UpdateState();
        }

        private void UpdateState() {

            if (playerState != PlayerState.Talking) {
                if (Mathf.Abs(speed) <= 0.5 && Mathf.Abs(rigidbody.velocity.y) <= 0.5 && playerState != PlayerState.Dead)
                {
                    playerState = PlayerState.Idle;
                }
                else if (!Grounded)
                {
                    playerState = PlayerState.Jump;
                }
                else if (Mathf.Abs(speed) >= 0.5)
                {
                    playerState = PlayerState.Run;
                }

            }



            if (lastState != playerState) {
                SimpleEventSystem.OnPlayerStateUpdate?.Invoke(lastState, playerState);
                lastState = playerState;
                
            }
        }

        private void MovementControl()
        {
            if (playerState != PlayerState.Talking) {
                moveX = Input.GetAxis("Horizontal");

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }
            }
            else {
                moveX = 0;
            }
          
        }

        private void Jump()
        {
            if (Grounded)
            {
                rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

        
    }
}

