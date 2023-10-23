using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroControl : MonoBehaviour
{
    [Header("Intro")]
    public TextMeshProUGUI introText;
    private string[] dialogue =
    {
        "평화롭던 어느 날… 세계수를 노리고 거대 골렘이 \n 다가오기 시작했다.",
        "골렘을 막을 수 있는 이는 단 한 사람.",
        "그는 바로...",
        "돌펜스! 돌펜스!",
        ""
    };
    private float[] delay =
    {
        7.0f, 4.0f, 3.0f, 2.0f, 2.0f
    };

    // Start is called before the first frame update
    void Start()
    {
        // 메세지 출력 코루틴 실행
        StartCoroutine(PrintDialogueCoroutine(delay));
    }

    // 다이얼로그 메세지를 출력하는 코루틴 함수
    private IEnumerator PrintDialogueCoroutine(float[] t)
    {
        for (int i = 0; i < t.Length; i++)
        {
            // 대기
            yield return new WaitForSeconds(t[i]);

            // 텍스트 변경
            introText.text = dialogue[i];
        }

    }
}
