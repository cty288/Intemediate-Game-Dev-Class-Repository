using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    private TMP_Text timeText;
   
    private TMP_Dropdown speedDropdown;
    private TMP_Text stageText;

    private void Awake() {
        timeText = transform.Find("TimeText").GetComponent<TMP_Text>();
       
        speedDropdown = transform.Find("SpeedDropdown").GetComponent<TMP_Dropdown>();
        stageText = transform.Find("StageText").GetComponent<TMP_Text>();
    }

    void Update() {
        timeText.text = $"Time: {Mathf.RoundToInt(Time.time)} sec";

        speedDropdown.onValueChanged.AddListener(OnDropdownValueChange);
        stageText.text = $"Stage: {Week3GameManager.Singleton.Stage}";
    }

    private void OnDropdownValueChange(int index) {
        Week3GameManager.Singleton.TimeSpeed = index + 1;
    }

    private void OnRestart() {
        SceneManager.LoadScene("Week3Scene");
    }
}
