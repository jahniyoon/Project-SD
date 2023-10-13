using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    [Header("Choi")]
    // 데이터를 보관하는 변수
    private static Dictionary<int, Dictionary<string, List<string>>>
        dataTable = new Dictionary<int, Dictionary<string, List<string>>>();
    // dataTable에 ID로 접근하기 위해
    // ID에 해당하는 인덱스를 보관하는 변수
    // idTable[ID][DATA_KEY] = dataTable에 저장된 순서
    // idTable[ID][DATA_INDEX] = 실제 데이터의 인덱스
    private static Dictionary<int, List<int>> idTable = default;
    // dataTable의 고정 상수
    private const string ID_HEADER = "ID";
    private const int DATA_KEY = 0;
    private const int DATA_INDEX = 1;

    #region [외부 메서드]
    // CSV Reader로 불러온 Dictionary<string, List<string>를
    // dataTable에 데이터를 저장하는 함수
    public static void SetData(Dictionary<string, List<string>> data)
    {
        // dataTable에 값 추가
        int index = dataTable.Count;
        dataTable.Add(index, data);

        // ID 값을 idTable에 저장하는 함수 호출
        SetIDTable(data);
    }

    // dataTable에 저장된 데이터를 가져오는 함수
    // 기본 반환 값은 string이다.
    public static object GetData(int id, string category)
    {
        // dataTable을 검색하는 함수 호출
        object data = FindDataTable(id, category);

        return data;
    }

    // 매개 변수에 id만 넣을 경우 Dictionary<string, string>로 반환한다.
    public static Dictionary<string, string> GetData(
        int id, string category = default, bool isMultiple = false)
    {
        // dataTable을 검색하는 함수 호출
        Dictionary<string, string> temp_DataTable = FindDataTable(id);

        return temp_DataTable;
    }

    #endregion

    #region [내부 메서드]
    // ID 값을 idTable에 저장하는 함수
    private static void SetIDTable(Dictionary<string, List<string>> data)
    {
        Debug.Log("SetIDTable호출");
        // dataTable의 길이 - 1 를 딕셔너리 접근 인덱스로 설정
        int index = dataTable.Count - 1;
        Debug.Log($"카운트{data["ID"][0]}");
        // data[ID_HEADER]의 길이 만큼 순회
        for (int i = 0; i < data[ID_HEADER].Count; i++)
        {
            int id = int.Parse(data[ID_HEADER][i]);
            int index2 = i;
            // 딕셔너리의 키값으로 ID를 설정하고, 내부 List에 실제 인덱스를 저장한다.
            // index는 딕셔너리의 위치, index2는 실제 데이터 열이 저장된 위치
            idTable.Add(id, new List<int>());
            idTable[id].Add(index);
            idTable[id].Add(index2);
            Debug.Log(idTable);
        }
    }

    // dataTable를 검색하는 함수
    private static string FindDataTable(int id, string category)
    {
        // dataTable에서 데이터를 찾아서 반환한다.
        int key = idTable[id][DATA_KEY];
        int index = idTable[id][DATA_INDEX];
        string data = dataTable[key][category][index];

        return data;
    }

    // 매개 변수에 id만 넣을 경우 Dictionary<string, string>로 반환한다.
    private static Dictionary<string, string> FindDataTable(int id)
    {
        Dictionary<string, string> temp_DataTable = new Dictionary<string, string>();
        int key = idTable[id][DATA_KEY];
        int count = dataTable[key].Count;
        string[] categorys = new string[count];
        int temp_Index = 0;
        // dataTable[key]를 모두 순회
        foreach (var pair in dataTable[key])
        {
            // 키 값 저장
            categorys[temp_Index] = pair.Key;
            temp_Index++;
        }

        // count 만큼 순회
        for (int i = 0; i < count; i++)
        {
            // temp_DataTable에 dataTable에 있는 모든 카테고리 & 값 등록
            int index = i;
            string category = categorys[i];
            string data = dataTable[key][category][index];
            temp_DataTable.Add(category, data);
        }

        // temp_DataTable 반환
        return temp_DataTable;
    }

    // string으로 저장된 data 값의 데이터 타입을
    // 조건식을 통해 찾아내는 함수
    //private static string ConvertDataType(string data)
    //{
    //    //if (int.TryParse(Key))
    //}
    #endregion
}
