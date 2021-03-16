using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AutoWeaponComponent : MonoBehaviour
    {
        [Range(0, 10)]
        public float ShootCooldown;
        [Range(0, 10)]
        public float ShootCooldownOverheat;
        [Range(0, 200)]
        public float MaxWeaponHeat;
        [Range(0, 200)]
        public float MinWeaponHeat;
        [Range(0, 200)]
        public float WeaponHeatEveryShot;
        [Range(0, 200)]
        public float WeaponHeatDrop;
        [Range(0, 200)]
        public float WeaponHeatDangerBound;
        public Slider HeatingSlider;

        public GameObject BulletPrefab;
        public GameObject BulletSpawnPoint;
        public AudioClip ShootAudioClip;

        private float _elapsedCooldown;
        private float _currentWeaponHeat;
        private AudioSource _audioSource;

        public bool CanShoot { get; set; }
        public bool WeaponOverheat { get; set; }

        private void Start()
        {
            CanShoot = true;
            _currentWeaponHeat = MinWeaponHeat;
            WeaponOverheat = false;
            _audioSource = GetComponent<AudioSource>();

        }

        public void CheckCanShoot()
        {
            _elapsedCooldown += Time.deltaTime;
            var currentCooldown = _currentWeaponHeat > WeaponHeatDangerBound ? ShootCooldownOverheat : ShootCooldown;
            if (_elapsedCooldown >= currentCooldown)
            {
                CanShoot = true;
                _elapsedCooldown = 0f;
            }
        }

        public void CheckFire(bool triggerFired)
        {
            if (triggerFired && CanShoot && !WeaponOverheat)
            {
                Shoot();
                return;
            }
            if (_currentWeaponHeat > MinWeaponHeat)
            {
                _currentWeaponHeat -= WeaponHeatDrop;
                if (_currentWeaponHeat <= MinWeaponHeat)
                {
                    WeaponOverheat = false;
                }
                UpdateHeatingSlider();
            }
        }

        private void UpdateHeatingSlider()
        {
            if (HeatingSlider)
            {
                HeatingSlider.value = _currentWeaponHeat / 100;
            }
        }

        private void Shoot()
        {
            Instantiate(BulletPrefab, BulletSpawnPoint.transform.position, transform.rotation);
            _audioSource.PlayOneShot(ShootAudioClip);
            CanShoot = false;

            _currentWeaponHeat += WeaponHeatEveryShot;
            if (_currentWeaponHeat > MaxWeaponHeat)
            {
                WeaponOverheat = true;
            }
            UpdateHeatingSlider();
        }

        internal void ResetWeapon()
        {
            _currentWeaponHeat = MinWeaponHeat;
            WeaponOverheat = false;
            UpdateHeatingSlider();
        }
    }
}
