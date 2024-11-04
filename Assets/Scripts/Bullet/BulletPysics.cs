using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPysics : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Merminin h�z�n� hesapla (m/s)
        float speed = rb.linearVelocity.magnitude;

        // H�z� km/h cinsine �evir
        float speedKmH = speed * 3.6f;

        // H�z� debug olarak yazd�r
        Debug.Log("Current Bullet Speed: " + speedKmH + " km/h");
    }
}
