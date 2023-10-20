using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBomb : MonoBehaviour
{
    [Header("Unit Info")]

    public int ID;              // 유닛 ID
    public string description;  // 유닛 설명
    public string modelInfo;    // 모델 설명
    public float velocity;    // 속도
    public float lifeTime;    // 유닛 비활성화 시간



    // Start is called before the first frame update
    void Start()
    {
        GetData();

        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void GetData()
    {
        ID = (int)DataManager.GetData(2003, "ID");
        description = (string)DataManager.GetData(ID, "Description");
        modelInfo = (string)DataManager.GetData(ID, "Model");
        velocity = (float)DataManager.GetData(ID, "Velocity");
        lifeTime = (float)DataManager.GetData(ID, "LifeTime");
    }
}
