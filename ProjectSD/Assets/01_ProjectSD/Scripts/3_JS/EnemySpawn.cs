using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("EnemySpawn")]
    public Transform player;
    public Transform bossMonster;
    // 플레이어의 초기 포지션
    private Vector3 playerPos;
    // 플레이어와 보스의 시작 거리를 저장
    private float startDistance;
    // 플레이어와 보스의 현재 거리를 저장
    private float currentDistance;
    // Spawn CSV파일의 초기 ID
    private const int DEFAULT_ID = 6001;
    // ID / 스폰 조건 / 범위 / 각도 /
    // enemy 타입 / 스폰 갯수 리스트
    private List<int> spawnIDs = new List<int>();
    private List<float> spawnConditions = new List<float>();
    private List<float> spawnLocations1 = new List<float>();
    private List<float> spawnLocations2 = new List<float>();
    private List<int> angles = new List<int>();
    private List<int> minionTypes1 = new List<int>();
    private List<int> minionTypes2 = new List<int>();
    private List<int> type1Amounts = new List<int>();
    private List<int> type2Amounts = new List<int>();
    // 중복 스폰 방지용 스폰 카운트
    private int spawnCount = 0;

    [Header("Test")]
    public GameObject[] testObj_1;
    public GameObject[] testObj_2;
    public GameObject[] testMonster;

    private void Awake()
    {
        // LEGACY:
        //// Enemy_Spawn CSV파일을 불러온 후
        //// DataManager에 Init 한다.
        //Dictionary<string, List<string>> spawnData = 
        //    CSVReader.ReadCSVFile("CSVFiles/Minion_Spawn_Table");
        //DataManager.SetData(spawnData);

        //// Enemy CSV파일을 불러온 후
        //// DataManager에 Init 한다.
        //Dictionary<string, List<string>> enemyData =
        //    CSVReader.ReadCSVFile("CSVFiles/Enemy");
        //DataManager.SetData(enemyData);  

        // 플레이어 / 보스의 초기 거리를 저장
        startDistance = Vector3.Distance(
            player.position, bossMonster.position);

        // 플레이어 초기 포지션 저장
        playerPos = player.position;

        //DataManager에 있는 Enemy_Spawn 데이터를 가져와서
        // 리스트에 추가하는 함수를 호출
        Initialize();
    }

    private void FixedUpdate()
    {
        // 플레이어(a)와 보스(b)의 현재 거리를 계산하는 함수 호출
        GetDistanceAtoB(player.position, bossMonster.position);

        // 스폰 조건에 일치하는지 확인하는 함수 호출
        CheckSpawnCondition();
    }

    // DataManager에 있는 Enemy_Spawn 데이터를 가져와서
    // 리스트에 추가하는 함수
    private void Initialize()
    {
        int count = DataManager.GetCount(DEFAULT_ID);
        for (int i = 0; i < count; i++)
        {
            // 리스트에 id에 해당하는 데이터 저장
            int id = DEFAULT_ID + i;
            spawnIDs.Add(id);
            spawnConditions.Add((float)DataManager.GetData(id, "Spawn_Condition"));
            float spawnLocation1 = (int)DataManager.GetData(id, "Spawn_Location1");
            float spawnLocation2 = (int)DataManager.GetData(id, "Spawn_Location2");
            spawnLocation1 = spawnLocation1 / 100f;
            spawnLocation2 = spawnLocation2 / 100f;
            spawnLocations1.Add(spawnLocation1);
            spawnLocations2.Add(spawnLocation2);
            //Debug.Log($"spawnLocation1[{i}] = {spawnLocation1}, " +
            //    $"spawnLocatipn2[{i}] = {spawnLocation2}");
            angles.Add((int)DataManager.GetData(id, "Angle"));
            minionTypes1.Add((int)DataManager.GetData(id, "Minion_Type1"));
            minionTypes2.Add((int)DataManager.GetData(id, "Minion_Type2"));
            type1Amounts.Add((int)DataManager.GetData(id, "Type1_Amount"));
            type2Amounts.Add((int)DataManager.GetData(id, "Type2_Amount"));
        }
    }

    // Enemy 인스턴스를 생성하는 함수
    private void SpawnEnemyInstance(GameObject prefab,
        Vector3 position, Quaternion rotate)
    {
        Instantiate(prefab, position, rotate);
    }

    // 생성시 부모 추가 오버로드
    private void SpawnEnemyInstance(GameObject prefab,
        Vector3 position, Quaternion rotate, GameObject parent)
    {
        Instantiate(prefab, position, rotate).
            transform.SetParent(parent.transform);
    }

    // 부모를 생성하는 함수
    private GameObject SpawnParentInstance(string name)
    {
        // 부모 생성
        GameObject parent = new GameObject(name);

        return parent;
    }

    // Enemys 프리팹 디렉토리 경로
    private const string DIRECTORY = "Prefabs/";
    // id로 Resources 폴더에 있는 프리팹을
    // 찾아서 반환하는 함수
    private GameObject GetResourcesPrefab(int id)
    {
        string name = (string)DataManager.GetData(id, "Model_Info");
        GameObject prefab = 
            Resources.Load<GameObject>(DIRECTORY + name);
        
        return prefab;
    }

    // 플레이어(a)와 보스(b)의 현재 거리를 계산하는 함수
    private void GetDistanceAtoB(Vector3 a, Vector3 b)
    {
        // 현재 거리를 저장하는 변수에 거리 값 저장
        // 시작 거리와 현재 거리를 나눈다.
        currentDistance = (Vector3.Distance(a, b) / startDistance);

        // 디버그용 출력
        //Debug.Log(currentDistance);
    }

    // 플레이어와 보스의 거리가 스폰 조건에 일치하는지 계산하는 함수
    private void CheckSpawnCondition()
    {
        // 스폰 조건의 갯수 만큼 순회
        // 중복 스폰 방지를 위해 시작 값을 spawnCount로 한다.
        for (int i = spawnCount; i < spawnIDs.Count; i++)
        {
            // 현재 거리가 스폰 조건 거리 이하일 경우
            if (currentDistance <= spawnConditions[i])
            {
                Debug.Log($"소환{i}");
                SpawnRandomMonsterInArea(spawnCount);
                spawnCount++;
                break;
            }
        }
    }

    // enemy를 지정한 범위 내에서 생성되게 하는 함수
    private void SpawnRandomMonsterInArea(int index)
    {
        // 최소 크기 반지름을 생성(ex:0.7 * startDistance(초기 보스&플레이어 거리)
        float radius = startDistance * spawnLocations2[index];
        //Debug.Log($"startDistance: {startDistance}, spawnLocation2: {spawnLocations2[index]}");
        //Debug.Log($"min radius:{radius}");
        DrawRadius drawMinRadius = new DrawRadius(radius, playerPos);

        // 최대 크기 반지름을 생성(ex:90)
        radius = startDistance * spawnLocations1[index];
        //Debug.Log($"max radius:{radius}");
        DrawRadius drawMaxRadius = new DrawRadius(radius, playerPos);

        // 각 반지름의 스폰 포지션을 받아옴
        List<Vector3> spawnMinPositions = drawMinRadius.GetCirclePositions();
        List<Vector3> spawnMaxPositions = drawMaxRadius.GetCirclePositions();

        //// 디버그용
        //// 테스트용 포지션 변경
        //for (int i = 0; i < testObj_1.Length; i++)
        //{
        //    testObj_2[i].transform.position = spawnMinPositions[i];
        //    testObj_1[i].transform.position = spawnMaxPositions[i];
        //}

        // Enemy_Spawn_Table에 있는 Enemy들을 생성하는 함수 호출
        SpawnEnemys(spawnCount, spawnMinPositions, spawnMaxPositions);
    }

    // 오리지널: 180도 / 30 = 6
    private const int SPAWN_AREA = 6;
    // Enemy_Spawn_Table에 있는 Enemy들을 생성하는 함수
    private void SpawnEnemys(int index, List<Vector3> spawnMinPositions,
        List<Vector3> spawnMaxPositions)
    {
        GameObject parent = SpawnParentInstance("Enemy_SpawnPhase_" + index);
        // 하드코딩으로 스폰조건 변경
        // 기존 180도 범위에서 120도로 조정
        // count만큼 순회
        int count = SPAWN_AREA;
        // 위의 하드코딩으로 120도로 범위를 조절하기 위해
        // 시작 i 값을 1로 하고 count -1를 하였다.
        for (int i = 1; i < count - 1; i++)
        {
            int posIndex = i;
            int posIndex2 = i + 1;
            Vector3 point0Min = spawnMinPositions[posIndex];
            Vector3 point0Max = spawnMinPositions[posIndex2];
            Vector3 point1Min = spawnMaxPositions[posIndex];
            Vector3 point1Max = spawnMaxPositions[posIndex2];

            int countType1 = type1Amounts[index];
            for (int j = 0; j < countType1; j++)
            {
                // Minion_Type1에 있는 아이디를 가져옴
                int id = minionTypes1[index];

                // Enemy 인스턴스를 생성하는 함수 호출
                SpawnEnemyInstance(GetResourcesPrefab(id),
                    GetRandomPositionInAreas(point0Min, point0Max, point1Min, point1Max),
                    Quaternion.identity, parent);
            }

            int countType2 = type2Amounts[index];
            for (int k = 0; k < countType2; k++)
            {
                // Minion_Type2에 있는 아이디를 가져옴
                int id = minionTypes2[index];

                // Enemy 인스턴스를 생성하는 함수 호출
                SpawnEnemyInstance(GetResourcesPrefab(id),
                    GetRandomPositionInAreas(point0Min, point0Max, point1Min, point1Max),
                    Quaternion.identity, parent);
            }
        }

        // enemy 생성 후 소리 출력
        AudioManager.instance.PlaySFX("SpawnSound");

        Debug.Log("적들 생성");
    }

    // 매개변수로 받은 4개의 포지션(점) 범위 내에서 x좌표에 따라
    // z좌표의 랜덤 범위를 조정하여 네개의 점 사이에서 벗어나지 않게
    // 랜덤하게 포지션을 계산해주는 함수
    // GPT 형님이 도와주심
    private Vector3 GetRandomPositionInAreas(Vector3 point0Min, Vector3 point0Max, Vector3 point1Min, Vector3 point1Max)
    {
        // 무작위로 x 좌표를 선택합니다.
        float randomX = Random.Range(point0Min.x, point1Max.x);

        // 해당 x 좌표에 따라 z 좌표의 범위를 계산합니다.
        float minZ = Mathf.Lerp(point0Min.z, point1Min.z, (randomX - point0Min.x) / (point1Max.x - point0Min.x));
        float maxZ = Mathf.Lerp(point0Max.z, point1Max.z, (randomX - point0Min.x) / (point1Max.x - point0Min.x));

        // 무작위로 z 좌표를 선택합니다.
        float randomZ = Random.Range(minZ, maxZ);

        // y 값을 고르거나 평균값을 사용할 수 있습니다.
        float randomY = (point0Min.y + point0Max.y + point1Min.y + point1Max.y) / 4f;

        // 랜덤한 위치를 생성합니다.
        Vector3 randomPos = new Vector3(randomX, randomY, randomZ);

        // 계산된 랜덤 포지션 반환
        return randomPos;
    }
}
