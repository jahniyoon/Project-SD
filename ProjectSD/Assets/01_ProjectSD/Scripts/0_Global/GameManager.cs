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
    public GameObject playerPrefab;
    public int gold;

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
        playerPrefab.SetActive(false);
        playerPrefab.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (isPCMODE)
        {
            GameObject PC = playerPrefab.transform.GetChild(0).gameObject;
            PC.GetComponent<CamRotate>().enabled = true;
            PC.transform.position = new Vector3(PC.transform.position.x, PC.transform.position.y+0.8f, PC.transform.position.z);
            Transform leftGun = playerPrefab.transform.GetComponent<PlayerShooter>().leftGun.transform;
            Transform rightGun = playerPrefab.transform.GetComponent<PlayerShooter>().rightGun.transform;
            leftGun.transform.position = new Vector3(-0.6f, 1.5f, 0.8f);
            rightGun.transform.position = new Vector3(0.5f, 1.5f, 0.8f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
