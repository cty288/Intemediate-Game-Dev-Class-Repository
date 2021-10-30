using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Week4
{
    public class GameUI : MonoBehaviour
    {
        private GridLayoutGroup heartLayoutGroup;
        private GridLayoutGroup itemLayoutGroup;

        private Text diamondCountText;

        private Text keyCountText;

        [SerializeField] 
        private GameObject dieBG;

       

        [SerializeField] private Button restartButton;
        //[SerializeField] private SelfMadeButton selfMadeRestartButton;
       // [SerializeField] private SelfMadeButton selfMadeNextLevelButton;

       [SerializeField] 
       private List<Sprite> itemSprites= new List<Sprite>();
        private void Awake()
        {
            heartLayoutGroup = transform.Find("HeartDisplay").GetComponent<GridLayoutGroup>();
            itemLayoutGroup = transform.Find("ItemsDisplay").GetComponent<GridLayoutGroup>();
            diamondCountText = transform.Find("DiamondDisplay/NumText").GetComponent<Text>();
            keyCountText = transform.Find("KeyDisplay/NumText").GetComponent<Text>();
        }

        private void Start()
        {
            UpdateItemPickedUI();
            SimpleEventSystem.OnLifeChange += OnLifeChanged;
            OnLifeChanged(GameManager.Singleton.Life,GameManager.Singleton.Life);

            SimpleEventSystem.OnDiamondChange += OnDiamondChanged;
            OnDiamondChanged(GameManager.Singleton.Diamond,GameManager.Singleton.Diamond);

            SimpleEventSystem.OnKeyChange += OnKeyChanged;
            OnKeyChanged(GameManager.Singleton.Key, GameManager.Singleton.Key);

            SimpleEventSystem.OnGameEnds += OnGameEnds;
            SimpleEventSystem.OnPlayerRespawn += OnPlayerRespawn;
            SimpleEventSystem.OnPlayerPickItem += OnPlayerPickItem;

            SimpleEventSystem.OnGameStart += OnGameStart;
            SimpleEventSystem.OnEntireGameEnds += OnEntireGameEnds;
        }

        private void OnGameStart() {
            StartCoroutine(FromMenuToGame());
        }

        private IEnumerator FromMenuToGame() {
            yield return new WaitForSeconds(1f);
            heartLayoutGroup.gameObject.SetActive(true);
            diamondCountText.transform.parent.gameObject.SetActive(true);
            keyCountText.transform.parent.gameObject.SetActive(true);
            transform.Find("Backpack").gameObject.SetActive(true);
            itemLayoutGroup.gameObject.SetActive(true);
        }

        private void OnPlayerPickItem(ItemType item) {
            UpdateItemPickedUI();
        }

        private void UpdateItemPickedUI() {
            if (itemLayoutGroup) {
                List<ItemType> itemTypes = GameManager.Singleton.ItemsPicked;

                for (int i = 0; i < 10; i++) {
                    GameObject itemObj = itemLayoutGroup.transform.GetChild(i).gameObject;
                    if (i < itemTypes.Count) {
                        itemObj.SetActive(true);
                        itemObj.GetComponent<Image>().sprite = itemSprites[(int) itemTypes[i]];
                    }
                    else {
                        itemObj.SetActive(false);
                    }
                    
                }
            }
           
        }

        private void OnPlayerRespawn() {
            dieBG.SetActive(false);
            UpdateItemPickedUI();
        }

        private void OnGameEnds(int heartAdded, int levelNum) {
            GameObject gamePassBG = transform.Find("LevelPassBG").gameObject;
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
            SimpleEventSystem.OnPlayerPickItem -= OnPlayerPickItem;
            SimpleEventSystem.OnGameEnds -= OnGameEnds;
            SimpleEventSystem.OnGameStart -= OnGameStart;
            SimpleEventSystem.OnEntireGameEnds -= OnEntireGameEnds;
        }

        private void OnEntireGameEnds() {
            this.gameObject.SetActive(false);
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
                    
                });

            }
        }
        void UpdateHeartUI(int life) {
            for (int i = 0; i < 20; i++)
            {
                heartLayoutGroup.transform.GetChild(i).gameObject.SetActive(i < life);
            }
        }
    }
}
