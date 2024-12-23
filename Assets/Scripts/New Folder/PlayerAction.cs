using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private PlayerGunSelector GunSelector;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && GunSelector.ActiveGun != null) GunSelector.ActiveGun.Shoot();
    }
}
