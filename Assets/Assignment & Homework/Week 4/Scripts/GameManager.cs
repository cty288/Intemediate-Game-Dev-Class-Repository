using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Week4
{
    public class GameManager : MonoBehaviour {
        public static GameManager Singleton;

        private PlayerControl player;

        private int life = 3;
        public int Life => life;
        private int diamond = 0;
        public int Diamond => diamond;

        private int key = 0;
        public int Key => key;


        private void Awake() {
            if (Singleton != null) {
                DestroyImmediate(this.gameObject);
            }
            else {
                Singleton = this;
                player = PlayerControl.PlayerSingleton;
                DontDestroyOnLoad(this.gameObject);
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
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

        public void ChangeKey(int num) {
            int oldKey = key;
            key += num;
            SimpleEventSystem.OnKeyChange?.Invoke(oldKey,key);
        }

        public void AddDiamond(int num) {
            diamond += num;
            SimpleEventSystem.OnDiamondChange?.Invoke(diamond - num, diamond);
        }

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerStateUpdate -= OnPlayerStateUpdate;
        }

        public void RestartCurrentLevel() {
            diamond = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
