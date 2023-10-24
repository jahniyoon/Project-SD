using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
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

    [Header("CSV")]
    public float hp = default;
    public float actTime = default;
    public float weakPointRate = default;
    public float coolTime = default;
    public float traceDist = 100.0f;
    public float speed = default;

    public float speedReduce = default;


    [Header("투사체 생성 지점")]
    public Transform bulletPort;


    [Header("거리")]

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

    private bool isSpeedDown = false;

    private float timer = 0f;
    private float interval = 1.0f;

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
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        SetMaxHealth(hp);   // 생성되고나서 HP 슬라이더를 업데이트한다.
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        anim = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

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
        //Debug.Log("호출");
        //Dictionary<string, List<string>> dataDictionary = default;
        //dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Golem_Table"); //이름으로 가져옴
        //DataManager.SetData(dataDictionary);
        hp = (float)DataManager.GetData(3001, "HP"); //이름으로 가져오는거라서 순서상관 X 0번째 행  //변수 선언은 해야함
        speed = (float)DataManager.GetData(3001, "MoveSpeed");
        coolTime = (float)DataManager.GetData(3001, "Cooltime");
        //traceDist = (float)DataManager.GetData(3001, "CheckRange");

        speedReduce = (float)DataManager.GetData(7041, "Speed_Reduce");
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
                StopAllCoroutines();
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
        if (other.tag.Equals("Enemy"))
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
        mesh.materials[0].color = Color.yellow;
        //mesh.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        mesh.materials[0].color = Color.white;

        //mesh.GetComponent<MeshRenderer>().material.color = Color.white;

    }

    IEnumerator SkillCounter()
    {
        while (!isDie)
        {


            if (state == State.TRACE)
            {
                SkillAttack();
                anim.CrossFade("attack5");
            }

            yield return new WaitForSeconds(coolTime);

        }

    }

    void SkillAttack()
    {
        GameObject instantBullet = Instantiate(bossBullet, bulletPort.position, bulletPort.rotation);
        BossBullet bulletScript = instantBullet.GetComponent<BossBullet>();

        bulletPort.transform.LookAt(target);
        instantBullet.transform.LookAt(target);
        //bulletScript.Launch(target);
        AudioManager.instance.PlaySFX("Boss_Fire");

    }

    // 현재 호출이 안됨
    private Coroutine myCoroutine;
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Fire") && myCoroutine == default)
        {
            myCoroutine = StartCoroutine(Slow());
        }
    }

    public void RunSlowCoroutine()
    {
        if (myCoroutine == null)
        {
            Debug.Log("Call SlowCoroutine");
            myCoroutine = StartCoroutine(Slow());
        }
    }

    IEnumerator Slow()
    {
        Debug.Log("Slow Call");
        //FireFX fire = GetComponent<FireFX>();
        
        // 스피드 다운 상태
        isSpeedDown = true;
        speed *= (1 - speedReduce);
        Debug.Log("Boss speed: " + speed);

        // 대기
        yield return new WaitForSeconds(1.0f);

        // 스피드 복구
        speed /= (1 - speedReduce); // 이전 속도로 되돌리기 위해 나누기 연산을 사용합니다.
        Debug.Log("Boss speed: " + speed);
        isSpeedDown = false;

        myCoroutine = null;
    }



}
