using Assets.Scripts;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Range(0, 100)]
    public int HealthPoints;
    public GameObject BaseObject;
    public Team Team;

    public abstract void UpdateHealthValue(int hitpoints);

}

