using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class TurretModel : MonoBehaviour
{

    public string Name;
    public LevelProp Level;
    public GunnerType GunnerType;
    public float maxHealth;
    public float currentHealth;
    public int CurrentXP = 0;
    public int NextLevelXP;
    public int Cost;

    [System.Serializable]
    public class WeaponClass
    {
        public List<Transform> Barrels = new List<Transform>();
        public int ShotForce;

        public float FiringRange;
        public float Dist;

        public bool OnTarget;

        public float Timer;
        public float FireRate;
        public float BarrelTimer;
        public float BarrelTimerRate;
        public float BarrelTimerLine;
    }

    public WeaponClass weaponClass;

    [System.Serializable]
    public class BulletClass
    {
        public BasicPool pool;
        public GameObject myBullet;
        public Rigidbody myBulletRb;

    }
    public BulletClass bulletClass;

    [System.Serializable]
    public class RotationClass
    {
        public Transform AngleX;    // X ekseninde dönen parça
        public Transform AngleY;    // Y ekseninde dönen parça
        public float RotationSpeed; // Dönme hýzý

        public float NotDeep;
        public float NotDeepT;
        public float BarrelHeightAllowance;
    }
    public RotationClass rotationClass;

    [System.Serializable]
    public class LaserClass
    {
        [Header("Use Laser")]
        public bool UseLaser = false;

        public int DamageOverTime = 30;
        public float ShowAmount = .5f;

        public LineRenderer lineRenderer;
        public ParticleSystem impactEffect;
        public Light impactLight;

    }
    public LaserClass laserClass;

    //public TurretModel(string name, LevelProp level, GunnerType gunnerType, int currentXP, bool useLaser)
    //{
    //    this.Name = name;
    //    this.Level = level;
    //    this.GunnerType = gunnerType;
    //    this.CurrentXP = currentXP;
    //    this.UseLaser = useLaser;
    //    SetLevelProperties(level, gunnerType);
    //}

    private void OnValidate()
    {
        UpdateTurretModel();
    }

    public void UpdateTurretModel()
    {
        SetLevelProperties(Level, GunnerType);
    }

    public void SetLevelProperties(LevelProp level, GunnerType fireType)
    {
        switch (fireType)
        {
            case GunnerType.LASER:
                switch (level)
                {
                    case LevelProp.LEVEL_ONE:
                        weaponClass.FiringRange = 70.0f;
                        maxHealth = 100.0f;
                        currentHealth = maxHealth;
                        weaponClass.ShotForce = 100;
                        weaponClass.FireRate = 1.0f;
                        weaponClass.BarrelTimer = 0;
                        weaponClass.BarrelTimerRate = 0.5f;
                        weaponClass.BarrelTimerLine = 0.1f;
                        rotationClass.RotationSpeed = 5.0f;
                        rotationClass.BarrelHeightAllowance = 1.0f;
                        laserClass.DamageOverTime = 30;
                        laserClass.ShowAmount = 0.5f;
                        NextLevelXP = 1000;
                        Cost = 500;
                        break;
                    case LevelProp.LEVEL_TWO:
                        weaponClass.FiringRange = 7.0f;
                        maxHealth = 100.0f;
                        currentHealth = maxHealth;
                        weaponClass.ShotForce = 100;
                        weaponClass.FireRate = 1.0f;
                        weaponClass.BarrelTimer = 0;
                        weaponClass.BarrelTimerRate = 0.4f;
                        weaponClass.BarrelTimerLine = 0.1f;
                        rotationClass.RotationSpeed = 8.0f;
                        rotationClass.BarrelHeightAllowance = 1.0f;
                        laserClass.DamageOverTime = 30;
                        laserClass.ShowAmount = 0.5f;
                        NextLevelXP = 1000;
                        Cost = 500;
                        break;
                    case LevelProp.LEVEL_THREE:
                        weaponClass.FiringRange = 7.0f;
                        maxHealth = 100.0f;
                        currentHealth = maxHealth;
                        weaponClass.ShotForce = 100;
                        weaponClass.FireRate = 1.0f;
                        weaponClass.BarrelTimer = 0;
                        weaponClass.BarrelTimerRate = 0.2f;
                        weaponClass.BarrelTimerLine = 0.1f;
                        rotationClass.RotationSpeed = 10.0f;
                        rotationClass.BarrelHeightAllowance = 1.0f;
                        laserClass.DamageOverTime = 30;
                        laserClass.ShowAmount = 0.5f;
                        NextLevelXP = 1000;
                        Cost = 500;
                        break;
                }
                break;

            case GunnerType.GUNNER:
                switch (level)
                {
                    case LevelProp.LEVEL_ONE:
                        weaponClass.FiringRange = 30.0f;
                        maxHealth = 100.0f;
                        currentHealth = maxHealth;
                        weaponClass.ShotForce = 100;
                        weaponClass.FireRate = 0.2f;
                        weaponClass.BarrelTimerRate = 0.5f;
                        weaponClass.BarrelTimerLine = 3f;
                        rotationClass.RotationSpeed = 5.0f;
                        rotationClass.BarrelHeightAllowance = 1.0f;
                        laserClass.DamageOverTime = 30;
                        laserClass.ShowAmount = 0.5f;
                        NextLevelXP = 1000;
                        Cost = 500;
                        break;
                    case LevelProp.LEVEL_TWO:
                        weaponClass.FiringRange = 40.0f;
                        maxHealth = 100.0f;
                        currentHealth = maxHealth;
                        weaponClass.ShotForce = 300;
                        weaponClass.FireRate = 0.2f;
                        weaponClass.BarrelTimerRate = 2f;
                        weaponClass.BarrelTimerLine = 0.1f;
                        rotationClass.RotationSpeed = 7.0f;
                        rotationClass.BarrelHeightAllowance = 1.0f;
                        laserClass.DamageOverTime = 30;
                        laserClass.ShowAmount = 0.5f;
                        NextLevelXP = 1000;
                        Cost = 500;
                        break;
                    case LevelProp.LEVEL_THREE:
                        weaponClass.FiringRange = 60.0f;
                        maxHealth = 100.0f;
                        currentHealth = maxHealth;
                        weaponClass.ShotForce = 400;
                        weaponClass.FireRate = 0.2f;
                        weaponClass.BarrelTimerRate = 1f;
                        weaponClass.BarrelTimerLine = 2f;
                        rotationClass.RotationSpeed = 10.0f;
                        rotationClass.BarrelHeightAllowance = 1.0f;
                        laserClass.DamageOverTime = 30;
                        laserClass.ShowAmount = 0.5f;
                        NextLevelXP = 1000;
                        Cost = 500;
                        break;
                }
                break;
        }
    }
}

public enum LevelProp
{
    LEVEL_ONE, LEVEL_TWO, LEVEL_THREE
}

public enum GunnerType
{
    LASER, GUNNER
}




