using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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
    public CapsuleCollider capsuleCollider;
    GameObject enemyObject;

    private void Start()
    {
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    // Initialize를 하는 생성자
    public void Initalize(int id, GameObject gameObject, float scaleFactor)
    {
        // id 저장
        this.id = id;

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
        isDead();

        Debug.Log($"hp:{hp}");
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
        //SetBoxColliderSize(boxCollider, rangeEx);

        // 캡슐 콜라이더 비활성화
        capsuleCollider.enabled = false;

        // 플레이어에게 골드를 주는 함수
        GameManager.instance.MinionKillGetGold();

        // 폭발 인스턴스 생성
        Transform transform = gameObject.transform;
        CreateExplosion(FindExplosionPrefab(), transform.position, transform.rotation);

        // 0초 후에 Enemy 비활성화
        gameObject.SetActive(false);

        capsuleCollider.enabled = true;
        //EnemyManager.instance.ChangeActive(enemyObject, 0f, false);
    }

    // 기본 디렉토리
    private const string DIRECTORY = "Prefabs/";
    // 폭발 Prefab을 찾는 함수
    private GameObject FindExplosionPrefab()
    {
        // ID로 Prefab 검색 후 할당
        string prefabName = (string)DataManager.GetData(id, "Explosion_Prefab");
        GameObject prefab = Resources.Load<GameObject>(DIRECTORY + prefabName);

        // 반환
        return prefab;
    }

    // 오브젝트를 비활성화하는 함수
    public void ChangeActiveFalse()
    {
        // Enemy 활성화 상태 변경
        gameObject.SetActive(false);
    }

    // 공격 범위 조정을 위해
    // 박스 콜라이더 사이즈를 조정하는 함수
    public void SetBoxColliderSize(BoxCollider boxCollider, float size)
    {
        // 공격 범위를 설정한다.
        float scale = size * scaleFactor;
        // 박스 콜라이더 크기 조절
        boxCollider = gameObject.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(scale, scale, scale);

        // 박스 콜라이더 활성화
        boxCollider.enabled = true;
    }

    // 외부 메서드 호출
    // 폭발 파티클 인스턴스를 생성하는 함수
    private void CreateExplosion(GameObject prefab,
        Vector3 position, Quaternion rotate)
    {
        position.y += 1f;
        // 폭발 파티클 인스턴스 생성
        GameObject explosion = 
            GameObject.Instantiate(prefab, position, rotate);

        // 폭발 파티클에 아이디 할당
        explosion.GetComponent<ExplosionHandler>().id = id;
    }
}
