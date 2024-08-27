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
        // Merminin hýzýný hesapla (m/s)
        float speed = rb.velocity.magnitude;

        // Hýzý km/h cinsine çevir
        float speedKmH = speed * 3.6f;

        // Hýzý debug olarak yazdýr
        Debug.Log("Current Bullet Speed: " + speedKmH + " km/h");
    }
}
