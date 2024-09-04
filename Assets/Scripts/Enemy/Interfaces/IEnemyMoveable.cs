using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable 
{
    Rigidbody rb { get;set;}

    bool IsMovingForward {  get; set; }

    void MoveEnemy(GameObject enemyObject, float MoveSpeed);

    void CheckForForwardOrBackFacing(Vector3 velocity);
}
