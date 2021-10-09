using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class GameManager : MonoBehaviour {
        public static GameManager Singleton;

        private PlayerControl player;

        public int Life = 3;
        public int Diamond = 0;
        

        private void Awake() {
            Singleton = this;
            player = PlayerControl.PlayerSingleton;
            DontDestroyOnLoad(this.gameObject);
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        }

        private void Update() {
            if (player.PlayerState != PlayerState.Dead) {
                if (player.transform.position.y <= 50) {
                    player.PlayerState = PlayerState.Dead;
                    AddLife(-1);
                }
            }
        }

        public void AddLife(int num) {
            Life += num;
            Life = Mathf.Clamp(Life,0,10);
        }
    }
}
