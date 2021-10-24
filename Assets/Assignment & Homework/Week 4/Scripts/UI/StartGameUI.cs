using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Week4
{
    public class StartGameUI : MonoBehaviour {
        private Button startButton;
        private Animation animation;

        private void Awake() {
            startButton = GetComponentInChildren<Button>();
            animation = GetComponent<Animation>();
        }

        private void Start() {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnStartButtonClicked() {
            GameManager.Singleton.GetPlayer().PlayerState = PlayerState.Idle;
            animation.Play();
            SimpleEventSystem.OnGameStart?.Invoke();
        }
    }
}
