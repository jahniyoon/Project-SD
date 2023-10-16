using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }
    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    [Header("Player")]
    public GameObject PC;
    public int gold;
    public GameObject shop;

    [Header("Title")]
    public GameObject titlePanel;

    [Header("GameOver")]
    public GameObject gameOverPanel;
    public bool isGameOver;

    [Header("Debug")]
    public bool isPCMODE;

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
    public void Retry()
    {
        PC.SetActive(false);
        PC.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (isPCMODE)
        {
            PC.transform.GetChild(0).GetComponent<CamRotate>().enabled = true;
            PC.transform.position = new Vector3(PC.transform.position.x, PC.transform.position.y, PC.transform.position.z);

            Transform leftHand = PC.transform.GetComponent<PlayerHand>().LeftHand.transform;
            Transform rightHand = PC.transform.GetComponent<PlayerHand>().RightHand.transform;

            leftHand.transform.position = PC.transform.GetComponent<PlayerHand>().LeftPosition.position;
            leftHand.transform.rotation = PC.transform.GetComponent<PlayerHand>().LeftPosition.rotation;
            rightHand.transform.position = PC.transform.GetComponent<PlayerHand>().RightPosition.position;
            leftHand.transform.rotation = PC.transform.GetComponent<PlayerHand>().RightPosition.rotation;

           
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameStart();
        }
    }

    public void GameStart()
    {
        Debug.Log("게임 시작");
        titlePanel.SetActive(false);

        PC.transform.GetComponent<PlayerShooter>().enabled = true;
        PC.transform.GetComponent<PlayerShop>().enabled = true;

        PC.transform.GetComponent<PlayerHand>().LeftHand.gameObject.SetActive(false);
        PC.transform.GetComponent<PlayerHand>().RightHand.gameObject.SetActive(false);

        Transform leftGun = PC.transform.GetComponent<PlayerShooter>().leftGun.transform;
        Transform rightGun = PC.transform.GetComponent<PlayerShooter>().rightGun.transform;
        leftGun.gameObject.SetActive(true);
        rightGun.gameObject.SetActive(true);

        leftGun.transform.position = PC.transform.GetComponent<PlayerHand>().LeftPosition.position;
        leftGun.transform.rotation = PC.transform.GetComponent<PlayerHand>().LeftPosition.rotation;
        rightGun.transform.position = PC.transform.GetComponent<PlayerHand>().RightPosition.position;
        rightGun.transform.rotation = PC.transform.GetComponent<PlayerHand>().RightPosition.rotation;
    }
 
   
}
