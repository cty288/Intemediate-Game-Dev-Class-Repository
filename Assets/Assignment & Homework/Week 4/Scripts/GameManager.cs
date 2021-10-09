using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class GameManager : MonoBehaviour {
        public static GameManager Singleton;

        private PlayerControl player;

        [SerializeField]
        private int life = 3;
        public int Life => life;
        public int Diamond = 0;
        


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

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerStateUpdate -= OnPlayerStateUpdate;
        }
    }
}
