using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretModel
{
    public string Name { get; set; }
    public LevelProp Level { get; set; }
    public float ShootingFrequency { get; set; }
    public float Damage { get; set; }
    public float RangeOfVision { get; set; }
    public float FiringRange { get; set; }
    public float Health { get; set; }
    public int CurrentXP { get; set; } = 0;
    public int NextLevelXP { get; set; }

    public TurretModel(string name, LevelProp level, int currentXP)
    {
        this.Name = name;
        this.Level = level;
        this.CurrentXP = currentXP;
        SetLevelProperties(level);
    }

    private void SetLevelProperties(LevelProp level)
    {
        switch (level)
        {
            case LevelProp.LEVEL_ONE:
                ShootingFrequency = 1.0f;
                Damage = 10.0f;
                RangeOfVision = 5.0f;
                FiringRange = 7.0f;
                Health = 100.0f;
                NextLevelXP = 1000;
                break;

            case LevelProp.LEVEL_TWO:
                ShootingFrequency = 1.5f;
                Damage = 20.0f;
                RangeOfVision = 6.0f;
                FiringRange = 8.0f;
                Health = 150.0f;
                NextLevelXP = 3000;
                break;

            case LevelProp.LEVEL_THREE:
                ShootingFrequency = 2.0f;
                Damage = 30.0f;
                RangeOfVision = 7.0f;
                FiringRange = 10.0f;
                Health = 200.0f;
                NextLevelXP = 5000;
                break;
        }
    }
}



public enum LevelProp
{
    LEVEL_ONE, LEVEL_TWO, LEVEL_THREE
}