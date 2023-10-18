using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

public class JsonToCSVConverter
{
    private const char QUOTATION_MARK = '"';

    public static string ConvertJsonToCSV(string json)
    {
        Debug.Log($"받은json = {json}");
        // JSON 데이터를 파싱하여 배열로 변환
        var jsonArray = JsonParser(json);

        if (jsonArray == null)
        {
            Debug.Log("NULL");
            return string.Empty;
        }

        // CSV 문자열을 빌드하기 위한 StringBuilder 생성
        var csvBuilder = new StringBuilder();

        // 각 배열 요소를 CSV 행으로 변환
        foreach (var row in jsonArray)
        {
            // 각 배열 요소의 요소를 쉼표로 구분된 CSV 열로 변환
            var csvRow = string.Join(",", row);
            csvBuilder.AppendLine(csvRow);
        }

        return csvBuilder.ToString();
    }

    private static List<List<string>> JsonParser(string json)
    {
        try
        {
            // JSON 데이터 파싱
            var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            Debug.Log($"파싱된 데이터: {jsonObj}");

            // 디버그
            foreach(var item in jsonObj)
            {
                Debug.Log($"item: {item}");
            }

            Debug.Log($"jsonObj Value = {jsonObj["values"]}");

            string data = jsonObj["values"].ToString();

            Debug.Log($"data: {data}");

            if (jsonObj.ContainsKey("values"))
            {
                Debug.Log($"ConatinsKey True");
                //var csvRows = new List<List<string>>();

                //var jsonArray = jsonObj["values"] as string;

                string pattern = @"\[[^\]]*\]"; // 정규 표현식 패턴: "["로 시작하고 "]"로 끝나는 모든 내용을 가져옴

                MatchCollection matches = Regex.Matches(data, pattern);

                Debug.Log($"matches: {matches.Count}");

                string resultText = "";

                int index = 0;
                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        Debug.Log("Match True");
                        string text = match.ToString();
                        text = extractBetweenDelimiters(text);
                        Debug.Log($"text: {text}");
                        Debug.Log($"Value = {resultText}");
                        resultText += text + "\n";
                    }
                    else
                    {
                        Debug.Log("Match False");
                    }
                    index++;
                }

                Debug.Log($"resultText: {resultText}");

                //MatchCollection matches = Regex.Matches(input, pattern);


                //foreach (Match match in matches)
                //{
                //    if (match.Success)
                //    {
                //        // 정규 표현식에 일치하는 부분은 match.Groups[1]에 저장됨
                //        string valueInsideBrackets = match.Groups[1].Value;
                //        Debug.Log(valueInsideBrackets);
                //    }
                //}

                //Debug.Log("AA");
                //Debug.Log(jsonArray);

                // 각 배열 요소를 CSV 행으로 변환
                //foreach (var row in jsonArray)
                //{
                //Debug.Log($"row: {row}");
                //if (row is List<object> rowList)
                //{
                //    var csvRow = new List<string>();

                //    // 각 배열 요소의 요소를 문자열로 변환하여 추가
                //    foreach (var cell in rowList)
                //    {
                //        csvRow.Add(cell.ToString());
                //    }

                //    csvRows.Add(csvRow);
                //}
                //}

                //return csvRows;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"JSON parsing error: {ex.Message}");
        }

        return null;
    }

    private static string RemoveBracketsAndNewlines(string input)
    {
        string result = input.Replace("[", "").Replace("]", "").Replace("\n", "");
        return result;
    }

    //public static string RemoveSpaceLines(string input)
    //{
    //    string text = input.Replace(" ", "");
    //    return text;
    //}

    //// "와 " 사이에 있는 텍스트를 추출하는 함수
    //// ex) "텍스트"
    //public static string ExtractTextBetweenQuotes(string input)
    //{
    //    // " 2 개당 카운트 1번 설정
    //    int count = CountQuotes(input) / 2;
    //    string text = input;
    //    string resultText = "";
    //    //Debug.Log(CountQuotes(input));

    //    for(int i = 0; i < count; i++)
    //    {
    //        int startIndex = text.IndexOf(QUOTATION_MARK);
    //        // 시작 따옴표를 찾은 경우
    //        if (startIndex >= 0)
    //        {
    //            int endIndex = text.IndexOf(QUOTATION_MARK, startIndex + 1);
    //            int length = (endIndex - startIndex);
    //            // 시작 따옴표 이후에 끝 따옴표를 찾은 경우
    //            if (endIndex > startIndex)
    //            {
    //                // "와 " 사이의 텍스트를 추출
    //                string tempText = QUOTATION_MARK + input.Substring(startIndex + 1, endIndex - startIndex - 1)
    //                    + QUOTATION_MARK;
    //                // i 값이 마지막이 아닐 경우
    //                if (i != (count - 1))
    //                {
    //                    // , 쉼표 추가
    //                    tempText += ",";
    //                }
    //                // tesultText에 tempText 추가
    //                resultText += tempText;

    //                // text에 추가된 텍스트 제거
    //                text = RemoveSubstring(text, startIndex, length);

    //                Debug.Log($"tempText = {tempText}");

    //                Debug.Log($"text= {text}");

    //            }
    //        }

    //    }

    //    return resultText;
    //}

    // " 문자열의 갯수를 세는 함수
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

    //// 문자열의 특정 범위를 삭제하는 함수
    //private static string RemoveSubstring(string originalText, int startIndex,
    //    int length)
    //{
    //    // 매개 변수로 받은 범위 내 텍스트 제거
    //    string text = originalText.Remove(startIndex, length);

    //    return text;
    //}

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
                // 찾은 텍스트를 " + elements + " 로 변환
                text += QUOTATION_MARK + elements[targetOrder - 1]
                    + QUOTATION_MARK;
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