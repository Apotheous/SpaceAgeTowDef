using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public Camera camera;

    void Update()
    {
        // Sadece sol fare tu�una bas�ld���nda i�lem yapar
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // E�er ray bir objeye �arparsa
            if (Physics.Raycast(ray, out hit))
            {
                IClickable clickable = hit.transform.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.ICilkable(); // T�klanan obje IClickable interface'ini uyguluyorsa, t�klama i�lemi tetiklenir
                }
            }
        }
    }
}
