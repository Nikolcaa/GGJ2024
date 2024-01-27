using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float sensitivity = 100.0f;
    public Transform target;
    public float clampAngle = 80.0f;

    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;
    void Start()
    {
        Cursor.visible = false;

        Vector3 rotation = target.localRotation.eulerAngles;
        _rotationX = rotation.x;
        _rotationY = rotation.y;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationY += mouseX * sensitivity * Time.deltaTime;
        _rotationX -= mouseY * sensitivity * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(_rotationX, _rotationY, 0.0f);
        target.rotation = localRotation;
    }
}
