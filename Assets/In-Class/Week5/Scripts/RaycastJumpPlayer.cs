using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Week4;

public class RaycastJumpPlayer : MonoBehaviour {
    private Rigidbody2D mRig;

    [SerializeField] private float jumpStrength = 5f;
    [SerializeField] private float movementSpeed = 5f;

    private float moveX;
    private bool isGrounded;
    private bool canJump;

    public float raycastDistance = 1f;

    private void Awake() {
        mRig = GetComponent<Rigidbody2D>();
    }

    private void Start() {
    }

    private void Update() {
        PlayerControls();
    }

    private void PlayerControls() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            canJump = true;
        }

        moveX = Input.GetAxis("Horiztontal");
        CheckGrounded();
    }

    private void CheckGrounded() {
        
    }
}
