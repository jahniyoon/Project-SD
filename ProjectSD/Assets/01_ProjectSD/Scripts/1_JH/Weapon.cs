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

    public MeshRenderer[] meshes;   // 디버그용 메시

    int buttonLayerMask = (1 << 8);
    [Header("Laser Point")]
    public Transform firePoint;
    public GameObject hitPoint;
    public LineRenderer laserRenderer;

    [Header("Bullet")]
    public GameObject bulletPrefab;
    public float bulletID;

    [Header("Shop")]
    public bool isBtnEnable;
    public string btnName;

    public void OnEnable()
    {
        isUpgrade = false;
        GetData(isUpgrade);
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
            bullet.transform.GetChild(0).GetComponent<Bullet>().isUpgrade = isUpgrade;

            Destroy(bullet, bullet.transform.GetChild(0).GetComponent<Bullet>().bulletLifeTime);
        }
    }

    public void WeaponUpgrade()
    {
        if(!isUpgrade)
        {
            isUpgrade = true;
            GetData(isUpgrade);

            int index = meshes.Length;
            for (int i = 0; i < index; i++)
            {
                meshes[i].material.color = Color.red;
            }
            Invoke("ResetUpgrade", upgradeDuration);
        }
    }

    public void ResetUpgrade()
    {
        GetData(!isUpgrade);
        int index = meshes.Length;
        for (int i = 0; i < index; i++)
        {
            meshes[i].material.color = Color.black;
        }
        isUpgrade = false;

    }

    public void GetData(bool isUpgrade)
    {
        int index = 0;
        if (isUpgrade)
        {
            index = 1;
            Dictionary<string, List<string>> upgradeDataDictionary = default;
            upgradeDataDictionary = CSVReader.ReadCSVFile("CSVFiles/Unit_Weapon_Upgrade_Table");
            upgradeDuration = float.Parse(upgradeDataDictionary["ActTime"][0]);
        }
        Dictionary<string, List<string>> dataDictionary = default;
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Weapon_Table");
        weaponID = int.Parse(dataDictionary["ID"][index]);

        fireRate = float.Parse(dataDictionary["Shot_Delay"][index]);

      
    }
}
