using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildRemove : MonoBehaviour
{
    // 컴포넌트를 가져올변수들
    private PlayerShooter pShooter;     // 플레이어의 총 방향을 위한 Class
    private PlayerInputs input;         // 플레이어의 입력값을 받기위한 Class

    private bool isUnitRemove;        // 현재 유닛 제거 상태인지 확인할 bool 변수

    private int terrainMask = 1 << 7;

    public bool IsUnitRemove        // isUnitRemove의 프로퍼티
    {
        get { return isUnitRemove; }
        set
        {
            if(isUnitRemove != value)
            {
                isUnitRemove = value;
            }
        }
    }

    private RaycastHit hitInfo;     // Ray를 감지할 CastHit

    private void Awake()
    {
        AwakeInIt();
        ComponentInit();
    }

    void Start()
    {
        
    }


    void Update()
    {
        if(IsUnitRemove == true)
        {       // if : 제거상태 true일때에
            pShooter.rightGun.laserRenderer.SetPosition(0, pShooter.rightGun.firePoint.position);
            if (Physics.Raycast(pShooter.rightGun.firePoint.position, pShooter.rightGun.firePoint.forward, out hitInfo, Mathf.Infinity, ~terrainMask))
            {
                //Debug.LogFormat("Hit Name -> {0}", hitInfo.collider.gameObject.name);
                if (hitInfo.collider.gameObject.CompareTag("Unit"))
                {       // 유닛을 감지했을떄에
                    //Debug.LogFormat("Unit Name -> {0}", hitInfo.collider.gameObject.name);
                    if (input.select)
                    {
                        Debug.Log("제거시작 들어옴");
                        // Ray를 맞은 Unit 제거
                        Destroy(hitInfo.collider.gameObject);
                        IsUnitRemove = false;
                        input.select = false;
                    }
                }
            }       // RayCastEnd
        } 
    }       // Update()

    private void AwakeInIt()
    {
        IsUnitRemove = false;
    }       // AwakeInIt()

    private void ComponentInit()
    {
        pShooter = GetComponent<PlayerShooter>();
        input = GetComponent<PlayerInputs>();
    }
}       // ClassEnd
