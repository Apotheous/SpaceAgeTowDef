using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{

    public type Type;

    public string enemyGroupTag;

    public float speed = 70f;

    public float my_Damage;

    public float explosionRadius;

}
public enum type
{
    BULLET,
    LASER,
    CANNONBALL,
    EXPLOSÝVE
}
