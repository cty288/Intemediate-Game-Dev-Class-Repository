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

        [SerializeField] 
        private GameObject dieBG;

        [SerializeField] private Button restartButton;

        private void Awake()
        {
            gridLayoutGroup = transform.Find("HeartDisplay").GetComponent<GridLayoutGroup>();
        }

        private void Start()
        {
            SimpleEventSystem.OnLifeChange += OnLifeChanged;
            OnLifeChanged(GameManager.Singleton.Life,GameManager.Singleton.Life);
        }


        private void OnDestroy()
        {
            SimpleEventSystem.OnLifeChange -= OnLifeChanged;
        }

        void OnLifeChanged(int oldlife, int newLife) {
           UpdateHeartUI(newLife);
           if (newLife < oldlife) {
               dieBG.SetActive(true);
               
               restartButton.onClick.RemoveAllListeners();

               if (newLife > 0) {
                   restartButton.onClick.AddListener(() => {
                       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
