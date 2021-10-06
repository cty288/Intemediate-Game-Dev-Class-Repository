using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    private Transform player;
    private Camera camera;
    [SerializeField]
    private float lerpSpeed=20;

    private bool dead = true;
    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GetComponent<Camera>();
    }

    private void Update() {


        float targetX = transform.position.x;

        targetX = Mathf.Lerp(targetX, player.position.x,lerpSpeed*Time.deltaTime);

        targetX = Mathf.Clamp(targetX, GameManager.Singleton.cameraMinX, GameManager.Singleton.cameraMaxX);

        transform.position = new Vector3(targetX, transform.position.y,transform.position.z);

    }
}
