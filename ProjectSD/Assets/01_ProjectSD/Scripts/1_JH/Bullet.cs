using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid = default;
    private SphereCollider bulletCollider;

    public float weaponID;
    public bool isUpgrade;
    public GameObject damageEffect;


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

    private TMP_Text bulletText;

    public bool isCrit;

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
            isCrit = true;
            bulletCollider.radius = 2f;
            mat.material.color = Color.red;
            finalDamage = bulletDamage * (critIncrease / 100);
        }
        else
        finalDamage = bulletDamage;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BossBullet"))
        {
            GameObject bossBullet = other.gameObject;
            bossBullet.GetComponent<BossBullet>().OnDamage(Mathf.FloorToInt(finalDamage));

            DamageEffect(other.tag);

            GameObject bullet = this.transform.parent.gameObject;
            Destroy(bullet);
        }
        else if (other.CompareTag("Boss"))
        {
            GameManager.instance.BossAttackGetGold();
            Debug.Log("보스를 맞췄다.");
            other.transform.root.GetComponent<Boss>().OnDamage(Mathf.FloorToInt(finalDamage));
            DamageEffect(other.tag);
            GameObject bullet = this.transform.parent.gameObject;
            Destroy(bullet);
            //other.GetComponent<Boss>().OnDamage();
        }
        else if (other.CompareTag("WeakPoint"))
        {
            Debug.Log("약점을 맞췄다.");
            other.transform.GetComponent<BossHitPoint>().OnDamage(Mathf.FloorToInt(finalDamage));
            DamageEffect(other.tag);
            GameObject bullet = this.transform.parent.gameObject;
            Destroy(bullet);

            //other.transform.root.GetComponent<Boss>().OnWeakPointDamage(Mathf.FloorToInt(finalDamage));

        }
        else if (other.CompareTag("Enemy"))
        {
            Debug.Log("적을 맞췄다.");

            if (other.transform.GetComponent<EnemyNormal>() != null)
            {
                other.transform.GetComponent<EnemyNormal>().enemy.
                    OnDamage(Mathf.FloorToInt(finalDamage));
            }
            else if (other.transform.GetComponent<EnemyFast>() != null)
            {
                other.transform.GetComponent<EnemyFast>().enemy.
                OnDamage(Mathf.FloorToInt(finalDamage));
            }

            DamageEffect(other.tag);
            GameObject bullet = this.transform.parent.gameObject;
            Destroy(bullet);
            //other.transform.root.GetComponent<Boss>().OnWeakPointDamage(Mathf.FloorToInt(finalDamage));

        }
    }


    public void DamageEffect(string tag)
    {
        Vector3 effectPos = this.transform.position;
        Vector3 vDist = effectPos - GameManager.instance.PC.transform.position; // 이펙트와 플레이어의 거리
        Vector3 vDir = vDist.normalized;    // 이펙트와 플레이어의 방향

        //float distance = Vector3.Distance(effectPos, GameManager.instance.PC.transform.position);
        GameObject damageFX =
             Instantiate(damageEffect, vDir * 10, Quaternion.identity);
       
        bulletText = damageFX.GetComponent<TextDisolve>().textObj;
        bulletText.text = string.Format("{0}", finalDamage);
        damageFX.GetComponent<TextDisolve>().colorName = "white";

        // 약점을 맞췄을 경우
        if (tag == "WeakPoint")
        {
            bulletText.text = string.Format("{0}", finalDamage * 1.5f);
            damageFX.GetComponent<TextDisolve>().colorName = "yellow";
        }
        // 크리티컬일 경우
        if (isCrit)
        {
            damageFX.GetComponent<TextDisolve>().colorName = "red";
        }

        damageFX.transform.forward = Camera.main.transform.forward;
     

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

        bulletDamage = float.Parse(dataDictionary["Att"][index]);
        critIncrease = float.Parse(dataDictionary["Critical_Rate"][index]);
        critProbability = float.Parse(dataDictionary["Critical_Chance"][index]);
        bulletSpeed = float.Parse(dataDictionary["Vlocity"][index]);
        bulletLifeTime = float.Parse(dataDictionary["LifeTime"][index]);
    }
}
