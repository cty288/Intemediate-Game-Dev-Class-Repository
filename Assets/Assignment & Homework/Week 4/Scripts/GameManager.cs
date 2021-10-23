using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Week4
{
    public class GameManager : MonoBehaviour {
        public static GameManager Singleton;

        private PlayerControl player;
        public PlayerControl Player => player;

        [SerializeField]
        private int life = 5;
        public int Life => life;
        [SerializeField]
        private int diamond = 0;
        [SerializeField] private int maxBackpackCapacity = 10;
        public int Diamond => diamond;

        private int key = 0;
        public int Key => key;

      

        private RespawnInfo respawnInfo;


        public List<ItemType> ItemsPicked = new List<ItemType>();

        private int bullet = 0;
        public int Bullet => bullet;
        private int boot = 0;
        public int Boot => boot;

        private void Awake() {
            if (Singleton != null) {
                DestroyImmediate(this.gameObject);
            }
            else {
                Singleton = this;
                //player = PlayerControl.PlayerSingleton;
                DontDestroyOnLoad(this.gameObject);
                player = GameObject.Find("Player").GetComponent<PlayerControl>();
            }

           
        }

        private void Update() {
            if (!player) {
                player = GameObject.Find("Player").GetComponent<PlayerControl>();
            }

            if (Input.GetKeyDown(KeyCode.N)) {
                GoToNextLevel();
            }
        }

        private void Start() {
            SimpleEventSystem.OnPlayerStateUpdate += OnPlayerStateUpdate;
        }

        private void OnPlayerStateUpdate(PlayerState old, PlayerState newState) {
            if (newState == PlayerState.Dead)
            {
                AddLife(-1);
            }
        }


        public void AddLife(int num) {
            int oldLife = life;
            life += num;
            life = Mathf.Clamp(Life,0,10);
            if (life != oldLife) {
                SimpleEventSystem.OnLifeChange?.Invoke(oldLife, life);
            }
            
        }

        

        [SerializeField] 
        private GameObject diamondPrefab;
        public void SpawnDiamonds(Vector3 position,int count) {
            for (int i = 0; i < count; i++)
            {

                GameObject diamondGo = Instantiate(diamondPrefab, position,
                    Quaternion.identity);

                diamondGo.GetComponent<BoxCollider2D>().isTrigger = false;
                diamondGo.GetComponent<Diamond>().useRigidbody = true;

            }
        }
        public void SpawnDiamond(Vector3 position)
        {
        
            SpawnDiamonds(position,1);
        }

        public int GetCurrentLevelNum() {
            return SceneManager.GetActiveScene().buildIndex + 1;
        }

        public void EndLevel() {
            
            player.PlayerState = PlayerState.End;
            int lifeAdd = Random.Range(1, 6);
            AddLife(lifeAdd);
            SimpleEventSystem.OnGameEnds?.Invoke(lifeAdd,GetCurrentLevelNum());
        }

        public void ChangeKey(int num) {
            int oldKey = key;
            key += num;
            SimpleEventSystem.OnKeyChange?.Invoke(oldKey,key);
        }

        public PlayerControl GetPlayer() {
            return GameObject.Find("Player").GetComponent<PlayerControl>();
        }

        public void AddDiamond(int num) {
            diamond += num;
            SimpleEventSystem.OnDiamondChange?.Invoke(diamond - num, diamond);
        }

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerStateUpdate -= OnPlayerStateUpdate;
        }

        public void ResetToFirstLevel() {
            diamond = 0;
            key = 0;
            respawnInfo = null;
            life = 5;
            ItemsPicked.Clear();
            SceneManager.LoadScene(0);
        }

        public void PickItem(Item itemPicked) {
            bool canPick = true;
            int insertIndex = -1;
            for (int i=0; i< ItemsPicked.Count; i++) {
                if (ItemsPicked[i] == itemPicked.ItemType) {
                    canPick = itemPicked.CanPickMultipleTime;
                    insertIndex = i;
                    break;
                }
            }

            if (canPick) {
                if (ItemsPicked.Count < maxBackpackCapacity) {
                    if (insertIndex != -1)
                    {
                        ItemsPicked.Insert(insertIndex, itemPicked.ItemType);
                    }
                    else
                    {
                        ItemsPicked.Add(itemPicked.ItemType);
                    }

                    switch (itemPicked.ItemType) {
                        case ItemType.Boot:
                            boot++;
                            break;
                        case ItemType.Bullet:
                            bullet++;
                            break;
                        
                    }
                    SimpleEventSystem.OnPlayerPickItem?.Invoke(itemPicked.ItemType);
                }
            }
          
        }


        public void GoToNextLevel() {
            
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int totalScenes = SceneManager.sceneCountInBuildSettings;

            SceneManager.LoadScene((currentSceneIndex+1) % totalScenes);
        }

        public void Respawn() {
            player.GetComponent<Rigidbody2D>().simulated = false;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            player.transform.position = respawnInfo.RespawnPoint;
            player.PlayerState = PlayerState.Idle;

            player.GetComponent<Rigidbody2D>().simulated = true;
            ClearInventory();
            SimpleEventSystem.OnPlayerRespawn?.Invoke();
        }

        public void ClearInventory() {
            ItemsPicked.Clear();
            bullet = 0;
            boot = 0;
        }

        public void SetRespawnInfo(Vector2 respawnPoint) {

            respawnInfo = new RespawnInfo() {
                RespawnPoint = respawnPoint
            };
        }

        public bool CanShoot() {
            foreach (ItemType itemType in ItemsPicked) {
                if (itemType == ItemType.Bullet) {
                    return true;
                }
            }

            return false;
        }

        public bool HasBoot() {
            foreach (ItemType itemType in ItemsPicked)
            {
                if (itemType == ItemType.Boot)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
