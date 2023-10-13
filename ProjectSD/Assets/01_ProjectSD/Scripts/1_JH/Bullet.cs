using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid = default;
    private CapsuleCollider bulletCollider;
    
    [Header("Bullet Damage")]
    public float bulletDamage = 1f;         // 총알 데미지
    public float critIncrease = 200f;        // 총알 치명타율
    public float critProbability = 0.1f;   // 총알 치명타 확률
    public float finalDamage;               // 최종 데미지

    public MeshRenderer mat;

    [Header("Bullet Option")]
    public float bulletSpeed = 10f;               // 총알 속도  
    public float bulletLifeTime = 10f;            // 총알 유지시간
    public float bulletSize = 0.5f;



    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.velocity = transform.forward * bulletSpeed;

        bulletCollider = this.transform.GetChild(0).GetComponent<CapsuleCollider>();
        bulletCollider.radius = bulletSize;

        DamageCalculate();
        Destroy(gameObject, bulletLifeTime);
    }

    // 데미지 계산
    public void DamageCalculate()
    {
        float clit = Random.Range(0f, 1f);
        Debug.Log("치명타율 : " + clit);

        if(clit < critProbability)
        {
            bulletCollider.radius = 2f;
            mat.material.color = Color.red;
            finalDamage = bulletDamage * (clit / 100);
        }
        else
        finalDamage = bulletDamage;
    }
}
