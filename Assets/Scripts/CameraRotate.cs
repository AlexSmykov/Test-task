using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private float rotateSens;


    private float yRotation;
    private float xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * rotateSens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * rotateSens;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}