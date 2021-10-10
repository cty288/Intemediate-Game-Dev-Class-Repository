using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Week4
{
    public class GameUI : MonoBehaviour
    {
        private GridLayoutGroup gridLayoutGroup;

        private Text diamondCountText;

        private Text keyCountText;

        [SerializeField] 
        private GameObject dieBG;

        [SerializeField] private Button restartButton;

        private void Awake()
        {
            gridLayoutGroup = transform.Find("HeartDisplay").GetComponent<GridLayoutGroup>();
            diamondCountText = transform.Find("DiamondDisplay/NumText").GetComponent<Text>();
            keyCountText = transform.Find("KeyDisplay/NumText").GetComponent<Text>();
        }

        private void Start()
        {
            SimpleEventSystem.OnLifeChange += OnLifeChanged;
            OnLifeChanged(GameManager.Singleton.Life,GameManager.Singleton.Life);

            SimpleEventSystem.OnDiamondChange += OnDiamondChanged;
            OnDiamondChanged(GameManager.Singleton.Diamond,GameManager.Singleton.Diamond);

            SimpleEventSystem.OnKeyChange += OnKeyChanged;
            OnKeyChanged(GameManager.Singleton.Key, GameManager.Singleton.Key);
        }

        private void OnKeyChanged(int old, int cur) {
            keyCountText.text = "X " + cur;
        }

        private void OnDiamondChanged(int old, int newNum) {
            diamondCountText.text = "X " + newNum;
        }

        private void OnDestroy()
        {
            SimpleEventSystem.OnLifeChange -= OnLifeChanged;
            SimpleEventSystem.OnDiamondChange -= OnDiamondChanged;
            SimpleEventSystem.OnKeyChange -= OnKeyChanged;
        }

        void OnLifeChanged(int oldlife, int newLife) {
           UpdateHeartUI(newLife);
           if (newLife < oldlife) {
               dieBG.SetActive(true);
               
               restartButton.onClick.RemoveAllListeners();

               if (newLife > 0) {
                   restartButton.onClick.AddListener(() => {
                       GameManager.Singleton.RestartCurrentLevel();
                   });
                }
               else {
                   //TODO
                   restartButton.onClick.AddListener(() => {
                       Debug.Log("Lose all life");
                   });
               }
           }
        }

        void UpdateHeartUI(int life) {
            for (int i = 0; i < 10; i++)
            {
                gridLayoutGroup.transform.GetChild(i).gameObject.SetActive(i < life);
            }
        }
    }
}
