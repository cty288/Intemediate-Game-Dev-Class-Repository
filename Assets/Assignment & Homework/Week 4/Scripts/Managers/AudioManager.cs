using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class AudioManager : MonoBehaviour {
        [SerializeField] private AudioSource bgm;
        [SerializeField] private AudioSource ambientSound;

        [SerializeField] private AudioClip menuClip;
        [SerializeField] private AudioClip gameBGM;
        [SerializeField] private AudioClip levelPassClip;
        [SerializeField] private AudioClip playerDieClip;

        public static AudioManager Singleton;

        private PlayerControl player;

        private void Awake() {
            Singleton = this;
        }

        private void Start() {
            SimpleEventSystem.OnPlayerStateUpdate += OnPlayerStateChange;
            player = GameManager.Singleton.GetPlayer();

            InitialBGM();
        }

        private void InitialBGM() {
            switch (player.PlayerState) {
                case PlayerState.Menu:
                    PlayBGM(menuClip);
                    break;
                default:
                    PlayBGM(gameBGM);
                    break;
            }
        }

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerStateUpdate -= OnPlayerStateChange;
        }

        private void OnPlayerStateChange(PlayerState oldState, PlayerState newState) {
            switch (newState) {
                case PlayerState.Dead:
                    PlayBGM(playerDieClip,false);
                    break;
                case PlayerState.Menu:
                    PlayBGM(menuClip);
                    break;
                case PlayerState.GamePass:
                    PlayBGM(levelPassClip,false);
                    break;
                case PlayerState.End:
                    PlayBGM(levelPassClip, false);
                    break;
            }

            switch (oldState) {
                case PlayerState.Menu:
                    PlayBGM(gameBGM);
                    break;
                case PlayerState.Dead:
                    PlayBGM(gameBGM);
                    break;
            }
            
        }

        public void PlayBGM(AudioClip clip, bool loop=true) {
            bgm.Stop();
            bgm.clip = clip;
            bgm.loop = loop;
            bgm.Play();
        }

        public void PlayObjectSounds(AudioClip clip, float relativeVolume) {
            ambientSound.PlayOneShot(clip,relativeVolume);
        }
    }
}
