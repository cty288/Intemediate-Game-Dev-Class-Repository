using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4{
    public enum PlayerState {
        Menu,
        Idle,
        Run,
        Jump,
        Talking,
        End,
        Dead
    }
    //control improvement
    public class PlayerControl : MonoBehaviour {
        
        [SerializeField]
        private float jumpForce = 5;

        [SerializeField] private float jumpForceWithBoot = 8;

        //[SerializeField] 
       // private float acceleration = 0.5f;
        [SerializeField] 
        private float maxSpeed = 3;
        [SerializeField] 
        private float friction = 0.3f;


        private Rigidbody2D mRigidbody;
        private float moveX = 0;
       
        [SerializeField]
        private float speed = 0;
        public float Speed => speed;

        private TriggerCheck jumpCheck;

        [SerializeField] private PlayerState playerState;
        [SerializeField]
        private PlayerState lastState;

        public PlayerState PlayerState {
            get => playerState;
            set => playerState = value;
        }

        public static PlayerControl PlayerSingleton;

        private TriggerCheck wallCheck;

        public bool Grounded {
            get {
                return jumpCheck.Triggered;
            }
        }

        private void Awake() {
            PlayerSingleton = this;
            mRigidbody = GetComponent<Rigidbody2D>();
            jumpCheck = transform.Find("JumpCheck").GetComponent<TriggerCheck>();
            wallCheck = transform.Find("WallCheck").GetComponent<TriggerCheck>();
        }

        private void Start() {
            if (GameManager.Singleton.GetCurrentLevelNum() == 1) {
                playerState = PlayerState.Menu;
            }
            else {
                playerState = PlayerState.Idle;
            }
           
            lastState = playerState;
        }

        private void FixedUpdate() {
            if (playerState != PlayerState.Menu) {
                if (moveX != 0)
                {
                    speed = moveX * maxSpeed;
                }

                speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
                if (moveX == 0)
                {
                    speed *= friction;
                }

                if (Mathf.Abs(mRigidbody.velocity.x) >= 0.5 || Grounded || Mathf.Abs(moveX) >= 0)
                {
                    if (!wallCheck.Triggered)
                    {
                        mRigidbody.velocity = new Vector2(speed, mRigidbody.velocity.y);
                    }

                }
            }
          

        }



        private void Update()
        {
            Time.timeScale = 1;

            if (playerState != PlayerState.Menu) {
                MovementControl();
                UpdateState();
            }
        
            
        }

        private void UpdateState() {
            

            if (playerState != PlayerState.Talking && playerState!= PlayerState.Dead && playerState!=PlayerState.End) {
                if (Mathf.Abs(speed) <= 0.5 && Mathf.Abs(mRigidbody.velocity.y) <= 0.5 && playerState != PlayerState.Dead)
                {
                    playerState = PlayerState.Idle;
                }
                else if (!Grounded && playerState != PlayerState.Dead)
                {
                   
                    playerState = PlayerState.Jump;
                }
                else if (Mathf.Abs(speed) >= 0.5)
                {
                    playerState = PlayerState.Run;
                }

            }

            if (transform.position.y <= -8)
            {
                playerState = PlayerState.Dead;
            }


            if (lastState != playerState) {
                SimpleEventSystem.OnPlayerStateUpdate?.Invoke(lastState, playerState);
                lastState = playerState;

            }
        }

        private void MovementControl()
        {
            if (playerState != PlayerState.Talking && playerState!= PlayerState.Dead && playerState!=PlayerState.End) {
                moveX = Input.GetAxis("Horizontal");

                if (Input.GetKey(KeyCode.Space))
                {
                    Jump();
                }

                if (Input.GetKeyUp(KeyCode.Space)) {
                    jumping = false;
                    jumpTimer = 0;
                }
            }
            else {
                moveX = 0;
            }
          
        }

        private bool jumping = false;
        [SerializeField]
        private float minimumJumpHoldTime = 0.8f;

        [SerializeField]
        private float minimumJumpForce = 2f;
        [SerializeField]
        private float minimumJumpForceWithBoot = 4f;
        private float jumpTimer = 0;
        private void Jump()
        {
            if (Grounded) {
                if (!jumping) {
                    float force = 0;
                    if (GameManager.Singleton.HasBoot())
                    {
                        force = (minimumJumpForceWithBoot + (GameManager.Singleton.Boot - 1) * jumpForceWithBoot / 6);
                    }
                    else
                    {
                        force = minimumJumpForce;
                    }
                    mRigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                }
                jumping = true;
            }

            if (jumping) {
                float force = 0;
                if (GameManager.Singleton.HasBoot()) {
                    force = (jumpForceWithBoot + (GameManager.Singleton.Boot - 1) * jumpForceWithBoot / 6);
                }
                else {
                    force = jumpForce;
                }

                jumpTimer += Time.deltaTime;
                if (jumpTimer <= minimumJumpHoldTime) {
                    mRigidbody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                }
            }
        }

        
    }
}

