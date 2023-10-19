using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    public int id;
    public string description;
    public string modelInfo;
    public int type;
    public int hp;
    public float damage;
    public float speed;
    public float rangeRec;
    public float rangeAtt;
    public float rangeEx;
    public float scaleFactor;
    BoxCollider boxCollider;
    GameObject enemyObject;

    // Initialize를 하는 생성자
    public Enemy(int id, GameObject gameObject, float scaleFactor)
    {
        // Enemy 오브젝트를 저장
        enemyObject = gameObject;

        // 기타 변수 설정
        description = (string)DataManager.GetData(id, "Description");
        modelInfo = (string)DataManager.GetData(id, "Model_Info");
        type = (int)DataManager.GetData(id, "Type");
        hp = (int)DataManager.GetData(id, "HP");
        damage = (int)DataManager.GetData(id, "Damage");
        speed = (int)DataManager.GetData(id, "Speed");
        rangeRec = (int)DataManager.GetData(id, "Range_Rec");
        rangeAtt = (int)DataManager.GetData(id, "Range_Att");
        rangeEx = (int)DataManager.GetData(id, "Range_Ex");
        this.scaleFactor = scaleFactor;

        // 공격범위 초기화 함수 호출
        SetBoxColliderSize(boxCollider, rangeAtt);
    }

    // Enemy를 초기화 하는 함수
    public void Reset()
    {
        // HP 초기화
        hp = (int)DataManager.GetData(id, "HP");

        // 공격범위 초기화 함수 호출
        SetBoxColliderSize(boxCollider, rangeAtt);
    }

    // 플레이어에게 공격 받을 때 실행되는 함수
    public void OnDamage(float damage)
    {
        // 자신에게 데미지 처리
        hp -= (int)damage;

        // hp가 0 이하인 경우 처리
        if (hp <= 0)
        {
            isDead();
        }

         Debug.Log($"hp:{hp}");
        // 죽음을 체크하는 함수 호출
        isDead();
    }

    // 죽음을 체크하는 함수
    public void isDead()
    {
        if (hp <= 0)
        {
            // 죽음 처리를 하는 함수 호출
            OnDead();
        }
    }

    // 죽음 처리를 하는 함수
    public void OnDead()
    {
        Debug.Log("죽는다.");
        // 공격 범위를 폭발 범위로 조정
        SetBoxColliderSize(boxCollider, rangeEx);

        // 0.1초 후에 Enemy 비활성화
        EnemyManager.instance.ChangeActive(enemyObject, 0.1f, false);
    }

    // 오브젝트를 비활성화하는 함수
    public void ChangeActiveFalse()
    {
        // Enemy 활성화 상태 변경
        enemyObject.SetActive(false);
    }

    // 공격 범위 조정을 위해
    // 박스 콜라이더 사이즈를 조정하는 함수
    public void SetBoxColliderSize(BoxCollider boxCollider, float size)
    {
        // 공격 범위를 설정한다.
        float scale = size * scaleFactor;
        // 박스 콜라이더 크기 조절
        boxCollider = enemyObject.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(scale, scale, scale);

        // 박스 콜라이더 활성화
        boxCollider.enabled = true;
    }
}
