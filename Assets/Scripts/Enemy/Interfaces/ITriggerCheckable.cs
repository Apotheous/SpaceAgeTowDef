using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerCheckable 
{
    bool IsAggroed { get; set; }
    bool IsWithinStrikeingDistance { get; set; }

    void SetAggroStatus(bool aggroStatus);
    void SetStrikingDistanceBool(bool IsWithinStrikeingDistance);
}
