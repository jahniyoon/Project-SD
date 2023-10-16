using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Choi")]
    public Transform player;
    public Transform bossMonster;
    // 플레이어와 보스의 시작 거리를 저장
    private float startDistance;
    // 플레이어와 보스의 현재 거리를 저장
    private float currentDistance;
    // Spawn CSV파일의 초기 ID
    private const int DEFAULT_ID = 3000;
    // ID를 저장하는 리스트
    private List<int> spawnIDs;

    private void Awake()
    {
        // Enemy_Spawn CSV파일을 불러온 후
        // DataManager에 Init 한다.
        Dictionary<string, List<string>> spawnData = 
            CSVReader.ReadCSVFile("CSVFiles/Enemy_Spawn");
        DataManager.SetData(spawnData);

        // 플레이어 / 보스의 초기 거리를 저장
        startDistance = Vector3.Distance(
            player.position, bossMonster.position);

        // 스폰 조건 거리를 받아옴
        int count = 0;
        int id = DEFAULT_ID; // enemy_Spawn의 초기 ID
        //int count = DataManager.GetCount(3000);
        // id들을 저장
        for (int i = 0; i < count; i++)
        {
            spawnIDs.Add(id);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //GetDistanceAtoB(playerStartPosition, bossStartPosition);
    }

    // Update is called once per frame
    void Update()
    {
        GetDistanceAtoB(player.position, bossMonster.position);
    }

    // 플레이어(a)와 보스(b)의 현재 거리를 계산하는 함수
    private void GetDistanceAtoB(Vector3 a, Vector3 b)
    {
        // 현재 거리를 저장하는 변수에 거리 값 저장
        currentDistance = Vector3.Distance(a, b);

        // 디버그용 출력
        Debug.Log(currentDistance);
    }

}
