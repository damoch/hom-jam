﻿using UnityEngine;

namespace Assets.Scripts
{
    public class HomingMissle : MonoBehaviour
    {
        public GameObject Target;
        public float Speed;
        public int Hitpoints;

        private void Start()
        {
            if(Target == null)
            {
                Target = FindObjectOfType<PlayerControler>().gameObject;
            }
        }

        private void Update()
        {
            if (Target == null)
            {
                Destroy(gameObject);
            }
            FaceEnemy();
            Vector3 dir = Vector2.up;
            dir *= Speed * Time.fixedDeltaTime;
            gameObject.transform.Translate(dir);
        }

        private void FaceEnemy()
        {
            var dir = Target.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Bullet")
            {
                Destroy(gameObject);
            }
            var enemy = other.gameObject;

            //Instantiate(BulletHit, transform.position, transform.rotation);
            if(enemy == Target)
            {
                enemy.GetComponent<Character>().UpdateHealthValue(-Hitpoints);
                Destroy(gameObject);
            }
        }
    }
}
