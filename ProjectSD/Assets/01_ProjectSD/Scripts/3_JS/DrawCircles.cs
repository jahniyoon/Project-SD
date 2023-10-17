using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircles : MonoBehaviour
{
    public GameObject player; // 플레이어 오브젝트
    public int numPoints = 6; // 원을 나눌 점의 수
    public float circleRadius = 100f; // 원의 반지름
    public float startAngle = 0f; // 시작 각도
    public Material lineMaterial; // 라인 렌더러에 사용할 머티리얼
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.positionCount = numPoints;
    }

    void Update()
    {
        DrawCircle();
    }

    void DrawCircle()
    {
        for (int i = 0; i < numPoints; i++)
        {
            float angle = startAngle + i * (180f / (numPoints - 1));
            float x = player.transform.position.x + circleRadius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float z = player.transform.position.z + circleRadius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float playerPosZ = player.transform.position.z * 2;
            Vector3 point = new Vector3(x, player.transform.position.y, z + playerPosZ);
            lineRenderer.SetPosition(i, point);

            // 각 꼭짓점의 위치 디버그 출력
            Debug.Log("Point " + i + " position: " + point);
        }
    }
}
