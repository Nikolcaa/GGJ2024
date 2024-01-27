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

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    float t;
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
    }
}
