using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable 
{
    Rigidbody rb { get;set;}

    bool IsMovingForward {  get; set; }

    void MoveEnemyTowardsTarget(GameObject enemyObject, float MoveSpeed);

    void CheckForForwardOrBackFacing(Vector3 velocity);
}
