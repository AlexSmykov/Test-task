using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private float horizontalDelta;
    private float verticalDelta;

    private Vector3 moveDirection;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        GetInput();
        SpeedControl();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        horizontalDelta = Input.GetAxisRaw("Horizontal");
        verticalDelta = Input.GetAxisRaw("Vertical");
    }

    private void Move()
    {
        moveDirection = transform.forward * verticalDelta + transform.right * horizontalDelta;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        rb.velocity *= 0.995f;
    }
}