using DG.Tweening;
using Google.GData.Documents;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireUnit : MonoBehaviour
{
    private SphereCollider rangeCollider; // 인식 범위 콜라이더

    [Header("Unit Info")]
    public int ID;              // 유닛 ID
    public string description;  // 유닛 설명
    public string modelInfo;    // 모델 설명
    public float range;         // 인식 범위
    public float shotDelay;     // 발사 딜레이
    public float actTime;       // 활성화 시간

    [Header("Fire Option")]
    public GameObject FireBombPrefab;   // 발사할 화염병 프리팹
    public float power;
    public Transform firePosition;

    public float lastFireTime;     // 마지막 발사시간

    private const int FIREUNITINDEX = 3;
    bool fireReady;



    // Start is called before the first frame update
    void Start()
    {
        GetData();  // 데이터를 가져오고

        lastFireTime = 0; // 마지막 발사시간 초기화
        rangeCollider = GetComponent<SphereCollider>();
        rangeCollider.radius = range;   // 콜라이더의 반경 설정

        Invoke("ReadyCheck", 3f);
        

        Destroy(gameObject, actTime); //일정시간 뒤에 제거
    }

    public void ReadyCheck()
    {

       fireReady= true;
    }
    private void OnDestroy()
    {       // Destroy되는 순간 호출
        GameManager.buttonsList[FIREUNITINDEX].NowItemValue -= 1;
    }
    public void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Finish") && fireReady)    // 보스 태그를 만났을 때
        {
            if (Time.time >= lastFireTime + shotDelay)  // 시간이 마지막 시간과 딜레이시간을 더한 값보다 클 경우
            {
                lastFireTime = Time.time;   // 마지막 시간 업데이트 후
                Shoot();    // 발사
            }
        }
    }

    // 발사
    public void Shoot()
    {
        AudioManager.instance.PlaySFX("FireBomb");

        GameObject ball = Instantiate(FireBombPrefab, firePosition.position, Quaternion.identity);
        ball.GetComponent<Rigidbody>().AddForce(calculateForce(), ForceMode.Impulse);
    }
    // 힘 계산하기
    public Vector3 calculateForce()
    {
        Vector3 targetPos = GameManager.instance.Golem.transform.position;
        targetPos.y += 25f;
        //firePosition.LookAt();  // 발사방향은 골렘을 향하게 변경
        firePosition.LookAt(targetPos);  // 발사방향은 골렘을 향하게 변경
        return firePosition.forward * power;
    }

    public void GetData()
    {
        ID = (int)DataManager.GetData(7040, "ID");
        description = (string)DataManager.GetData(ID, "Description");
        modelInfo = (string)DataManager.GetData(ID, "Model_Info");
        range = (float)DataManager.GetData(ID, "Range_Rac");
        shotDelay = (float)DataManager.GetData(ID, "ShotDelay");
        actTime = (float)DataManager.GetData(ID, "ActTime");
        power = (float)DataManager.GetData(ID, "Power");
    }
}
