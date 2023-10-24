using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFast : MonoBehaviour
{
    [Header("EnemyNormal")]
    private const int ID = 5002;
    private const float SCALE_FACTOR = 3f;
    public Enemy enemy;
    public GameObject explosionPrefab;
    private NavMoveable navMoveable;

    private void Awake()
    {
        enemy = gameObject.GetComponent<Enemy>();
    }

    // OnEnable 할 때 마다 객체 초기화
    private void OnEnable()
    {
        // ID로 Enemy클래스의 객체를 생성하고 기본설정을 한다.
        enemy.Initalize(ID, gameObject, SCALE_FACTOR);
        navMoveable = GetComponent<NavMoveable>();
        navMoveable.speed = enemy.speed;
    }

    // 공격용 콜라이더 함수
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"공격범위 내 감지{other.name}");
        if (other.tag.Equals("Player"))
        {
            //Debug.Log("플레이어 공격");
            other.GetComponent<PlayerHealth>().OnDamage(enemy.damage);

            //Debug.Log("플레이어에게 자폭 공격");
            enemy.OnDead();
        }
    }
}
