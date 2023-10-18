using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//! [DataManager의 사용법]
//! 0. CSVFile을 가져온다.
//! CSVReader.ReadCSVFile(CSVFiles/CSV파일명); 
//! *what? CSVFiles = CSV 파일이 저장된 디렉토리
//! *what? CSV파일명 = 가져올 CSV 파일명, 확장자는 생략한다.
//! *what? 반환타입 = Dictionary<string, List<string>> 이며,
//! 가져올 변수를 같은 타입으로 선언해야 한다.
//!
//! 1. DataManager에 데이터를 보관한다.
//! DataManager.SetData(데이터);
//! *what? 데이터 = 불러온 CSV파일
//!
//! 2. DataManager에 저장된 데이터를 불러온다.
//! 2-1. 하나의 데이터 값만 가져올 때
//! DataManager.GetData(아이디, 카테고리);
//! 값을 받을 때 int num = (int)DataManager.GetData();
//! 위와 같은 형태로 맞는 데이터 타입으로 형변환 해야 한다.
//! *what? 아이디 = CSV파일 참조
//! *what? 카테고리 = CSV파일의 행 참조
//!
//! 2-2. 아이디에 해당하는 줄의 모든 데이터를 가져올 떄
//! DataManager.GetData(아이디);
//! *what? 반환타입 = Dictionary<string, string> 이며,
//! 가져올 변수를 같은 타입으로 선언해야 한다.
//!
//! 아이디가 들어있는 CSV파일의 카운트를 가져온다.
//! DataManager.GetCount(아이디);
public static class DataManager
{
    [Header("Choi")]
    // 데이터를 보관하는 변수
    private static Dictionary<int, Dictionary<string, List<string>>>
        dataTable = new Dictionary<int, Dictionary<string, List<string>>>();
    // dataTable에 ID로 접근하기 위해
    // ID에 해당하는 인덱스를 보관하는 변수
    // idTable[ID][DATA_KEY] = dataTable에 저장된 순서
    // idTable[ID][DATA_INDEX] = 실제 데이터의 인덱스
    private static Dictionary<int, List<int>> 
        idTable = new Dictionary<int, List<int>>();
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
        // GoogleSheetLoader에서 모든 데이터를
        // 불러왔을 경우
        if (GoogleSheetLoader.isDone)
        {
            // dataTable을 검색하는 함수 호출
            object data = FindDataTable(id, category);

            // 데이터 타입을 가져오는 함수 호출
            string type = GetDataType((string)data);

            // 데이터 타입에 따라 형변환 하는 함수 호출
            data = ConvertDataType(type, (string)data);

            return data;
        }

        Debug.LogWarning("GetData(): GoogleSheetLoader에서 모든 데이터가" +
            " 로딩되지 않았습니다. GoogleSheetLoader.isDone = false");
        return null;
    }

    // 매개 변수에 id만 넣을 경우 Dictionary<string, string>로 반환한다.
    public static Dictionary<string, string> GetData(
        int id)
    {
        // dataTable을 검색하는 함수 호출
        Dictionary<string, string> temp_DataTable = FindDataTable(id);

        return temp_DataTable;
    }

    // 아이디를 매개변수로 받고 아이디가 들어 있는 CSV파일의
    // 열 갯수를 가져오는 함수.
    public static int GetCount(int id)
    {
        // 키 값과 키 값을 사용해 카운트를 가져온다.
        int key = GetDataKey(id);
        int count = dataTable[key][ID_HEADER].Count;

        // 찾은 카운트 반환
        return count;
    }

    // 아이디가 위치한 실제 인덱스를 가져오는 함수
    public static int GetIndex(int id)
    {
        // 아이디의 실제 인덱스를 가져오는 함수 호출
        id = GetDataKey(id);

        return id;
    }
    #endregion

    #region [내부 메서드]
    // ID 값을 idTable에 저장하는 함수
    private static void SetIDTable(Dictionary<string, List<string>> data)
    {
        // dataTable의 길이 - 1 를 딕셔너리 접근 인덱스로 설정
        int index = dataTable.Count - 1;

        // data[ID_HEADER]의 길이 만큼 순회
        for (int i = 0; i < data[ID_HEADER].Count; i++)
        {
            int id = int.Parse(data[ID_HEADER][i]);
            int index2 = i;
            // idTable에 있는 기존 ID와 현재 ID가 중복되었을 경우
            if (idTable.ContainsKey(id))
            {
                Debug.LogWarning($"SetIDTable(): 등록하는 ID: [{id}]는 " +
                    $"기존에 있는 ID와 중복됩니다. CSV 파일의 ID 값을 변경하시거나, " +
                    $"CSV파일이 중복으로 SetData()를 호출하는지 확인해주세요.");
            }
            // 딕셔너리의 키값으로 ID를 설정하고, 내부 List에 실제 인덱스를 저장한다.
            // index는 딕셔너리의 위치, index2는 실제 데이터 열이 저장된 위치
            idTable.Add(id, new List<int>());
            idTable[id].Add(index); // [0] 딕셔너리의 위치
            idTable[id].Add(index2); // [1] 실제 데이터 열이 저장된 위치
        }
    }

    // dataTable를 검색하는 함수
    private static object FindDataTable(int id, string category)
    {
        object data = default;

        // idTable에 정상적으로 접근했을 경우
        if (idTable.ContainsKey(id))
        {
            // dataTable에서 데이터를 찾아서 반환한다.
            int key = idTable[id][DATA_KEY];
            int index = idTable[id][DATA_INDEX];
            data = dataTable[key][category][index];
        }

        // 접근하지 못했을 경우
        else
        {
            // 디버그 메세지 출력
            Debug.LogWarning($"FindDataTable({id}, {category}): 데이터를 찾지 못했습니다. " +
                $"ID와 Category를 확인해 주세요.");
        }

        return data;
    }

    // 매개 변수에 id만 넣을 경우 Dictionary<string, string>로 반환한다.
    private static Dictionary<string, string> FindDataTable(int id)
    {
        Dictionary<string, string> temp_DataTable = new Dictionary<string, string>();
        // idTable에 정상적으로 접근했을 경우
        if (idTable.ContainsKey(id))
        {
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

            // ID에 해당하는 실제 데이터의 인덱스를 가져옴
            int index = idTable[id][DATA_INDEX];
            // count 만큼 순회
            for (int i = 0; i < count; i++)
            {
                // temp_DataTable에 dataTable에 있는 모든 카테고리 & 값 등록
                string category = categorys[i];
                string data = dataTable[key][category][index];
                temp_DataTable.Add(category, data);
            }
        }

        else
        // 정상적으로 접근하지 못했을 경우
        {
            // 디버그 메세지 출력
            Debug.LogWarning($"FindDataTable({id}): 데이터를 찾지 못했습니다. " +
                $"ID를 확인해 주세요.");
        }

        // temp_DataTable 반환
        return temp_DataTable;
    }

    // string으로 저장된 data 값의 데이터 타입을
    // 조건식을 통해 찾아내는 함수
    public static string GetDataType(string data)
    {
        // 데이터 타입이 int 일 경우
        if (int.TryParse(data, out int tempIntValue))
        {
            // "int" 반환
            return "int";
        }

        // 데이터 타입이 float 일 경우
        else if (float.TryParse(data, out float tempFloatValue))
        {
            // "float" 반환
            return "float";
        }

        // 데이터 타입이 bool 일 경우
        else if (bool.TryParse(data, out bool tempBoolValue))
        {
            // "bool" 반환
            return "bool";
        }

        // 모두 일치 하지 않을 경우 "string" 반환
        return "string";
    }

    // 데이터 타입을 형변환 해주는 함수
    private static object ConvertDataType(string type, string data)
    {
        // 받아온 type에 따라 데이터 타입을 형변환
        switch (type)
        {
            // "int"일 경우
            case "int":
                return int.Parse(data);

            // "float"일 경우
            case "float":
                return float.Parse(data);

            // "bool"일 경우
            case "bool":
                return bool.Parse(data);

            // "string"일 경우
            default:
                return data;
        }
    }

    // 아이디로 해당 아이디가 위치한 CSV파일의 위치를 받아오는 함수
    private static int GetDataKey(int id)
    {
        int index = idTable[id][DATA_KEY];
        
        // 찾은 위치 인덱스 반환
        return index;
    }
    #endregion
}
