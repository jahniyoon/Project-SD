using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    public bool isUpgrade;         // 업그레이드 확인
    public float fireRate = 0.2f;         // 발사 속도
    public float lastFireTime;     // 마지막 발사시간

    int buttonLayerMask = (1 << 8);
    [Header("Laser Point")]
    public Transform firePoint;
    public GameObject hitPoint;
    private LineRenderer laserRenderer;

    [Header("Bullet")]
    public GameObject bulletPrefab;

    [Header("Shop")]
    public bool isBtnEnable;
    public string btnName;

    public void OnEnable()
    {
        isUpgrade = false;
        lastFireTime = 0;       // 시간 초기화
    }

    // Start is called before the first frame update
    void Start()
    {
        laserRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        LaserPointer();
    }

    public void LaserPointer()
    {
        laserRenderer.SetPosition(0, firePoint.position);

        RaycastHit rayHit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out rayHit, Mathf.Infinity, buttonLayerMask))
        {
            hitPoint.gameObject.SetActive(true);
            hitPoint.transform.position = rayHit.point;

            laserRenderer.SetPosition(1, rayHit.point);

            btnName = rayHit.transform.name;
            isBtnEnable = true;
        }
        else
        {
            laserRenderer.SetPosition(1, firePoint.forward * 2000);

            btnName = null;
            isBtnEnable = false;

            hitPoint.gameObject.SetActive(false);
        }
    }

    public void Shoot()
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            lastFireTime = Time.time;

            GameObject bullet =
             Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Destroy(bullet, bullet.transform.GetChild(0).GetComponent<Bullet>().bulletLifeTime);
        }
    }

    public void WeaponUpgrade()
    {
        if(!isUpgrade)
        {
            isUpgrade = true;
        }
    }
}
