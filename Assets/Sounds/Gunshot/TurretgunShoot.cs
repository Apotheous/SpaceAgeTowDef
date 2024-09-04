using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretgunShoot : MonoBehaviour
{
    public AudioSource gunShotSound;

    private void Start()
    {
        gunShotSound = gameObject.GetComponent<AudioSource>();
    }
    public void GunShootingSound()
    {
        gunShotSound.Play();
    }
}
