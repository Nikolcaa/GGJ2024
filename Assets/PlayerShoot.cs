using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public RectTransform crosshair;
    public Transform shootingPoint;
    public LayerMask layerMask;
    public Transform gun;
    public Transform gunTargetPosition;

    private PlayerMovement playerMovement;

    private Vector3 bakedGunPos;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        bakedGunPos = gun.localPosition;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    float t;
    bool isPressed = false;
    void Update()
    {
        t += Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if (t >= 1 / playerMovement.PlayerData.AttackSpeed)
            {
                t = 0;

                Vector3 crosshairPosition = crosshair.position;

                RaycastHit hit;
                if (Physics.Raycast(Camera.main.transform.position, crosshairPosition - Camera.main.transform.position, out hit, 100, layerMask))
                {
                    crosshairPosition = hit.point;
                }

                GameObject bullet = Instantiate(bulletPrefab, shootingPoint);
                bullet.transform.SetParent(null);
                //bullet.transform.LookAt(bulletPosition);
                bullet.transform.forward = crosshairPosition - bullet.transform.position;
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;
                Destroy(bullet, 2f);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!isPressed)
            {
                isPressed = true;
                gun.DOLocalMove(gunTargetPosition.localPosition, 0.5f)
                    .SetEase(Ease.InOutQuad);
            }
            else
            {
                isPressed = false;
                gun.DOLocalMove(bakedGunPos, 0.5f)
                    .SetEase(Ease.InOutQuad);
            }
        }

    }
}
