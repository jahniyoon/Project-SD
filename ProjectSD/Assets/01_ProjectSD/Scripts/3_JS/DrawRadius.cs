using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRadius
{
    private const int NUM_POINTS = 7; // 원을 나눌 점의 수
    private const float START_ANGLE = 0f; // 시작 각도

    // 원의 반지름 포지션을 저장하는 변수
    private List<Vector3> circlePositions = new List<Vector3>();

    // 함수를 호출하는 생성자
    public DrawRadius(float radius, Vector3 playerPos)
    {
        // 원을 그리는 함수 실행
        RunDrawRadius(radius, playerPos);
    }

    // 원을 그리는 함수
    private void RunDrawRadius(float radius, Vector3 playerPos)
    {
        for (int i = 0; i < NUM_POINTS; i++)
        {
            float angle = START_ANGLE + i * (180f / (NUM_POINTS - 1));
            float x = playerPos.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            float z = playerPos.z + radius * Mathf.Sin(Mathf.Deg2Rad * angle);
            float playerPosZ = playerPos.z * 2f;
            Vector3 point = new Vector3(x, playerPos.y, z + playerPosZ);

            // 각 꼭짓점의 위치 디버그 출력
            //Debug.Log("Point " + i + " position: " + point);

            // 반지름 포지션 추가
            circlePositions.Add(point);
        }
    }

    // 저장된 반지름 포지션 리스트를 반환하는 함수
    public List<Vector3> GetCirclePositions()
    {
        return circlePositions;
    }
}
