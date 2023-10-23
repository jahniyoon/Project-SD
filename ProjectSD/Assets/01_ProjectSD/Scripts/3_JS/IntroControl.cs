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
        "평화롭던 어느 날… 세계수를 노리고 거대 골렘이 다가오기 시작했다. \n" +
            "골렘을 막을 수 있는 이는 단 한 사람.",
        "그는 바로...",
        "돌펜스! 돌펜스!",
    };
    private float delay = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator PrintDialogueCoroutine(float t)
    {
        // 대기
        yield return new WaitForSeconds(t);
    }
}
