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

        [SerializeField] 
        private List<Vector2> FloorXRanges = new List<Vector2>();

        [SerializeField] 
        private List<float> FloorY = new List<float>();

        private Collider2D[] colliders;

        [SerializeField] private Vector3 speechBubbleRelativePosition;

        [SerializeField] private float minimumSummonedSpawnerTime = 10;

        [SerializeField]
        private GameObject enemySpawnerPrefab;

        [SerializeField] 
        private SpeechUI speechUI;

        [SerializeField] private List<float> randomStateSwitchPossibility = new List<float>();
        protected override void Start() {
            base.Start();
            bossState = BossState.ShootFireball;
            lastBossState = bossState;
            timer = 0;
            currentTargetTime = minimumChaseTime;
            SimpleEventSystem.OnPlayerFloorChange += OnPlayerFloorChange;
            colliders = GetComponents<Collider2D>();
        }

        protected override void AnimationControl() {
          
        }

        private void OnPlayerFloorChange(int oldFloor, int newFloor) {
            ChangeToState(BossState.Teleport);
        }

        protected override void OnDestroy() {
            base.OnDestroy();
            SimpleEventSystem.OnPlayerFloorChange -= OnPlayerFloorChange;
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
                        moveSpeed = 0;
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
                    //instantly after 2 sec -> random state 
                    if (!isSummoning) {
                        isSummoning = true;
                        StartCoroutine(StartSummoning());
                    }
                    else {
                        speechUI.transform.position = transform.position + speechBubbleRelativePosition;
                    }

                    
                    break;
                case BossState.Teleport:
                    //change state:
                    //instantly after 1.7 sec -> random state
                    if (!isTeleporting) {
                        isTeleporting = true;
                        StartCoroutine(StartTeleport());
                    }
                   
                    break;
            }
        }

        private bool isTeleporting = false;
        private IEnumerator StartTeleport() {
            animator.SetTrigger("disappear");
            CollidersSwitch(false);
            
            yield return new WaitForSeconds(0.8f);
            TeleportToPlayer();
            animator.SetTrigger("appear");
            yield return new WaitForSeconds(0.7f);
            CollidersSwitch(true);
            
        }

        private bool isSummoning = false;

        private IEnumerator StartSummoning() {
            speechUI.Activate(true);
            speechUI.SetText("Dwae apq$!@%&* !!!");
            yield return new WaitForSeconds(2);
            speechUI.Activate(false);
            speechUI.ResetMsg();
            StartCoroutine(SpawnEnemySpawner());
        }

        private IEnumerator SpawnEnemySpawner() {
            int playerFloor = BossLevelManager.Singleton.Floor;
            Vector2 floorXRange = FloorXRanges[playerFloor - 1];
            float spawnX = Random.Range(floorXRange.x, floorXRange.y);
            float spawnY = FloorY[playerFloor - 1] - 1.4f;

            GameObject spawner = Instantiate(enemySpawnerPrefab, new Vector3(spawnX, spawnY),
                Quaternion.identity);

            Animator spawnerAnimator = spawner.GetComponent<Animator>();
            EnemySpawner enemySpawner = spawner.GetComponent<EnemySpawner>();

            float spawnLastTime = minimumSummonedSpawnerTime + Random.Range(0f, 10f);
           
            enemySpawner.SpawnXLimits = floorXRange;

            yield return new WaitForSeconds(spawnLastTime);

            spawnerAnimator.SetTrigger("Despawn");

            yield return new WaitForSeconds(1f);

            Destroy(spawner);
        }


        private void TeleportToPlayer() {
            int playerFloor = BossLevelManager.Singleton.Floor;
            float teleportX = Random.Range(FloorXRanges[playerFloor - 1].x, FloorXRanges[playerFloor - 1].y);
            transform.position = new Vector3(teleportX, FloorY[playerFloor - 1], transform.position.z);
        }

        private void CollidersSwitch(bool isOn) {
            foreach (Collider2D collider in colliders) {
                collider.enabled = isOn;
            }
            transform.Find("EnemyHealthBarCanvas").gameObject.SetActive(isOn);
        }

        [SerializeField] private float ignoreChaseOrShootConditionTime = 3f;
        private float ignoreTimer = 0;
        private void ChangeToState(BossState newState) {
            bossState = newState;
            timer = 0;
            isTeleporting = false;
            isSummoning = false;
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
                    currentTargetTime = 1.5f;
                    break;
            }
        }

        private void ChangeStateRandom() {
          
            int index;

            do {
                int possibility = Random.Range(0, 101);
                if (possibility >= 0 && possibility < randomStateSwitchPossibility[0]) {
                    index = 0;
                }
                else if (possibility >= randomStateSwitchPossibility[0] &&
                         possibility < randomStateSwitchPossibility[1]) {
                    index = 1;
                }
                else if (possibility >= randomStateSwitchPossibility[1] &&
                         possibility < randomStateSwitchPossibility[2]) {
                    index = 2;
                }
                else {
                    index = 3;
                }
            } while (index == (int) bossState);

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
