using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Text.RegularExpressions;

public class JsonToCSVConverter
{
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
            // 받아온 jsonObj 데이터가 정상이어서
            // "values" 키가 있을 경우
            if (jsonObj.ContainsKey("values"))
            {
                Debug.Log($"ConatinsKey True");
                //var csvRows = new List<List<string>>();

                //var jsonArray = jsonObj["values"] as string;

                string pattern = @"\[[^\]]*\]"; // 정규 표현식 패턴: "["로 시작하고 "]"로 끝나는 모든 내용을 가져옴

                MatchCollection matches = Regex.Matches(data, pattern);

                Debug.Log($"matches: {matches.Count}");

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        Debug.Log("Match True");
                        // 정규 표현식에 일치하는 부분은 match.Groups[1]에 저장됨
                        string valueInsideBrackets = match.Groups[1].Value;
                        string text = RemoveBracketsAndNewlines(match.ToString());
                        //text = RemoveSpaceLines(text);
                        Debug.Log($"{CountQuotes(text)} > {text}");
                        text = ExtractTextBetweenQuotes(text);
                        Debug.Log($"value: {text}");

                       
                    }
                    else
                    {
                        Debug.Log("Match False");
                    }
                }

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

    public static string RemoveSpaceLines(string input)
    {
        string text = input.Replace(" ", "");
        return text;
    }

    private const char QUOTATION_MARK = '"';
    // "와 " 사이에 있는 텍스트를 추출하는 함수
    // ex) "텍스트"
    public static string ExtractTextBetweenQuotes(string input)
    {
        // " 2 개당 카운트 1번 설정
        int count = CountQuotes(input) / 2;
        string text = "";
        //Debug.Log(CountQuotes(input));

        for(int i = 0; i < count; i++)
        {
            int startIndex = input.IndexOf(QUOTATION_MARK);
            // 시작 따옴표를 찾은 경우
            if (startIndex >= 0)
            {
                int endIndex = input.IndexOf(QUOTATION_MARK, startIndex + 1);

                // 시작 따옴표 이후에 끝 따옴표를 찾은 경우
                if (endIndex > startIndex)
                {
                    // "와 " 사이의 텍스트를 추출
                    string tempText = QUOTATION_MARK + input.Substring(startIndex + 1, endIndex - startIndex - 1)
                        + QUOTATION_MARK;
                    // i 값이 마지막이 아닐 경우
                    if (i != (count - 1))
                    {
                        // , 쉼표 추가
                        tempText += ",";
                    }
                    // text에 tempText 추가
                    text += tempText;
                }
            }

        }

        return text;
    }

    // " 문자열의 갯수를 세는 함수
    public static int CountQuotes(string input)
    {
        int count = 0;

        foreach (char c in input)
        {
            if (c == '"')
            {
                count++;
            }
        }

        return count;
    }
}