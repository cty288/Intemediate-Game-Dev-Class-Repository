using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Week4
{
    public class ShootBullet : MonoBehaviour {
        private Transform shootPosition;
        [SerializeField]
        private GameObject ShootingBulletPrefab;
        [SerializeField] 
        private float shootSpeed = 5f;
        [SerializeField] 
        private Vector2 shootAngle = new Vector2(1, 1);
        [SerializeField] 
        private float shootInterval = 0.2f;

        [SerializeField] private AudioClip shootSound;
        private float timer;

        
        private PlayerControl player;
        private void Awake() {
            shootPosition = transform.Find("FirePos");
            player = GetComponent<PlayerControl>();
        }

        private void Update() {
            timer += Time.deltaTime;
            if (Input.GetMouseButton(0) && player.PlayerState != PlayerState.Talking &&
                player.PlayerState != PlayerState.Dead && player.PlayerState != PlayerState.End) {

                if (GameManager.Singleton.CanShoot()) {
                    if (timer >= Mathf.Max(0.05f, shootInterval- (GameManager.Singleton.Bullet-1)*0.05f)) {
                        timer = 0;
                        Shoot();
                    }
                   
                }
            }
        }

        private void Shoot() {
            AudioManager.Singleton.PlayObjectSounds(shootSound,0.8f);
            int facing = player.transform.localScale.x > 0 ? 1 : -1; 
            GameObject bullet = Instantiate(ShootingBulletPrefab, shootPosition.position,
                Quaternion.identity);
            Vector2 shootAngleFixed = new Vector2(shootAngle.x * facing, shootAngle.y);

            bullet.GetComponent<ShootableBullet>().Shoot(shootSpeed + Mathf.Abs(player.Speed) /2,shootAngleFixed);
        }
    }
}
