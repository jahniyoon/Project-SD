using UnityEngine;
using UnityEngine.AI;   // 스크립트에서 내비게이션 시스템 기능을 사용하려면 AI 네임스페이스를 using 선언해야함

public class NavMoveable : MonoBehaviour
{
    // 길을 찾아서 이동할 에이전트
    NavMeshAgent agent;

    // 에이전트의 목적지
    [SerializeField]
    public Transform target;

    public float speed = 5f;
    private bool isStop = false;

    [Header("GooRooGi")]
    public float rollForce = 100.0f; // 구르기 힘 세기
    private Rigidbody rb;


    private void Start()
    {
        // 게임이 시작되면 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 가져와서 저장
        agent = GetComponent<NavMeshAgent>();

        rb = GetComponent<Rigidbody>();

        agent.speed = speed;
        //agent.ResetPath();
    }

    void FixedUpdate()
    {
        if (isStop == true)
        {
            return;
        }

        // agent가 NavMesh 상에 위치 할 경우
        if (agent.isOnNavMesh)
        {
            if (GameManager.instance.isGameOver)
            {
                // 에이전트 정지
                agent.isStopped = true;
                isStop = true;
            }
            else
            {
                // 에이전트에게 목적지를 알려주는 함수
                agent.SetDestination(target.position);
            }
        }
    }

    private void Update()
    {
        if (agent.velocity.magnitude > 0.1f)
        {
            // 네비게이션 방향으로 구르기 힘을 적용
            Vector3 rollDirection = agent.velocity.normalized;
            rb.AddForce(rollDirection * rollForce);
        }
    }

    // agent의 스피드를 변경하는 함수
    public void ChangeSpeed(float value)
    {
        speed = value;
        agent.speed = speed;
    }

    // agent의 이동 여부를 토글하는 함수
    public void ToggleMoveable(bool isMove)
    {
        agent.isStopped = isMove;
    }
}