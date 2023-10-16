using UnityEngine;

public class PlayerRangeDetection : MonoBehaviour
{
    public Transform player; // 플레이어 Transform 컴포넌트
    public float radius = 5f; // 반지름
    public int numberOfSlices = 6; // 180도 범위를 몇 개의 구간으로 쪼갤지 결정

    void Update()
    {
        Vector3 center = player.position;
        float sliceAngle = 180f / numberOfSlices;

        for (int i = 0; i < numberOfSlices; i++)
        {
            // 현재 구간의 시작 각도와 끝 각도 계산
            float startAngle = -90 + i * sliceAngle;
            float endAngle = -90 + (i + 1) * sliceAngle;

            // 시작 및 끝 각도로부터 방향 벡터를 얻음
            Vector3 startDirection = Quaternion.Euler(0, 0, -startAngle) * player.up;
            Vector3 endDirection = Quaternion.Euler(0, 0, -endAngle) * player.up;

            // 시작과 끝 지점 계산
            Vector3 startPosition = center + startDirection * radius;
            Vector3 endPosition = center + endDirection * radius;

            // 시작과 끝 지점을 디버그로 출력
            Debug.Log(i + " Start Position: " + startPosition);
            Debug.Log(i + " End Position: " + endPosition);

            // 원의 구간을 그리기
            Debug.DrawLine(center, startPosition, Color.red);
            Debug.DrawLine(center, endPosition, Color.red);
        }
    }
}
