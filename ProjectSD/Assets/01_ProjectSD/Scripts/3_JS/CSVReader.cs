using System.Collections.Generic;
using UnityEngine;

public class CSVReader
{
    private const char DELIMITER = ','; // CSV 파일에서 사용하는 구분자 (기본값은 콤마)

    // CSV 파일을 읽어와서 딕셔너리로 반환하는 함수
    // 반환될 딕셔너리는 Dictionary<string, List<string>> 이다.
    // csvFileName에 "CSVFiles"와 같은 Resources 안에 있는 디렉토리명과
    // WheelList.csv"과 같이 csv 파일의 이름을 입력한다.
    // ReadCSVFile(CSVFiles/WheelList)와 같은 형태로 CSV파일을 불러온다.
    public static Dictionary<string, List<string>> ReadCSVFile(string csvFileName)
    {
        // csv 파일의 정보를 행과 열로 구분하여 저장할 딕셔너리
        // 행은 키 값이 되고, 열은 키 값 내부의 값이 된다.
        Dictionary<string, List<string>> dataDictionary =
            new Dictionary<string, List<string>>();
        // 매개 변수로 받아온 파일 이름으로 파일을 불러온다.
        TextAsset filePath = Resources.Load<TextAsset>(csvFileName);

        Debug.LogFormat("filePath -> {0}", filePath.text);
        // 디버그용 변수
        bool isCSVReadSuccessful = false;
        // CSV 파일을 가져왔을 경우
        if (filePath != null)
        {
            {
                string[] lines = filePath.text.Split('\n'); // 줄 바꿈으로 행 구분을 위해 추가

                // lines의 길이가 1 이상일 경우
                if (lines.Length > 0)
                {
                    string[] headers = lines[0].Split(DELIMITER); // 문자열을 ',' 기준으로 자름

                    // CSV 파일의 첫 번째 라인(행)을 foreach로 순회
                    foreach (string header in headers)
                    {
                        // dataDictionary에 행 이름을 키 값으로 리스트 추가
                        // Trim() 함수를 사용하여 .csv 파일을 읽어올 때 생기는 공백을 제거
                        dataDictionary.Add(header.Trim(), new List<string>());
                    }

                    // 첫번째 행[0]을 헤더로 사용하고 두 번째[1] 부터 데이터 행으로 사용하기 위해
                    // index를 1 부터 시작

                    int count = lines.Length;
                    for (int i = 1; i < count; i++)
                    {
                        string line = lines[i];
                        // 엑셀로 작업할 경우 공백이 생겨 빈 데이터로
                        // 새로 줄이 생기는 현상이 있어
                        // 공백이 생길 경우 break 하도록 설정
                        if (line == "") { break; }
 
                        string[] values = line.Split(DELIMITER);

                        for (int j = 0; j < values.Length; j++)
                        {
                            // 헤더(행) 리스트에 값 추가
                            dataDictionary[headers[j].Trim()].Add(values[j]);
                        }
                    }
                }
            }

            // 디버그용 bool 값 변경
            isCSVReadSuccessful = true;
        }

        // 가져 왔을 경우
        if (isCSVReadSuccessful)
        {
            Debug.Log($"ReadCSVFile(): ▶ 경로 {csvFileName} ▶ CSV 파일 로드 성공");
        }
        // 가져오지 못했을 경우
        else
        {
            Debug.Log($"ReadCSVFile(): ▶ 경로 {csvFileName} ▶ CSV 파일 로드 실패 ▶ " +
                $"일치하는 CSV 파일이 없습니다. ▶ 스크립트: CSVReader");
        }

        return dataDictionary;
    }

    // ReadCSVFile() 함수의 스프레드 시트 버전
    public static Dictionary<string, List<string>> NewReadCSVFile(string csvData)
    {
        // csv 파일의 정보를 행과 열로 구분하여 저장할 딕셔너리
        // 행은 키 값이 되고, 열은 키 값 내부의 값이 된다.
        Dictionary<string, List<string>> dataDictionary =
            new Dictionary<string, List<string>>();

        string[] lines = csvData.Split('\n'); // 줄 바꿈으로 행 구분을 위해 추가

        // lines의 길이가 1 이상일 경우
        if (lines.Length > 0)
        {
            string[] headers = lines[0].Split(DELIMITER); // 문자열을 ',' 기준으로 자름

            // CSV 파일의 첫 번째 라인(행)을 foreach로 순회
            foreach (string header in headers)
            {
                // dataDictionary에 행 이름을 키 값으로 리스트 추가
                // Trim() 함수를 사용하여 .csv 파일을 읽어올 때 생기는 공백을 제거
                dataDictionary.Add(header.Trim(), new List<string>());
            }

            // 첫번째 행[0]을 헤더로 사용하고 두 번째[1] 부터 데이터 행으로 사용하기 위해
            // index를 1 부터 시작

            int count = lines.Length;
            for (int i = 1; i < count; i++)
            {
                string line = lines[i];
                // 엑셀로 작업할 경우 공백이 생겨 빈 데이터로
                // 새로 줄이 생기는 현상이 있어
                // 공백이 생길 경우 break 하도록 설정
                if (line == "") { break; }

                string[] values = line.Split(DELIMITER);

                for (int j = 0; j < values.Length; j++)
                {
                    // 헤더(행) 리스트에 값 추가
                    dataDictionary[headers[j].Trim()].Add(values[j]);
                }
            }
        }

        return dataDictionary;
    }
}