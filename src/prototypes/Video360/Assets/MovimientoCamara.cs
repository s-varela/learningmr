using UnityEngine;
using System.Collections;

public class MovimientoCamara : MonoBehaviour
{
    public GameObject Camara;
    public float YSense = 15;
    public float XSense = 15;
    public Vector2 RangoDeApertura = new Vector2(-60, 60);


    float rotationY = 0F;
    float rotationX = 0F;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }


        rotationX = transform.eulerAngles.y + Input.GetAxis("Mouse X") * Time.deltaTime * XSense;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotationX, transform.eulerAngles.z);


        rotationY += Input.GetAxis("Mouse Y") * YSense * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, RangoDeApertura.x, RangoDeApertura.y);

        Camara.transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);

    }

}
