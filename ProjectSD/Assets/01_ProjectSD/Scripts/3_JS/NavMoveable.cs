using UnityEngine;
using UnityEngine.AI;   // 스크립트에서 내비게이션 시스템 기능을 사용하려면 AI 네임스페이스를 using 선언해야함

public class NavMoveable : MonoBehaviour
{
    // 길을 찾아서 이동할 에이전트
    NavMeshAgent agent;

    // 에이전트의 목적지
    [SerializeField]
    Transform target;

    public float speed = 5f;
    private bool isStop = false;

    private void Awake()
    {
        // 게임이 시작되면 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 가져와서 저장
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.speed = speed;
    }

    void FixedUpdate()
    {
        if (isStop == true)
        {
            return;
        }

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