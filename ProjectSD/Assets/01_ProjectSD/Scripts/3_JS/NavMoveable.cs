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

    // 회전용
    public float rotationSpeed = 360.0f; // 1초 동안 360도 회전
    private float rotationTimer = 0.0f; // 회전 타이머

    private void Start()
    {
        // 게임이 시작되면 게임 오브젝트에 부착된 NavMeshAgent 컴포넌트를 가져와서 저장
        agent = GetComponent<NavMeshAgent>();

        agent.speed = speed;
        //agent.ResetPath();
    }

    void Update()
    {
        if (isStop == true)
        {
            return;
        }

        if (agent.enabled == true)
        {
            // 공이 멈춰있지 않을 경우
            if (agent.isStopped == false)
            {
                // 공 회전 함수 호출
                RollingBall();
            }         
        }
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

    // 공을 회전하는 함수
    private void RollingBall()
    {
        // 회전 시간을 증가시킵니다.
        rotationTimer += Time.deltaTime;

        // 회전 타이머가 회전 시간을 넘어서면 0으로 리셋합니다.
        if (rotationTimer > 1.0f)
        {
            rotationTimer = 0.0f;
        }

        // 현재 공의 방향을 얻어옵니다.
        Vector3 currentDirection = transform.forward;

        // 회전 각도를 계산합니다. (0도에서 360도)
        float angle = Mathf.Lerp(0, 360, rotationTimer);

        // 공을 현재 방향을 기준으로 x축을 중심으로 회전합니다.
        Quaternion rotationX = Quaternion.Euler(angle, 0, 0);

        // 공을 목표 방향을 바라보게 합니다.
        transform.LookAt(target);

        // 두 개의 회전을 결합하여 공을 원하는 방향으로 회전시킵니다.
        transform.rotation *= rotationX;
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