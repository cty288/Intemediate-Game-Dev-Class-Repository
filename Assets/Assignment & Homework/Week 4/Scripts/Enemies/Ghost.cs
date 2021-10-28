using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public enum BossState {
        ChasePlayer,
        Teleport,
        ShootFireball,
        SummonEnemies
    }
    public class Ghost : Enemy {
       
        [SerializeField]
        private BossState bossState;
        private BossState lastBossState;

        private float timer;

        [SerializeField] 
        private DialogueTrigger dialogueTrigger;

        [SerializeField] private float minimumChaseTime = 5f;
        [SerializeField] private float minimumShootFireballTime = 5f;
        [SerializeField] private float chaseAndShootSwitchDistance = 6f;

        [SerializeField] private int damageWhenKicked = 30;
        private float currentTargetTime = 0;

        private float damageReceived;
        
        protected override void Start() {
            base.Start();
            bossState = BossState.ShootFireball;
            lastBossState = bossState;
            timer = 0;
            currentTargetTime = minimumChaseTime;
            
        }

        protected override void OnDamaged(int damageAmount) {
            damageReceived += damageAmount;
        }

        protected override void DetectPlayer() {
            
        }

        protected override void CheckStateSwitch() {
            timer += Time.deltaTime;
            if (timer >= currentTargetTime) {
                ChangeStateRandom();
                timer = 0;
            }
            else {
                ChangeStateCheck();
            }


            if (lastBossState != bossState)
            {
                OnStateChanged(lastBossState, bossState);
                lastState = state;
            }
        }

        private void OnStateChanged(BossState lastBossState, BossState newState) {
           
        }

        protected override void MovementControl() {
            if (dialogueTrigger.DialogueFinished) {
                float distanceToTargetX = 0;


                switch (bossState)
                {
                    case BossState.ChasePlayer:
                        moveSpeed = chasingSpeed;
                        targetX = player.transform.position.x;
                        distanceToTargetX = targetX - transform.position.x;

                     
                        break;
                    case BossState.ShootFireball:

                       
                        break;
                    case BossState.SummonEnemies:

                       
                        break;
                    case BossState.Teleport:
                       
                        break;
                }

                
                mRigidbody.velocity = new Vector2(Mathf.Sign(distanceToTargetX) * moveSpeed, mRigidbody.velocity.y);
            }
            
        }

        private void ChangeStateCheck() {
            ignoreTimer += Time.deltaTime;
            switch (bossState)
            {
                case BossState.ChasePlayer:
                    if (damageReceived >= 60)
                    {
                        damageReceived = 0;
                        ChangeToState(BossState.Teleport);
                    }
                    if (ignoreTimer >= ignoreChaseOrShootConditionTime) {
                        //change state:
                        //1. Player changes their floor -> to teleport
                        //2. Timer up -> random new state
                        //3. Lose health >60 -> teleport
                        //4. distance > a distance -> to shoot fireballs

                        if (Mathf.Abs(this.transform.position.x - player.transform.position.x) >=
                            chaseAndShootSwitchDistance)
                        {
                            ChangeToState(BossState.ShootFireball);
                        }

                        //change floor
                    }
                    break;
                case BossState.ShootFireball:
                    if (damageReceived >= 60)
                    {
                        damageReceived = 0;
                        ChangeToState(BossState.Teleport);
                    }
                    if (ignoreTimer >= ignoreChaseOrShootConditionTime) {
                        //change state:
                        //1. Player changes their floor -> to teleport
                        //2. Timer up -> random new state
                        //3. Lose health >60 -> teleport
                        //4. distance < a distance -> to chase player
                        if (Mathf.Abs(this.transform.position.x - player.transform.position.x) <=
                            chaseAndShootSwitchDistance)
                        {
                            ChangeToState(BossState.ChasePlayer);
                        }
                    }
                   
                    break;
                case BossState.SummonEnemies:
                    //change state:
                    //instantly after 2 sec -> random state (no code here)

                    break;
                case BossState.Teleport:
                    //change state:
                    //instantly after 0.4 sec -> random state (no code here)
                    break;
            }
        }


        [SerializeField] private float ignoreChaseOrShootConditionTime = 3f;
        private float ignoreTimer = 0;
        private void ChangeToState(BossState newState) {
            bossState = newState;
            timer = 0;
            switch (newState) {
                case BossState.ChasePlayer:
                    currentTargetTime = minimumChaseTime + Random.Range(0f, 5f);
                    ignoreTimer = 0;
                    break;
                case BossState.ShootFireball:
                    currentTargetTime = minimumShootFireballTime + Random.Range(0, 5f);
                    ignoreTimer = 0;
                    break;
                case BossState.SummonEnemies:
                    currentTargetTime = 2f;
                    break;
                case BossState.Teleport:
                    currentTargetTime = 0.4f;
                    break;
            }
        }

        private void ChangeStateRandom() {
            int index = Random.Range(0, 4);
            while (index == (int) bossState) {
                index = Random.Range(0, 4);
            }
            ChangeToState((BossState) index);
        }

        protected override void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.name == "Player" && Alive)
            {
                if (GameManager.Singleton.Player.GetComponent<Rigidbody2D>().
                    velocity.y < 0)
                {
                    DealDamage(damageWhenKicked);
                    Rigidbody2D plaRigidbody = player.GetComponent<Rigidbody2D>();
                    plaRigidbody.velocity = new Vector2(plaRigidbody.velocity.x, 10);
                }

            }
        }
    }
}
