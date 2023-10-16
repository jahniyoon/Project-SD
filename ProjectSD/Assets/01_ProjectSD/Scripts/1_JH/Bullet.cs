using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid = default;
    private SphereCollider bulletCollider;

    public float weaponID;
    public bool isUpgrade;

    [Header("Bullet Damage")]
    public float bulletDamage;         // 총알 데미지
    public float critIncrease;        // 총알 치명타율
    public float critProbability;   // 총알 치명타 확률
    public float finalDamage;               // 최종 데미지

    public MeshRenderer mat;

    [Header("Bullet Option")]
    public float bulletSpeed = 10f;               // 총알 속도  
    public float bulletLifeTime = 10f;            // 총알 유지시간
    public float bulletSize = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        GetData();

        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * bulletSpeed;

        bulletCollider = GetComponent<SphereCollider>();
        bulletCollider.radius = bulletSize;

        DamageCalculate();
    }

    // 데미지 계산
    public void DamageCalculate()
    {
        float clit = Random.Range(0f, 1f);
        //Debug.Log("치명타율 : " + clit);

        if(clit < critProbability)
        {
            bulletCollider.radius = 2f;
            mat.material.color = Color.red;
            finalDamage = bulletDamage * (clit / 100);
        }
        else
        finalDamage = bulletDamage;
    }

    public void OnTriggerEnter(Collider other)
    {
        GameObject bullet = this.transform.parent.gameObject;
        Debug.Log(bullet);
        Destroy(bullet);
    }

    public void GetData()
    {
        int index = 0;
        if(isUpgrade)    // 2000이면 강화무기
        { 
            index = 1; 
        }

        Dictionary<string, List<string>> dataDictionary = default;
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Projectile_Table");

        bulletDamage = float.Parse(dataDictionary["Damage"][index]);
        critIncrease = float.Parse(dataDictionary["Crit_Increase"][index]);
        critProbability = float.Parse(dataDictionary["Crit_Probability"][index]);
        bulletSpeed = float.Parse(dataDictionary["Speed"][index]);
        bulletLifeTime = float.Parse(dataDictionary["LifeTime"][index]);
    }
}
