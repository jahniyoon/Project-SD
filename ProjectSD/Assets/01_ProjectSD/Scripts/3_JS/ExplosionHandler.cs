using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{

    [Header("Particle")]
    private SphereCollider sphereCollider;

    [Header("Enemy")]
    public int id;
    public Enemy enemy;

    [Header("Explosion")]
    public float damage;
    public float activeTime = 0.1f;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        // 지정된 시간 후에 파티클을 Destroy() 하는 함수 호출
        StartCoroutine(DestroyCoroutine());

        // Start에서 가져와야함 Awake는 호출 순서때문에 오류난다.
        // 왜냐면 DataManager가 설정되기 전에 호출되기 때문
        // 데미지 & 콜라이더 설정 함수 호출
        damage = (int)DataManager.GetData(id, "Damage");
        SetSphereCollider();
    }

    // 콜라이더를 활성화 하면서 지정된 크기 만큼 키우는 함수
    private void SetSphereCollider()
    {
        // 콜라이더 크기 설정
        float size = (int)DataManager.GetData(id, "Range_Ex");
        sphereCollider.radius = size;

        // 콜라이더 활성화
        sphereCollider.enabled = true;
    }

    // 폭발 범위 내에 들어올 경우
    // 공격 대상에 RigidBody가 있어야 검출 가능
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"검출: {other.name}");
        // 태그가 플레이어일 경우
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player 폭발");
            // 데미지 처리
            other.GetComponent<PlayerHealth>().OnDamage(damage);
        }
        // 태그가 Enemy일 경우
        else if (other.CompareTag("Enemy"))
        {
            //Debug.Log("Enemy 폭발");

            // 데미지 처리
            //Debug.Log($"FF Name:{other.name} {other.GetComponent<Enemy>()}");
            other.GetComponent<Enemy>().OnDamage(damage);
        }
    }

    // 지정된 시간 후에 파티클을 삭제 하는 함수
    private IEnumerator DestroyCoroutine()
    {
        // activeTime만큼 대기
        yield return new WaitForSeconds(activeTime);

        // 파티클 삭제
        Destroy(gameObject);
    }
}
