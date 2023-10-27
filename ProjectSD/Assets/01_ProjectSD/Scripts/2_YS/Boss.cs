using Google.GData.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Enemy;
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

    private GameObject stunParticle;


    [Header("데미지 이펙트")]
    public GameObject damageEffect;
    private TMP_Text bulletText;

    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE,
        STUN
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
        Invoke("SetRadius", 3f);

        anim.CrossFade("GolemWalk");
        anim["GolemWalk"].speed = 0.15f;

        StartCoroutine(CheckMonsterState());

        StartCoroutine(MonsterAction());

        StartCoroutine(SkillCounter());
    }

    public void GameOver()
    {
        state = State.DIE;
        anim.CrossFade("roar");
        anim["roar"].speed = 0.5f;
    }

    public void SetRadius()
    {
        agent.radius = 0.01f;

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
        while (!isDie && !GameManager.instance.isGameOver)
        {
            yield return new WaitForSeconds(0.1f);


            if (state == State.DIE) yield break;


            float distance = Vector3.Distance(target.position, boss.position);

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            else if (distance >= traceDist && state != State.STUN)     //추적 사정거리 외부에서 추적 시작
            {
                state = State.TRACE;
            }
            else if (hp <= 0)
            {
                //state = State.DIE;
                //GameManager.instance.GameClear();
                //StopAllCoroutines();
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

                    anim.CrossFade("GolemWalk");
                    anim["GolemWalk"].speed = 0.15f;

                    //AudioManager.instance.PlayLoopSound("walk_Boss");
                    //AudioManager.instance.SoundPitchPlay("walk_Boss", 0.15f);  //낮출수록 소리 늦어짐

                    //임의의 시간 후 투사체
                    //StartCoroutine(SkillCounter());


                    //anim.SetBool("IsWalk", true);


                    //anim.SetBool(hashAttack, false);
                    break;


                case State.ATTACK:
                    break;


                //사망
                case State.DIE:
                    //isDie = true;
                    ////추적 중지
                    //agent.isStopped = true;
                    //anim.CrossFade("death", 0.25f);

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

        if (hp <= 0 && !isDie)
        {
            isDie = true;
            state = State.DIE;
            //추적 중지
            agent.isStopped = true;
            anim.CrossFade("newDeath", 0.25f);
            GameManager.instance.GameClear();
            StopAllCoroutines();
            Destroy(gameObject, 4f);

        }
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
        while (!isDie)
        {


            if (state == State.TRACE)
            {
                SkillAttack();
                //anim.CrossFade("attack5");
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
            StartCoroutine("DamageColor");

        }
    }

    IEnumerator Slow()
    {
        Debug.Log("Slow Call");
        //FireFX fire = GetComponent<FireFX>();
        
        // 스피드 다운 상태
        isSpeedDown = true;
        //speed *= (1 - speedReduce);
        Debug.Log("Boss speed: " + speed);
        agent.speed *= (1- speedReduce);

        // 대기
        yield return new WaitForSeconds(1.0f);

        // 스피드 복구
        //speed /= (1 - speedReduce); // 이전 속도로 되돌리기 위해 나누기 연산을 사용합니다.
        Debug.Log("Boss speed: " + speed);
        agent.speed = speed;

        isSpeedDown = false;

        myCoroutine = null;
    }

    // 스턴 디버프를 주는 함수
    // t만큼 스턴을 지속한다.
    public void OnStun(float t)
    {
        Debug.Log("스턴이 켜졌다.");
        // enemyState가 스턴 상태가 아닐 경우
        if (state != State.STUN)
        {
            // 스턴 상태로 변경
            state = State.STUN;

            // 에이전트 네비게이션 정지
            agent.isStopped = true;

            // 애니메이션 idle로 변경
            anim.CrossFade("idle");

            // 스턴 파티클 생성 함수 호출
            CreateStunParticle();

            // 일정 시간 후에 스턴 해제
            StartCoroutine(DisableStun(t));
        }
    }

    // 스턴 파티클을 생성하는 함수
    private void CreateStunParticle()
    {
        // 스턴 파티클 인스턴스 생성
        stunParticle = Instantiate(
            Resources.Load<GameObject>("Prefabs/Stun_1"), gameObject.transform);

    }

    // 일정 시간 후에 스턴 디버프를 해제하는 코루틴 함수
    private IEnumerator DisableStun(float t)
    {
        // 대기
        yield return new WaitForSeconds(t);

        // 상태 변경
        state = State.TRACE;

        // 에이전트 네비게이션 정지 해제
        agent.isStopped = false;

        //state = State.TRACE;

        // 애니메이션 Walk로 변경
        anim.CrossFade("GolemWalk");
        anim["GolemWalk"].speed = 0.15f;

        // 스턴 파티클 삭제
        Destroy(stunParticle);
    }

    public void DamageEffect(float damage)
    {
        Vector3 effectPos = this.transform.position;
        Vector3 vDist = effectPos - GameManager.instance.PC.transform.position; // 이펙트와 플레이어의 거리
        Vector3 vDir = vDist.normalized;    // 이펙트와 플레이어의 방향
        Vector3 damageFXPos = vDir * 10;
        damageFXPos.y += GameManager.instance.playerHeight;

        GameObject damageFX =
             Instantiate(damageEffect, damageFXPos, Quaternion.identity);
        bulletText = damageFX.GetComponent<TextDisolve>().textObj;
        bulletText.text = string.Format("{0}", damage);
        damageFX.GetComponent<TextDisolve>().colorName = "red";
        damageFX.transform.forward = Camera.main.transform.forward;


    }

}
