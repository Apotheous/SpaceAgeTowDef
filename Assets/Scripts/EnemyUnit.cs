using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{

    public float moveSpeed = 2f;  // Hareket hýzý

    void Update()
    {
        // Objenin ileri doðru (z ekseni boyunca) hareket etmesini saðla
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
