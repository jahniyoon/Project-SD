using System;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [Header("Enemys")]
    private static GameObject[] enemys;

    private void Awake()
    {
        instance = this;
    }

    public static GameObject GetEnemyParent(int index)
    {
        return enemys[index];
    }

    // 일정 시간 후에 Enemy 오브젝트의 상태를 변경하는 함수
    public void ChangeActive(GameObject gameObject, float t, bool isActive)
    {
        // 코루틴 실행
        StartCoroutine(ChangeActiveCoroutine(gameObject, t, isActive));
    }

    // SetActive용 코루틴
    private IEnumerator ChangeActiveCoroutine(GameObject gameObject, float t, bool isActive)
    {
        // 대기
        yield return new WaitForSeconds(t);

        // 상태 변경
        gameObject.SetActive(isActive);
    }
}
