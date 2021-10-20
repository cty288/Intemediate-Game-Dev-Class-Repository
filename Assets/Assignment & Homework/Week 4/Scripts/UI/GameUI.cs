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
        //[SerializeField] private SelfMadeButton selfMadeRestartButton;
       // [SerializeField] private SelfMadeButton selfMadeNextLevelButton;

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

            SimpleEventSystem.OnGameEnds += OnGameEnds;
            SimpleEventSystem.OnPlayerRespawn += OnPlayerRespawn;
        }

        private void OnPlayerRespawn() {
            dieBG.SetActive(false);
        }

        private void OnGameEnds(int heartAdded, int levelNum) {
            GameObject gamePassBG = GameObject.Find("LevelPassBG");
            gamePassBG.gameObject.SetActive(true);
            gamePassBG.GetComponent<Animation>().Play();
            gamePassBG.transform.Find("InfoText").GetComponent<Text>().text = $"Level {levelNum} Passed!";
            gamePassBG.transform.Find("AddHeartText").GetComponent<Text>().text = $"+{heartAdded}";

            gamePassBG.transform.Find("RestartButton").GetComponent<Button>().onClick.AddListener(() => {
                GameManager.Singleton.GoToNextLevel();
            });

           // if (selfMadeNextLevelButton) {
              //  selfMadeNextLevelButton.Activate();
            //}
           

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
            SimpleEventSystem.OnPlayerRespawn -= OnPlayerRespawn;
        }

        void OnLifeChanged(int oldlife, int newLife) {
           UpdateHeartUI(newLife);
           if (newLife < oldlife) {
               StartCoroutine(ShowDieBG(newLife));
           }
        }
        IEnumerator ShowDieBG(int newLife) {
            yield return new WaitForSeconds(1);
            dieBG.SetActive(true);
            
            //selfMadeRestartButton.Activate();
           

            if (newLife > 0)
            {
                dieBG.transform.Find("InfoText").gameObject.SetActive(true);
                dieBG.transform.Find("LoseAllLifeText").gameObject.SetActive(false);

                restartButton.onClick.AddListener(() => {
                    GameManager.Singleton.Respawn();
                    dieBG.SetActive(false);
                });

            }
            else
            {
                dieBG.transform.Find("LoseAllLifeText").gameObject.SetActive(true);
                dieBG.transform.Find("InfoText").gameObject.SetActive(false);
                //TODO
                restartButton.onClick.AddListener(() => {
                    
                    Debug.Log("Lose all life");
                    GameManager.Singleton.ResetToFirstLevel();
                    GameManager.Singleton.AddLife(5);
                });

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
