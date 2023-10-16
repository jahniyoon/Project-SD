using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Transform target;
    public Transform boss;
    public NavMeshAgent agent;
    public GameObject bossBullet;

    public Transform bulletPort;

    public float traceDist = 100.0f; //추적 거리
    public float attackDist = 1.0f; // 공격 사정거리


    
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    public State state;

    private bool isDie = false;

    //임시 데미지 함수
    //public void OnDamage(float damage)
    //{
    //    //Weapon weapon = other.GetComponent<Weapon>();
    //    hp -= damage;

    //    if (hp <= 0)
    //    {
    //        state = State.DIE;
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();

        StartCoroutine(CheckMonsterState());

        StartCoroutine(MonsterAction());

        StartCoroutine(SkillCounter());
    }

    // Update is called once per frame
    void Update()
    {



    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.1f);


            if (state == State.DIE) yield break;


            float distance = Vector3.Distance(target.position, boss.position);


            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance >= traceDist)     //추적 사정거리 외부에서 추적 시작
            {
                state = State.TRACE;
            }
            //else if (distance <= traceDist)
            //{
            //    traceStart = true;
            //    state = State.TRACE;
            //}
            //else if (!traceStart)
            //{
            //    state = State.IDLE;
            //}
        }
    }

    public virtual IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {

                case State.IDLE:

                    agent.isStopped = true;


                    //anim.SetBool(hashTrace, false);
                    break;


                case State.TRACE:

                    agent.SetDestination(target.position);
                    agent.isStopped = false;

                    //임의의 시간 후 투사체
                    //StartCoroutine(SkillCounter());

                   
                    //anim.SetBool(hashTrace, true);


                    //anim.SetBool(hashAttack, false);
                    break;


                case State.ATTACK:
                    break;
           

                //사망
                case State.DIE:
                    isDie = true;
                    //추적 중지
                    agent.isStopped = true;
                    //사망 애니메이션 실행
                    //anim.SetTrigger(hashDie);

                    //몬스터의 Collider 컴포넌트 비활성화
                    //GetComponent<BoxCollider>().enabled = false;

                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnDrawGizmos()
    {
        
        if (state == State.TRACE)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
    }

    //IEnumerator HitPoint()
    //{
        

    //    //boxCollider.enabled = false;
    //    //Debug.Log("비활성화");
    //    yield return new WaitForSeconds(5.0f);
    //    //boxCollider.enabled = true;
    //    //Debug.Log("활성화");
    //}


    void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            //TODO:졸개 몬스터 소환 로직
        }

        //if(other.tag.Equals("Bullet"))
        //{
        //    //StartCoroutine(HitPoint());
        //}
    }

    IEnumerator SkillCounter()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(4.0f);

            if (state == State.TRACE)
            {
                SkillAttack();
            }
        }
       
    }

    void SkillAttack()
    {
        GameObject instantBullet = Instantiate(bossBullet, bulletPort.position, bulletPort.rotation);

        bulletPort.transform.LookAt(target);
        instantBullet.transform.LookAt(target);
        
    }
}
