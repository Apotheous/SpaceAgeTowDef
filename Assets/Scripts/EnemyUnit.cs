using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{

    public float moveSpeed = 2f;  // Hareket h�z�

    void Update()
    {
        // Objenin ileri do�ru (z ekseni boyunca) hareket etmesini sa�la
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
