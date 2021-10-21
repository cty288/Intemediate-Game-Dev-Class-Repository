using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Week4
{
    public enum EnemyState {
        Patrol,
        Chasing
    }
    public class Enemy : MonoBehaviour, IDamageable {
        protected Animator animator;
        protected EnemyState state = EnemyState.Patrol;
        protected EnemyState lastState = EnemyState.Patrol;
        [SerializeField]
        protected float targetX;
        protected Rigidbody2D mRigidbody;
        protected PlayerControl player;

        protected Transform healthBarCanvasTr;
        protected float healthBarInitialScale;
        
        protected bool directionSwitched = false;

        public bool Alive {
            get {
                return health > 0;
            }
        }

        [SerializeField] 
        protected int health = 100;
        public int Health => health;

        
        protected int maxHealth;
        public int MaxHealth => maxHealth;

        [SerializeField] 
        protected Vector2 posXLimit;
        public Vector2 PosXLimit {
            get => posXLimit;
            set => posXLimit = value;
        }

        [SerializeField] 
        protected float viewDistance = 5;

        [SerializeField] 
        protected float chasingSpeed = 4f;

        [SerializeField] 
        protected float patrolSpeed = 2f;

        
        protected float moveSpeed = 2f;

        [SerializeField] 
        protected Transform playerDetectorTransform;

        [SerializeField] 
        protected Transform groundDetectorTransform;

        [SerializeField]
        protected int awardDiamondCount = 1;

        public Action<Enemy> OnEnemyDie;

        private void Awake() {
            animator = GetComponent<Animator>();
            mRigidbody = GetComponent<Rigidbody2D>();
            maxHealth = health;
            healthBarCanvasTr = GetComponentInChildren<Canvas>().transform;
        }

        protected virtual void Start() {
            targetX = posXLimit.y;
            player = GameManager.Singleton.Player;
            healthBarInitialScale = healthBarCanvasTr.localScale.x;
            SimpleEventSystem.OnPlayerStateUpdate += OnPlayerStateUpdate;
            SimpleEventSystem.OnPlayerRespawn += OnPlayerRespawn;
        }

        

        private void OnPlayerRespawn() {
            animator.SetTrigger("PlayerRespawn");
           // transform.position = new Vector2(posXLimit.y,transform.position.x);
            mRigidbody.simulated = true;
        }

        private void OnPlayerStateUpdate(PlayerState old, PlayerState state) {
            if (state == PlayerState.Dead) {
                animator.SetTrigger("Idle");
                mRigidbody.simulated = false;
            }
        }

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerStateUpdate -= OnPlayerStateUpdate;
            SimpleEventSystem.OnPlayerRespawn -= OnPlayerRespawn;
        }

        private void Update() {
            if (!player) {
                player = GameManager.Singleton.GetPlayer();
            }
            DetectPlayer();
            CheckStateSwitch();
            if (player.PlayerState != PlayerState.Dead) {
                MovementControl();
            }

            if (Alive) {
                AnimationControl();
            }
            else {
                animator.enabled = false;
            }
            
            ChangeDirection();
        }

        protected virtual void AnimationControl() {
            switch (state) {
                case EnemyState.Chasing:
                    animator.SetBool("ChasePlayer", true);
                    break;
                case EnemyState.Patrol:
                    animator.SetBool("ChasePlayer",false);
                    break;
            }

          
        }

        protected virtual void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name=="Player" && Alive)
            {
                if (GameManager.Singleton.Player.GetComponent<Rigidbody2D>().
                    velocity.y<=0) {
                    health = 0;
                    OnKilled();
                    OnEnemyDie?.Invoke(this);
                }
               
            }
        }

        private void OnKilled() {
            
            for (int i = 0; i < awardDiamondCount; i++) {
                float randomPos = Random.Range(-1f, 1f);
                GameManager.Singleton.SpawnDiamond(transform.position + new Vector3(0, 2.5f, 0) +
                                                    new Vector3(randomPos, randomPos, randomPos));
            }
           

            GetComponent<CircleCollider2D>().isTrigger = true;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, -15);
            player.GetComponent<Rigidbody2D>().velocity += new Vector2(0, 10);
            Destroy(this.gameObject,5);
        }

       

        private void OnCollisionStay2D(Collision2D other) {
           
            if (other.collider.CompareTag("Player") && Alive) {
                player.PlayerState = PlayerState.Dead;
            }
        }

       

        public void DealDamage(int damage) {
            health -= damage;
            if (health <= 0) {
                OnKilled();
                OnEnemyDie?.Invoke(this);
            }
        }

        private void CheckStateSwitch() {
            if (lastState != state) {
                OnStateChanged(lastState,state);
                lastState = state;
            }
        }

        protected virtual void OnStateChanged(EnemyState lastState, EnemyState newState) {
            switch (newState) {
                case EnemyState.Patrol:
                    targetX = posXLimit.x;
                    break;
                case EnemyState.Chasing:
                    break;
            }
        }

        protected virtual void ChangeDirection() {
            float distanceToTargetX = targetX - transform.position.x;
            if (distanceToTargetX > 0) {
                transform.rotation = Quaternion.Euler(0,-180,0);
                healthBarCanvasTr.localScale = new Vector3(-healthBarInitialScale, healthBarInitialScale,
                    healthBarInitialScale);
            }

            if (distanceToTargetX < 0) {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                healthBarCanvasTr.localScale = new Vector3(healthBarInitialScale, healthBarInitialScale,
                    healthBarInitialScale);
            }
        }

        private float blindTimer = 0;
        protected virtual void DetectPlayer() {
            blindTimer -= Time.deltaTime;
            RaycastHit2D hit =  Physics2D.Raycast(playerDetectorTransform.position, -transform.right, viewDistance);

            if (hit.collider) {
               
                if (hit.collider.CompareTag("Player")) {
                    
                    //check grounded
                    RaycastHit2D hitGround = Physics2D.Raycast(groundDetectorTransform.position,
                        Vector3.Normalize(-transform.right + new Vector3(0, -1, 0)),
                        0.5f);
                   
                    if (hitGround.collider && blindTimer<=0) {
                       
                        state = EnemyState.Chasing;

                        OnChasingPlayer();
                    }
                    else {
                        state = EnemyState.Patrol;
                        
                        blindTimer = 0.5f;
                        OnPatroling();
                    }

                }
            }
            else {
                state = EnemyState.Patrol;
                OnPatroling();
            }
        }

        protected virtual void OnChasingPlayer() {

        }

        protected virtual void OnPatroling() {

        }

        protected virtual void MovementControl() {
            float distanceToTargetX = 0;

            if (state == EnemyState.Patrol) {
                moveSpeed = patrolSpeed;
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
                moveSpeed = chasingSpeed;
                targetX = player.transform.position.x;
                distanceToTargetX = targetX - transform.position.x;
            }

            mRigidbody.velocity = new Vector2(Mathf.Sign(distanceToTargetX) * moveSpeed, mRigidbody.velocity.y);
        }
    }
}
