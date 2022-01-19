using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;

    private bool drag = false;


    // Jesli chcielibysmy przywrococ kamere na srodek to musimy zapamietaj jej pozycje
    private void Start()
    {
        ResetCamera = Camera.main.transform.position;
    }

    private void LateUpdate()
    {
        // Jesli trzymamy PPM
        if (Input.GetMouseButton(1))
        {
            // Liczymy roznice w polozeniach kamery
            Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
            if(drag == false)
            {
                drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;

        }
        // Przy trzymaniu przysku przesuwamy kamere o obliczona roznice
        if (drag)
        {
            Camera.main.transform.position = Origin - Difference;
        }
    }
}