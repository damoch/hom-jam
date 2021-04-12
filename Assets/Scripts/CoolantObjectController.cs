using UnityEngine;

namespace Assets.Scripts
{
    public class CoolantObjectController : MonoBehaviour
    {
        public int CoolantValue;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                var weapon = other.gameObject.GetComponent<AutoWeaponComponent>();
                weapon.CoolWeaponDownBy(CoolantValue);
                Destroy(gameObject);
            }
        }
    }
}
