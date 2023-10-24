using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Weapon : MonoBehaviour
{
    [Header("Weapon")]
    public int weaponID;
    public bool isUpgrade;         // 업그레이드 확인
    public float fireRate = 0.2f;         // 발사 속도
    public float lastFireTime;     // 마지막 발사시간

    public float upgradeDuration;

    public GameObject[] weapon;

    public MeshRenderer[] meshes;   // 디버그용 메시


    int buttonLayerMask = (1 << 8);
    [Header("Laser Point")]
    public Transform firePoint;
    public GameObject hitPoint;
    public LineRenderer laserRenderer;
    public Material[] materials;

    Vector3 originScale = Vector3.one * 0.02f;
    public Transform pointUI;


    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletID;
    public GameObject[] bullets;

    [Header("Shop")]
    public bool isBtnEnable;
    public string btnName;

    public bool isRight;

    private int upgradeWeaponIndex = 0;       // 업그레이드 효과 끝난후 갯수 -해줄 Index
    public void OnEnable()
    {
        pointUI.gameObject.SetActive(false);
        isUpgrade = false;
        bulletPrefab = bullets[0];

        weapon[1].gameObject.SetActive(false);
        firePoint = weapon[0].transform.GetChild(0).transform;
        GetData(isUpgrade);

        lastFireTime = 0;       // 시간 초기화

    }

    // Start is called before the first frame update
    void Start()
    {
        laserRenderer = GetComponent<LineRenderer>();
        laserRenderer.material = materials[0];

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
            //hitPoint.gameObject.SetActive(true);
            //hitPoint.transform.position = rayHit.point;
            pointUI.gameObject.SetActive(true);
            pointUI.transform.position = rayHit.point;
            pointUI.localScale = originScale * Mathf.Max(1, rayHit.distance);


            laserRenderer.SetPosition(1, rayHit.point);

            btnName = rayHit.transform.name;
            isBtnEnable = true;
        }
        else
        {
            laserRenderer.SetPosition(1, firePoint.forward * 2000);

            btnName = null;
            isBtnEnable = false;

            //hitPoint.gameObject.SetActive(false);
            pointUI.gameObject.SetActive(false);
        }
    }

    public void Shoot()
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            lastFireTime = Time.time;

            GameObject bullet =
             Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.transform.GetComponent<Bullet>().isUpgrade = isUpgrade;
            if (isRight)
            {
                if (!isUpgrade)
                {
                    AudioManager.instance.PlaySFX("Fire1");
                }
                else
                    AudioManager.instance.PlaySFX("UpFire1");

            }
            else
            {
                if (!isUpgrade)
                {
                    AudioManager.instance.PlaySFX("Fire2");
                }
                else
                    AudioManager.instance.PlaySFX("UpFire2");
            }
            Destroy(bullet, bullet.transform.GetComponent<Bullet>().bulletLifeTime);
        }
    }

    public void WeaponUpgrade()
    {
        if (!isUpgrade)
        {
            isUpgrade = true;
            GetData(isUpgrade);

            weapon[0].gameObject.SetActive(false);
            weapon[1].gameObject.SetActive(true);

            bulletPrefab = bullets[1];

            firePoint = weapon[1].transform.GetChild(0).transform;
            laserRenderer.material = materials[1];


            Invoke("ResetUpgrade", upgradeDuration);
        }
    }

    public void ResetUpgrade()
    {
        GetData(!isUpgrade);

        weapon[0].gameObject.SetActive(true);
        weapon[1].gameObject.SetActive(false);

        bulletPrefab = bullets[0];

        firePoint = weapon[0].transform.GetChild(0).transform;
        laserRenderer.material = materials[0];


        isUpgrade = false;
        if (GameManager.buttonsList[upgradeWeaponIndex].NowItemValue != 0)
        {
            GameManager.buttonsList[upgradeWeaponIndex].NowItemValue -= 1;
        }

    }

    public void GetData(bool isUpgrade)
    {
        int index = 1001;
        if (isUpgrade)
        {
            index += 1;
            upgradeDuration = (float)DataManager.GetData(7010, "ActTime");
        }

        weaponID = (int)DataManager.GetData(index, "ID");
        fireRate = (float)DataManager.GetData(index, "Shot_Delay");

    }
}
