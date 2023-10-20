using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFX : MonoBehaviour
{
    private SphereCollider rangeCollider; // 범위
    public GameObject Particle;

    [Header("Unit Info")]

    public int ID;              // 유닛 ID
    public string description;  // 유닛 설명
    public string modelInfo;    // 모델 설명
    public float lifeTime;      // 유닛 비활성화 시간
    public float radius;        // 콜리전 사이즈
    public float damage;        // 데미지 
    public float damageDelay;   // 데미지 딜레이
    public float speedReduce;   // 괴수 속도 감소율
    // 괴수 용 속도 = 괴수 기본속도*(1-speedReduce)



    // Start is called before the first frame update
    void Start()
    {
        GetData();
        rangeCollider = GetComponent<SphereCollider>();
        rangeCollider.radius = radius;  // 불 사거리 정해주고
        Particle.transform.localScale = new Vector3(radius, 1, radius); // 불 모양크기도 변경

        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetData()
    {
        ID = (int)DataManager.GetData(7041, "ID");
        description = (string)DataManager.GetData(ID, "Description");
        modelInfo = (string)DataManager.GetData(ID, "Model_Info");
        lifeTime = (float)DataManager.GetData(ID, "Collision_LifeTime");
        radius = (float)DataManager.GetData(ID, "Collision_Size");
        damage = (float)DataManager.GetData(ID, "Damage");
        damageDelay = (float)DataManager.GetData(ID, "Damage_Delay");
        speedReduce = (float)DataManager.GetData(ID, "Speed_Reduce");
    }
}
