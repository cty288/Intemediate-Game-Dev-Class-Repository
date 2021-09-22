using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextChanger : MonoBehaviour
{
    public enum Stage {
        One,
        Two,
        Three
    }
    public TMP_Text text;
    public string newText="new text";

    public Stage stage;

    private int counter;
    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    void Start() {
        text.text = newText;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (stage == Stage.One) {
                text.text = "Stage one";
            }
            else if (stage == Stage.Two) {
                text.text = "Stage two";
            }
            else {
                text.text = "Stage three";
            }
            
        }
    }
}
