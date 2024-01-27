using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
    private Transform graph;
    public GameObject playerDead_Objects;

    private Vector3 _moveDirection = Vector3.zero;
    private CharacterController _controller;

    private Volume globalVolume;

    public static Transform Transform;

    public static bool isDead = false;

    private void Awake()
    {
        Transform = transform;
        graph = transform.GetChild(0);
        globalVolume = FindObjectOfType<Volume>();
    }

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isDead)
            return;

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

    private void Die(Enemy enemy)
    {
        if (isDead)
            return;

        isDead = true;
        _controller.enabled = false;

        Vector3 dir = (transform.position - enemy.graphChild.position).normalized;
        dir.y = 0;
        dir.x *= 3;
        dir.z *= 3;

        //transform.DOMove(transform.position + dir, 1)
        //    .SetEase(Ease.OutBack);

        transform.DOJump(transform.position + dir, 1, 1, 2f)
            .SetEase(Ease.OutBack);

        transform.DORotate(new Vector3(-90, 0, 0), 1.5f)
            .SetEase(Ease.OutBack);

        enemy.Die();
        playerDead_Objects.SetActive(true);

        StartCoroutine(asd());
        IEnumerator asd()
        {
            yield return new WaitForSeconds(0.3f);
            Vignette vignette;
            if (globalVolume.profile.TryGet(out vignette))
            {
                vignette.color.Override(Color.red);
                vignette.intensity.Override(0.5f);
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            Die(enemy);
        }
    }
}
