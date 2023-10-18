using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static OVRPlugin;


public class Boss : MonoBehaviour
{
    public Transform target;
    public Transform boss;
    public NavMeshAgent agent;
    public GameObject bossBullet;
    public Animation anim;

    public SkinnedMeshRenderer mesh;

    public GameObject[] weakPoint = default;
    public Slider bossHPSlider;

    [Header ("CSV")]
    public float hp = default;
    public float actTime = default;
    public float weakPointRate = default;

    [Header("투사체 생성 지점")]
    public Transform bulletPort;


    [Header("거리")]
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
        GetData();
    }

    public void GameStart()
    {
        SetMaxHealth(hp);   // 생성되고나서 HP 슬라이더를 업데이트한다.
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();

        anim.CrossFade("walk");
        anim["walk"].speed = 0.15f;

        StartCoroutine(CheckMonsterState());

        StartCoroutine(MonsterAction());

        StartCoroutine(SkillCounter());
    }

    // Update is called once per frame
    void Update()
    {

       
    }

    public void SetMaxHealth(float newHealth)
    {
        bossHPSlider.maxValue = newHealth;
        bossHPSlider.value = newHealth;
    }
    public void SetHealth(float newHealth)
    { 
        bossHPSlider.value = newHealth;
    }


    public void GetData()
    {
        Debug.Log("호출");
        Dictionary<string, List<string>> dataDictionary = default;
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Golem_Table"); //이름으로 가져옴
        DataManager.SetData(dataDictionary);
        hp = (int)DataManager.GetData(3001, "HP"); //이름으로 가져오는거라서 순서상관 X 0번째 행  //변수 선언은 해야함
        weakPointRate = (float)DataManager.GetData(3001, "WeakpointRate");
        actTime = (float)DataManager.GetData(3001, "ActTime");
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
            else if (hp <= 0)
            {
                state = State.DIE;
                GameManager.instance.GameClear();
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


                    //anim.SetBool("IsWalk", false);

                    break;


                case State.TRACE:

                    agent.SetDestination(target.position);
                    agent.isStopped = false;

                    anim.CrossFade("walk");
                    anim["walk"].speed = 0.15f;

                    //임의의 시간 후 투사체
                    //StartCoroutine(SkillCounter());


                    //anim.SetBool("IsWalk", true);


                    //anim.SetBool(hashAttack, false);
                    break;


                case State.ATTACK:           
                    break;
           

                //사망
                case State.DIE:
                    isDie = true;
                    //추적 중지
                    agent.isStopped = true;
                    anim.CrossFade("death", 0.25f);

                    //GameManager.instance.GameOver();
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

    IEnumerator HitPoint()
    {
        //boxCollider.enabled = false;
        //Debug.Log("비활성화");
        yield return new WaitForSeconds(5.0f);
        //boxCollider.enabled = true;
        //Debug.Log("활성화");
    }


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

    public void OnDamage(float damage)
    {
        hp -= damage;
        SetHealth(hp);
        StartCoroutine("DamageColor");

    }

    


    IEnumerator DamageColor()
    {
        mesh.materials[0].color = Color.red;
        //mesh.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        mesh.materials[0].color = Color.white;

        //mesh.GetComponent<MeshRenderer>().material.color = Color.white;

    }
    
    IEnumerator SkillCounter()
    {
        while(!isDie)
        {
            

            if (state == State.TRACE)
            {
                SkillAttack();
                anim.CrossFade("attack5");
            }

            yield return new WaitForSeconds(5.0f);
           
        }

    }

    void SkillAttack()
    {
        GameObject instantBullet = Instantiate(bossBullet, bulletPort.position, bulletPort.rotation);
        BossBullet bulletScript = instantBullet.GetComponent<BossBullet>();

        bulletPort.transform.LookAt(target);
        instantBullet.transform.LookAt(target);
        bulletScript.Launch(target);

    }



}
