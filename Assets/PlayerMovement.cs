using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float MovementSpeed;
    public float AttackSpeed;
}

public class PlayerMovement : MonoBehaviour
{
    [Header("DATA")]
    public PlayerData PlayerData;

    [Header("Some properties")]
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform directionTransform;
    public GroundCheck groundCheck;

    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (groundCheck.isGrounded)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
            direction = directionTransform.TransformDirection(direction);
            direction.y = 0;
            direction *= PlayerData.MovementSpeed;

            _moveDirection = direction;

            if (Input.GetButton("Jump"))
            {
                _moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
            direction = directionTransform.TransformDirection(direction);
            direction *= PlayerData.MovementSpeed;

            _moveDirection.x = direction.x;
            _moveDirection.z = direction.z;
        }

        _moveDirection.y -= gravity * Time.deltaTime;
        _controller.Move(_moveDirection * Time.deltaTime);
    }
}
