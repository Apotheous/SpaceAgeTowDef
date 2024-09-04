using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAnimController : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rb;
    public Enemy unit;
    private string currentState;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        unit = GetComponent<Enemy>();
    }
    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
        currentState = newState;
    }

    void Update()
    {
        ChangeAnimationState(currentState);
    }
}
