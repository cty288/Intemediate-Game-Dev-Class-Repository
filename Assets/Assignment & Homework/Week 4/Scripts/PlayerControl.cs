using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4{
    public enum PlayerState {
        Idle,
        Run,
        Jump,
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

        public PlayerState PlayerState => playerState;


        /// <summary>
        ///   event triggered when player state changed. Pass old and new state to the callback function
        /// </summary>
        public Action<PlayerState,PlayerState> OnPlayerStateUpdate;

        public bool Grounded {
            get {
                return jumpCheck.Triggered;
            }
        }

        private void Awake() {
            Time.timeScale = 1;
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
            MovementControl();
            UpdateState();
        }

        private void UpdateState() {
            if (Mathf.Abs(speed) <= 0.5 && Mathf.Abs(rigidbody.velocity.y) <= 0.5 && playerState!=PlayerState.Dead) {
                playerState = PlayerState.Idle;
            }else if (!Grounded) {
                playerState = PlayerState.Jump;
            }else if (Mathf.Abs(speed) >= 0.5) {
                playerState = PlayerState.Run;
            }

            if (lastState != playerState) {
                OnPlayerStateUpdate?.Invoke(lastState, playerState);
                lastState = playerState;
                
            }
        }

        private void MovementControl()
        {
            moveX = Input.GetAxis("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
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

