using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public Transform target;
    public NavMeshAgent agent;

    public enum State
    {
        IDLE,
        TRACE,
        ATTAC,
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
    }

    // Update is called once per frame
    void Update()
    {
        

        
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);

       
            if (state == State.DIE) yield break;

            
            //float distance = Vector3.Distance(playerTr.position, monsterTr.position);

        
            //if (distance <= attackDist)
            //{
            //    state = State.ATTACK;
            //}
            //else if (distance >= traceDist)     //추적 사정거리 외부에서 추적 시작
            //{
            //    state = State.TRACE;
            //}
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
                    
                    //agent.SetDestination(playerTr.position);
                    agent.isStopped = false;

                   
                    //anim.SetBool(hashTrace, true);

                    
                    //anim.SetBool(hashAttack, false);
                    break;

                
                //case State.ATTACK:
                    

                //    switch (monsterType)
                //    {
                //        case Type.A:
                //            anim.SetBool(hashAttack, true);
                //            break;

                //        case Type.B:

                //            anim.SetBool(hashAttack, true);
                //            yield return new WaitForSeconds(0.65f);

                //            Vector3 fireBallTransform = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
                //            GameObject instantBullet = Instantiate(bullet, fireBallTransform, transform.rotation);
                //            Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                //            rigidBullet.velocity = transform.forward * 3;

                //            GameObject leftInstantBullet = Instantiate(bullet, fireBallTransform, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -15, 0)));
                //            Rigidbody rigidLeftBullet = leftInstantBullet.GetComponent<Rigidbody>();
                //            rigidLeftBullet.velocity = leftInstantBullet.transform.forward * 3;


                //            GameObject rightInstantBullet = Instantiate(bullet, fireBallTransform, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 15, 0)));
                //            Rigidbody rigidRightBullet = rightInstantBullet.GetComponent<Rigidbody>();
                //            rigidRightBullet.velocity = rightInstantBullet.transform.forward * 3;

                //            yield return new WaitForSeconds(1.5f);
                //            break;

                //        case Type.C:

                //            int knight = Random.Range(0, 3);

                //            switch (knight)
                //            {
                //                case 0:
                //                    anim.SetBool(hashAttack, true);
                //                    yield return new WaitForSeconds(2.0f);
                //                    anim.SetBool(hashAttack, false);
                //                    yield return new WaitForSeconds(0.1f);
                //                    break;

                //                case 1:
                //                    anim.SetBool(hashAttack1, true);
                //                    yield return new WaitForSeconds(2.0f);
                //                    anim.SetBool(hashAttack1, false);
                //                    yield return new WaitForSeconds(0.1f);
                //                    break;

                //                case 2:
                //                    anim.SetBool(hashSpawn, true);
                //                    yield return new WaitForSeconds(1.5f);

                //                    GameObject instantSpawnA = Instantiate(monsterPrefab, spawnA.position, spawnA.rotation);
                //                    GameObject instantSpawnB = Instantiate(monsterPrefab, spawnB.position, spawnB.rotation);
                //                    GameObject instantSpawnC = Instantiate(monsterPrefab, spawnC.position, spawnC.rotation);
                //                    anim.SetBool(hashSpawn, false);

                //                    yield return new WaitForSeconds(1.0f);
                //                    break;
                //            }
                //            break;

                //        case Type.D:

                //            int dog = Random.Range(0, 2);

                //            switch (dog)
                //            {
                //                case 0:
                //                    anim.SetBool(hashAttack, true);
                //                    yield return new WaitForSeconds(2.5f);
                //                    anim.SetBool(hashAttack, false);
                //                    yield return new WaitForSeconds(0.1f);
                //                    break;
                //                case 1:
                //                    anim.SetBool(hashRock, true);
                //                    yield return new WaitForSeconds(0.76f);

                //                    GameObject instantSpawnA = Instantiate(rock, rockSpawnA.position, rockSpawnA.rotation);
                //                    Rigidbody rigidRock = instantSpawnA.GetComponent<Rigidbody>();
                //                    rigidRock.velocity = transform.forward * 4;

                //                    GameObject leftInstantSpawnB = Instantiate(rock, rockSpawnB.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -30, 0)));
                //                    Rigidbody rigidLeftrock = leftInstantSpawnB.GetComponent<Rigidbody>();
                //                    rigidRock.velocity = leftInstantSpawnB.transform.forward * 4;

                //                    GameObject rightInstantSpawnC = Instantiate(rock, rockSpawnC.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 30, 0)));
                //                    Rigidbody rigidRightrock = rightInstantSpawnC.GetComponent<Rigidbody>();
                //                    rigidRock.velocity = rightInstantSpawnC.transform.forward * 4;


                                  

                                    //anim.SetBool(hashRock, false);
                    //                yield return new WaitForSeconds(1.0f);
                    //                break;
                    //        }
                    //        break;
                    //}
                    //break;

                //사망
                case State.DIE:
                    isDie = true;
                    //추적 중지
                    agent.isStopped = true;
                    //사망 애니메이션 실행
                    //anim.SetTrigger(hashDie);
                    //몬스터의 Collider 컴포넌트 비활성화
                    GetComponent<BoxCollider>().enabled = false;

                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
