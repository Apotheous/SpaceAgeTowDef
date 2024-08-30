using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : MonoBehaviour
{

    public float moveSpeed = 2f;  // Hareket hýzý

    public float startHealth;
    private float health;

    [Header("Unity Stuff")]
    public Image healthBar;
    private void Start()
    {
        health = startHealth;
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / startHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    void Update()
    {
        // Objenin ileri doðru (z ekseni boyunca) hareket etmesini saðla
        transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
