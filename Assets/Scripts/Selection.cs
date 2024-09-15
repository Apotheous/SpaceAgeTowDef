using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public Camera camera;

    void Update()
    {
        // Sadece sol fare tuþuna basýldýðýnda iþlem yapar
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Eðer ray bir objeye çarparsa
            if (Physics.Raycast(ray, out hit))
            {
                IClickable clickable = hit.transform.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.ICilkable(); // Týklanan obje IClickable interface'ini uyguluyorsa, týklama iþlemi tetiklenir
                }
            }
        }
    }
}
