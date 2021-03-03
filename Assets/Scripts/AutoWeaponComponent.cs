using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class AutoWeaponComponent : MonoBehaviour
    {
        [Range(0, 10)]
        public float ShootCooldown;
        [Range(0, 200)]
        public float MaxWeaponHeat;
        [Range(0, 200)]
        public float MinWeaponHeat;
        [Range(0, 200)]
        public float WeaponHeatEveryShot;
        [Range(0, 200)]
        public float WeaponHeatDrop;
        public Slider HeatingSlider;

        public GameObject BulletPrefab;
        public GameObject BulletSpawnPoint;

        private float _elapsedCooldown;
        private float _currentWeaponHeat;
        private bool _weaponOverheat;

        public bool CanShoot { get; set; }

        private void Start()
        {
            CanShoot = true;
            _currentWeaponHeat = MinWeaponHeat;
            _weaponOverheat = false;

        }

        public void CheckCanShoot()
        {
            _elapsedCooldown += Time.deltaTime;

            if (_elapsedCooldown >= ShootCooldown)
            {
                CanShoot = true;
                _elapsedCooldown = 0f;
            }
        }

        public void CheckFire()
        {
            if (Input.GetMouseButton(0) && CanShoot && !_weaponOverheat)
            {
                Shoot();
                return;
            }
            if (_currentWeaponHeat > MinWeaponHeat)
            {
                _currentWeaponHeat -= WeaponHeatDrop;
                if (_currentWeaponHeat <= MinWeaponHeat)
                {
                    _weaponOverheat = false;
                }

                HeatingSlider.value = _currentWeaponHeat/100;
            }
        }

        private void Shoot()
        {
            Instantiate(BulletPrefab, BulletSpawnPoint.transform.position, transform.rotation);
            CanShoot = false;

            _currentWeaponHeat += WeaponHeatEveryShot;
            if (_currentWeaponHeat > MaxWeaponHeat)
            {
                _weaponOverheat = true;
            }
            HeatingSlider.value = _currentWeaponHeat / 100;
        }
    }
}
