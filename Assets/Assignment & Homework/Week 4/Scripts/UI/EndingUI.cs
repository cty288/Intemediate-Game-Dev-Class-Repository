using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Week4
{
    public class EndingUI : MonoBehaviour {
        private Transform child;

        private Text diamondNumText;
        private Text respawnTimeText;
        private Text enemiesKilledText;

        private Button menuButton;
        private void Awake() {
            child = transform.Find("Child");
            Transform panel = child.Find("Panel");
            diamondNumText = panel.Find("DiamondNumer").GetComponent<Text>();
            respawnTimeText = panel.Find("RespawnNumber").GetComponent<Text>();
            enemiesKilledText = panel.Find("EnemyKilledNumber").GetComponent<Text>();
            menuButton = panel.Find("RestartButton").GetComponent<Button>();
        }

        private void Start() {
            SimpleEventSystem.OnEntireGameEnds += OnGameEnds;
            menuButton.onClick.AddListener(GameManager.Singleton.ResetToFirstLevel);
        }

        private void OnDestroy() {
            SimpleEventSystem.OnEntireGameEnds -= OnGameEnds;
            menuButton.onClick.RemoveListener(GameManager.Singleton.ResetToFirstLevel);
        }

        private void OnGameEnds() {
            child.gameObject.SetActive(true);
            SetEndingText();
        }

        private void SetEndingText() {
            diamondNumText.text = GameManager.Singleton.TotalDiamondGet.ToString();
            respawnTimeText.text = GameManager.Singleton.TotalRespawnTime.ToString();
            enemiesKilledText.text = GameManager.Singleton.TotalEnemiesKilled.ToString();
        }
    }
}
