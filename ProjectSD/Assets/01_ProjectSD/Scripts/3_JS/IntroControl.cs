using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class IntroControl : MonoBehaviour
{
    [Header("Intro")]
    public Image introScreen;
    public TextMeshProUGUI[] introTexts;
    private string[] dialogues =
    {
        "평화롭던 어느 날…",
        "세계수를 노리고 거대 골렘이 다가오기 시작했다.",
        "골렘을 막을 수 있는 유일한 이는 바로.",
        "“대마법사 돌펜스”",
        "위대한",
        " 대마법사 ",
        "돌펜스여,\n", // 여기서 보이스 출력
        "골렘을 ",
        "막고 ",  // 텍스트 합침
        "세계를 ",
        "구하여라!", // 텍스트 합침
        ""
    };
    private float[] delays =
    {
        1.5f, 3.5f, 3.0f, 2.5f, 2.1f, 0.9f, 0.9f, 0.8f, 0.7f, 0.7f, 0.8f, 1.0f
    };
    private float[] textDelays =
    {
        1.0f, 3.0f, 2.5f, 2.0f, 1.5f, 0.8f, 0.9f, 0.8f, 0.7f, 0.7f, 0.8f, 1.0f
    };
    private int introPhase;

    [Header("Player")]
    public Transform player;
    // 플레이어 초기 포지션 y 위치
    private float defaultPosY;
    // 인트로 재생시 플레이어 포지션 y 위치
    private float introPosY = 18.0f;

    public void PlayIntro()
    {
        // 초기 플레이어 y 위치 기억
        defaultPosY = player.position.y;

        // 플레이어 y 위치 이동
        MovePositionY(introPosY, player.position);

        // 3.0초간 인트로 스크린 페이드인
        introScreen.DOFade(1f, 3.0f);

        // 메세지 출력 코루틴 실행
        StartCoroutine(PrintDialogueCoroutine(delays));
    }

    private void Update()
    {
        // 보이스 재생
        if (introPhase == 4)
        {
            introPhase = 0;

            // 돌펜스 보이스 효과음 재생
            AudioManager.instance.PlaySFX("Voice");
        }

        // 인트로 종료
        if (introPhase == (delays.Length - 1))
        {
            introPhase = 0;

            // 3.0초간 인트로 스크린 페이드 아웃
            introScreen.DOFade(0f, 2.0f);

            Action myAction = () => GameManager.instance.GameStart();
            Action myAction2 = () => MovePositionY(defaultPosY, player.position);
            Action myAction3 = () => Destroy(gameObject);

            // 2.0초 뒤에 게임 스타트
            StartCoroutine(WaitForRunFunction(2.0f, myAction));
            // 2.0초 뒤에 플레이어 y 위치 이동
            StartCoroutine(WaitForRunFunction(2.0f, myAction2));
            // 3.0초 뒤에 게임 오브젝트 삭제
            StartCoroutine(WaitForRunFunction(3.0f, myAction3));
        }
    }

    // 포지션 y를 이동하는 함수
    private void MovePositionY(float posY, Vector3 pos)
    {
        // y 위치 이동
        pos.y = posY;
        player.position = pos;
    }

    // 다이얼로그 메세지를 출력하는 코루틴 함수
    private IEnumerator PrintDialogueCoroutine(float[] t)
    {
        for (int i = 0; i < t.Length; i++)
        {
            // 대기
            yield return new WaitForSeconds(t[i]);

            // 텍스트 변경
            for (int j = 0; j < introTexts.Length; j++)
            {

                if (!(i >= 5) || (i == (t.Length - 1)))
                {
                    introTexts[j].text = default;
                }    
                // t.Length 범위를 벗어나지 않는 선에서 i+1을 함
                //Mathf.Clamp(i + 1, 1, t.Length);
                //Mathf.Min(i + 1, t.Length);
                introTexts[j].DOText(introTexts[j].text + dialogues[i], textDelays[i]);
                introPhase = i;
            }
        }
    }

    // 일정 시간 후에 액션을 실행하는 코루틴 함수
    private IEnumerator WaitForRunFunction(float t, Action func)
    {
        // 대기
        yield return new WaitForSeconds(t);

        // 액션 함수 실행
        func();
    }
}
