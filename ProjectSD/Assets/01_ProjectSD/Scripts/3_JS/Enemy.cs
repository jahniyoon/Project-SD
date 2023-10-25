using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyState
    {
        Default,
        Slow,
        Stun,
        Fire
    }

    [Header("Enemy")]
    public EnemyState enemyState;
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
    public SphereCollider sphereCollider;
    GameObject enemyObject;
    NavMoveable navMoveable;

    private GameObject stunParticle;

    private void Start()
    {
        sphereCollider = gameObject.GetComponent<SphereCollider>();
        navMoveable = gameObject.GetComponent<NavMoveable>();
        //OnSlow(5f);
        //OnStun(10f);
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

        //Debug.Log($"hp:{hp}");
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

        // 스피어 콜라이더 비활성화
        //sphereCollider.enabled = false;

        // 플레이어에게 골드를 주는 함수
        GameManager.instance.MinionKillGetGold();

        // 폭발 인스턴스 생성
        Transform transform = gameObject.transform;
        CreateExplosion(FindExplosionPrefab(), transform.position, transform.rotation);

        // Enemy 삭제
        Destroy(gameObject);

        //capsuleCollider.enabled = true;
        //EnemyManager.instance.ChangeActive(enemyObject, 0f, false);
    }

    // 슬로우 배율 (speed * 슬로우 배율)
    private const float SLOW = 0.1f;
    // 슬로우 디버프를 주는 함수
    // t만큼 슬로우를 지속한다.
    public void OnSlow(float t)
    {
        // enemyState가 슬로우 상태가 아닐 경우
        if (enemyState != EnemyState.Slow)
        {
            // 슬로우 상태로 변경
            enemyState = EnemyState.Slow;

            // 속도를 조정 (speed * 슬로우 배율)
            navMoveable.ChangeSpeed(speed * SLOW);

            // 일정 시간 후에 슬로우 해제
            StartCoroutine(DisableSlow(t));
        }
    }

    // 스턴 디버프를 주는 함수
    // t만큼 스턴을 지속한다.
    public void OnStun(float t)
    {
        // enemyState가 스턴 상태가 아닐 경우
        if (enemyState != EnemyState.Stun)
        {
            // 스턴 상태로 변경
            enemyState = EnemyState.Stun;

            // 스턴 함수를 호출 (isStun = true)
            navMoveable.ToggleMoveable(true);

            // 스턴 파티클 생성 함수 호출
            CreateStunParticle();

            // 일정 시간 후에 스턴 해제
            StartCoroutine(DisableStun(t));
        }
    }

    // 불 장판에 닿았을 경우 처리하는 함수
    public void OnFire(float t, float damage)
    {
        // enemyState가 파이어 상태가 아닐 경우
        if (enemyState != EnemyState.Fire)
        {
            // Fire 상태로 변경
            enemyState = EnemyState.Fire;

            OnDamage(damage);

            // 일정 시간 후에 Fire 상태 해제
            StartCoroutine(DisableFire(t));
        }
    }

    // 스턴 파티클을 생성하는 함수
    private void CreateStunParticle()
    {
        // 스턴 파티클 인스턴스 생성
        stunParticle = Instantiate(
            Resources.Load<GameObject>("Prefabs/Stun_1"), gameObject.transform);
    }

    // 일정 시간후에 슬로우 디버프를 해제하는 코루틴 함수
    private IEnumerator DisableSlow(float t)
    {
        // 대기
        yield return new WaitForSeconds(t);

        // 상태 변경
        enemyState = EnemyState.Default;

        // 슬로우 해제
        navMoveable.ChangeSpeed(speed);
    }

    // 일정 시간 후에 스턴 디버프를 해제하는 코루틴 함수
    private IEnumerator DisableStun(float t)
    {
        // 대기
        yield return new WaitForSeconds(t);

        // 상태 변경
        enemyState = EnemyState.Default;

        // 스턴 해제
        navMoveable.ToggleMoveable(false);

        // 스턴 파티클 삭제
        Destroy(stunParticle);
    }

    // 일정 시간 후에 스턴 디버프를 해제하는 코루틴 함수
    private IEnumerator DisableFire(float t)
    {
        // 대기
        yield return new WaitForSeconds(t);

        // 상태 변경
        enemyState = EnemyState.Default;
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

        // 폭발 효과음 출력
        AudioManager.instance.PlaySFX("Explosion");
    }
}
