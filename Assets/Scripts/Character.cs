using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [Range(0, 100)]
    public int HealthPoints;

    public abstract void UpdateHealthValue(int hitpoints);

}

