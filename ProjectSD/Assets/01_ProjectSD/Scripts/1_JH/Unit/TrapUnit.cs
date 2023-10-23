using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapUnit : MonoBehaviour
{
    private BoxCollider trapTrigger;

    [Header("Unit Info")]
    public int ID;              // 유닛 ID
    public float unitLifeTime;  // 유닛 지속시간
    public float trapLifeTime;  // 방벽 지속시간
    public float stunTime;      // 스턴 시간

    public float triggerSize;   // 방벽 개시를 위한 트리거 크기
    public float trapSize;      // 방벽 사이즈

    public GameObject trapObj;  // 트랩 오브젝트
    public GameObject trapUnitObj;
    public Animator trapAnim;   // 트랩 애니메이션
    private bool isTrapOn;

    // Start is called before the first frame update
    void Start()
    {
        GetData();
        trapTrigger = GetComponent<BoxCollider>();
        trapTrigger.size = new Vector3(triggerSize, 1, 1); // 트리거 사이즈를 정해준다.
        trapObj.gameObject.transform.localScale = new Vector3(triggerSize/2, 1, 1);
        trapObj.GetComponent<Trap>().stunTime = stunTime;

        trapAnim = trapObj.GetComponent<Animator>();

        Invoke("TrapUnitDisable", unitLifeTime);    // 일정 시간 뒤에 유닛은 사라지고
        Destroy(gameObject, unitLifeTime + trapLifeTime); // 유닛 지속 시간 이후 소멸
    }

    public void TrapUnitDisable()
    {
        trapUnitObj.gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !isTrapOn)    // 적 태그를 만날 경우
        {
            isTrapOn = true;
            trapTrigger.enabled = false;    // 콜라이더 꺼주고
            trapAnim.SetTrigger("TrapOn");
        }
    }


    public void GetData()
    {
        ID = (int)DataManager.GetData(7030, "ID");
       
        unitLifeTime = (float)DataManager.GetData(ID, "Value1");
        trapLifeTime = (float)DataManager.GetData(ID+1, "Value1");
        stunTime = (float)DataManager.GetData(ID+2, "Value1");

        triggerSize = (float)DataManager.GetData(ID, "Trigger_Size");
        trapSize = (float)DataManager.GetData(ID, "Trap_Size");   
    }
}
