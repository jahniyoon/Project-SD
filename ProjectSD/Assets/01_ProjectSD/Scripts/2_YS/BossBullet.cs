using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class BossBullet : MonoBehaviour
{
    public Transform target;
    private Rigidbody rigid;
    private MeshRenderer mesh;


    [Header("CSV")]
    public float hp = default;
    public float lifeTime = default;  //destroy할때 사라지는 시간?
    public float damage = default;
    public float speed = default;


    // Start is called before the first frame update
    void Start()
    {
        GetData();
        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        rigid.velocity = transform.forward * speed;


    }

    public void Launch(Transform target)
    {
        Debug.Log("런치 되나?");
        this.target = target;
        rigid = GetComponent<Rigidbody>();
        mesh = GetComponent<MeshRenderer>();
        rigid.velocity = transform.forward * speed;
    }


    // Update is called once per frame
    void Update()
    {
        DestroyBullet();
    }

    public void DestroyBullet()
    {
        if(GameManager.instance.isGameOver)
        {
            Destroy(gameObject);    
        }
    }

    public void GetData()
    {
        //Dictionary<string, List<string>> dataDictionary = default;
        //dataDictionary = CSVReader.ReadCSVFile("CSVFiles/GolemPattern01_Table"); //이름으로 가져옴

        //DataManager.SetData(dataDictionary);
        //hp = (int)DataManager.GetData(2004, "HP");//이름으로 가져오는거라서 순서상관 X 0번째 행  //변수 선언은 해야함
        //lifeTime = (float)DataManager.GetData(2004, "Projectile_Lifetime");
        //damage = (int)DataManager.GetData(2004, "Damage");
        //speed = (float)DataManager.GetData(2004, "Speed");

        hp = (float)DataManager.GetData(4001, "HP");
        lifeTime = (float)DataManager.GetData(4001, "Lifetime");
        damage = (float)DataManager.GetData(4001, "Damage");
        speed = (float)DataManager.GetData(4001, "Speed");
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            other.GetComponent<PlayerHealth>().OnDamage(damage);            
        }
    }

    // 보스 총알이 데미지를 입는 함수
    public void OnDamage(int damage)
    {
        hp -= damage;
        StartCoroutine("DamageColor");

        // 체력이 0이되면 파괴
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator DamageColor()
    {
        mesh.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        mesh.GetComponent<MeshRenderer>().material.color = Color.white;

    }

}
