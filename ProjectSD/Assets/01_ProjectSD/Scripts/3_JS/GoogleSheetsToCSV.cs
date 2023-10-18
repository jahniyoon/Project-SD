//using UnityEngine;
//using Google.Apis.Sheets.v4;
//using Google.Apis.Services;
//using Google.Apis.Util.Store;
//using Google.Apis.Auth.OAuth2;
//using Google.Apis.Sheets.v4.Data;
//using System.Collections.Generic;
//using System.IO;

//public class GoogleSheetsToCSV : MonoBehaviour
//{
//    public string jsonPath = "Assets/YourCredentials.json";
//    public string spreadsheetId = "your-spreadsheet-id";
//    public string csvFilePath = "Assets/OutputData.csv"; // CSV 파일 경로

//    void Start()
//    {
//        UserCredential credential;

//        using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
//        {
//            string credPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), ".credentials/sheets.googleapis.com-your-app-name");
//            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
//                GoogleClientSecrets.Load(stream).Secrets,
//                new[] { SheetsService.Scope.SpreadsheetsReadonly },
//                "user",
//                System.Threading.CancellationToken.None,
//                new FileDataStore(credPath, true)).Result;
//        }

//        var service = new SheetsService(new BaseClientService.Initializer()
//        {
//            HttpClientInitializer = credential,
//            ApplicationName = "YourAppName"
//        });

//        // 스프레드시트 데이터 가져오기
//        SpreadsheetsResource.ValuesResource.GetRequest request =
//            service.Spreadsheets.Values.Get(spreadsheetId, "A1:B2");
//        ValueRange response = request.Execute();
//        IList<IList<object>> values = response.Values;

//        if (values != null && values.Count > 0)
//        {
//            // CSV 파일로 데이터 저장
//            using (StreamWriter writer = new StreamWriter(csvFilePath))
//            {
//                foreach (var row in values)
//                {
//                    string rowString = string.Join(",", row);
//                    writer.WriteLine(rowString);
//                }
//            }

//            Debug.Log("Data saved to CSV file: " + csvFilePath);
//        }
//        else
//        {
//            Debug.Log("No data to save.");
//        }
//    }
//}
