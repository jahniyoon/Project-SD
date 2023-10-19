using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

public class JsonToCsvConverter
{
    // " 문자 상수
    private const char QUOTATION_MARK = '"';

    // JSON 문자열을 CSV 형식 문자열로 변경해주는 함수
    public static string ConvertJsonToCsv(string json)
    {
        // JSON 데이터를 파싱하여 CSV 형식으로 변환
        string csvData = JsonParser(json);

        // 변환된 csvData가 null 값일 경우
        if (csvData == null)
        {
            Debug.Log("ConvertJsonToCsv(): 출력 결과 NULL, json 문자열을 확인해주세요.");

            // 빈 문자열 반환
            return string.Empty;
        }

        return csvData;
    }

    private static string JsonParser(string json)
    {
        // JSON 데이터 파싱
        var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        // 결과 반환용 텍스트 선언
        string resultText = "";

        // JSON 데이터 파싱이 올바르게 되어서
        // jsonObj에 values키가 있을 경우
        if (jsonObj.ContainsKey("values"))
        {
            // JSON내에 있는 values 데이터 추출
            string data = jsonObj["values"].ToString();

            // 정규 표현식 패턴: "["로 시작하고 "]"로 끝나는 모든 내용을 가져옴
            string pattern = @"\[[^\]]*\]";

            // pattern에 일치하는 모든 문자열을 객체로 저장
            // 리스트 같은 느낌이라고 보면 된다.
            MatchCollection matches = Regex.Matches(data, pattern);

            // foreach에 사용할 index 선언
            int index = 0;
            // matches 내에 있는 모든 객체 수만큼 순회
            foreach (Match match in matches)
            {
                // match에 성공했을 경우
                // 패턴에 일치하는 문자열을 찾았을 경우
                if (match.Success)
                {
                    //Debug.Log("Match True");

                    // match를 string으로 형변환
                    string text = match.ToString();

                    // 반복되는 특정 문자(")의 안에 있는 문자열들을
                    // 찾아서 하나로 합친 후 리턴하는 함수 호출
                    text = extractBetweenDelimiters(text);

                    // 행 구분을 위해 문자열 "\n" 추가
                    resultText += text + "\n";
                }
                // match에 실패했을 경우
                else
                {
                    Debug.Log("Match False");
                }
                index++;
            }
        }

        // 결과 반환
        return resultText;
    }

    // "(QUOTATION_MARK) 문자열의 갯수를 세는 함수
    public static int CountQuotes(string input)
    {
        int count = 0;

        foreach (char c in input)
        {
            if (c == QUOTATION_MARK)
            {
                count++;
            }
        }

        return count;
    }

    // 반복되는 특정 문자(")의 안에 있는 문자열들을
    // 찾아서 하나로 합친 후 리턴하는 함수
    // Split() 기본 값으로 QUOTATION_MARK 사용중.
    private static string extractBetweenDelimiters(string input)
    {
        // (")이 들어있는 문자열을 자름
        string[] elements = input.Split(QUOTATION_MARK);

        string text = "";
        int count = CountQuotes(input) / 2;
        for (int i = 0; i < count; i++)
        {
            // targetOrder의 경우 2의 배수마다 인덱스 1로
            // 인식한다. [0] = 2, [1] = 4, [2] = 6 이런식이다.
            int targetOrder = (i + 1) * 2;
            if (targetOrder >= 1 && targetOrder <= elements.Length)
            {
                // 찾은 텍스트를 text에 추가
                // 따로 "를 추가할 필요가 없다.
                // ex) "elements" 이런식으로 추가 하면 오류나기 때문
                text += elements[targetOrder - 1];
            }
            else                             
            {
                Console.WriteLine("문자열을 찾지 못했습니다. 올바른 인덱스 값 혹은 문자열을 " +
                    "입력해주세요.");
            }

            // i 값이 마지막이 아닐 경우
            if (i != (count - 1))
            {
                // , 쉼표 추가
                text += ",";
            }
        }

        return text;
    }
}