using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class ResetWhenRespawn : MonoBehaviour {
        private Vector2 originalPosition;
        private Quaternion originalRotation;
        private Vector3 originalScale;
        private Rigidbody2D mRigidbody;

        private void Awake() {
            mRigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start() {

            originalPosition = transform.position;
            originalRotation = transform.rotation;
            originalScale = transform.localScale;
            SimpleEventSystem.OnPlayerRespawn += OnPlayerRespawn;
        }

        private void OnPlayerRespawn() {
            if (mRigidbody) {
                mRigidbody.simulated = false;
                mRigidbody.velocity = Vector2.zero;
            }
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            transform.localScale = originalScale;

            if (mRigidbody)
            {
                mRigidbody.simulated = true;
            }
        }

        private void OnDestroy() {
            SimpleEventSystem.OnPlayerRespawn -= OnPlayerRespawn;
        }
    }
}
