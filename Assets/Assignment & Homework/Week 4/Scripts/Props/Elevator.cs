using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class Elevator : MonoBehaviour
    {
        private bool onGround = true;

        [SerializeField] private float lerp = 0.1f;
        [SerializeField] private Vector2 elevateTo;

        private Vector2 targetPos = Vector2.zero;
        private Vector2 groundPos;
        public bool OnGround
        {
            get => onGround;
            set => onGround = value;
        }

        private void Awake()
        {
            groundPos = transform.position;
        }

        private void Update()
        {
            targetPos = onGround ? groundPos : elevateTo;

            transform.position = Vector2.Lerp(transform.position, targetPos, lerp * Time.deltaTime);
        }
    }
}
