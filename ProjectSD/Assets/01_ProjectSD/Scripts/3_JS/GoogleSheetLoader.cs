using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleSheetLoader : MonoBehaviour
{
    [Header("GoogleAPI")]
    // 스프레드 시트 url에 있는 ID
    private const string spreadsheetId = "1zGWet3RC6xmqQTkGS31E8BQjEPTuwhIFT1LkVIJSJYI";
    // API 접근 KEY
    private const string apiKey = "AIzaSyCXNUN-D43FJGsu37TjJxRUDWD7wcF98CU";
    // 불러올 문서의 시트 이름 배열
    // 불러올 시트 이름을 넣어주세요!!!
    private string[] sheetNames =
    {
        "Minion_Table", "Minion_Spawn_Table", 
        "PC_Table", "Weapon_Table", "Unit_Weapon_Upgrade_Table", "Projectile_Table",
         "Shop_Item_Table", "Gold_Table",
         "Golem_Table", "Golem_Projectile_Table", "Golem_Weak_Table",
         "Unit_FireBomb", "Collision_Table", "Unit_Trap"
    };
    // 코루틴에서 데이터를 반환하고
    // 반환된 데이터를 저장하기 위한 콜백 변수
    private Action<string> callBack;
    // 모든 데이터가 데이터 매니저에 등록되었는지
    // 상태를 알려주는 변수
    public static bool isDone = false;

    private void Awake()
    {
        // 데이터 매니저를 설정하는 함수 호출
        SetDataManager();
    }

    // 데이터 매니저에 GoogleSheet 문서 데이터를
    // 저장하는 함수
    private void SetDataManager()
    {
        // sheetNames의 길이 만큼 순회
        for (int i = 0; i < sheetNames.Length; i++)
        {

            // 코루틴으로 구글 시트 데이터를 불러온다.
            // isCsvConert = true를 매개변수로 할당해서
            // Csv 데이터로 변환한다.
            StartCoroutine(GoogleSheetsReader.GetGoogleSheetsData(
                spreadsheetId, apiKey, sheetNames[i], true, data =>
                {
                    // callBack 변수에서 받은 data를
                    // CSVReader.NewReadCSVFile()에
                    // 매개변수로 보내 데이터 타입을 변경
                    Dictionary<string, List<string>> dataDictionary = 
                    CSVReader.NewReadCSVFile(data);

                    // dataDictionary를 데이터 매니저에 추가
                    DataManager.SetData(dataDictionary);
                }));
        }

        // 모든 데이터를 데이터 매니저에
        // 할당했다고 상태를 변경하는 코루틴 호출
        // 정확한 상태를 설정하기 위해 1초 대기
        StartCoroutine(WaitForChangeState(1f));
    }

    // isDone의 상태를 변경하는 코루틴 함수
    private IEnumerator WaitForChangeState(float t)
    {
        // 대기
        yield return new WaitForSeconds(t);

        // 로딩 완료 상태 변경
        isDone = true;
    }
}
