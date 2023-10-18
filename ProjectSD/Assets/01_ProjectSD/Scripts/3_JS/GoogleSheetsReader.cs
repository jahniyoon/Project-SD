using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetsReader : MonoBehaviour
{
    private string spreadsheetId = "1zGWet3RC6xmqQTkGS31E8BQjEPTuwhIFT1LkVIJSJYI";
    private string range = "Minion_Table"; // Change to the specific sheet name or range you want to access

    private string apiKey = "AIzaSyCXNUN-D43FJGsu37TjJxRUDWD7wcF98CU";

    private void Start()
    {
        StartCoroutine(GetGoogleSheetsData());
    }

    private IEnumerator GetGoogleSheetsData()
    {
        string url = $"https://sheets.googleapis.com/v4/spreadsheets/{spreadsheetId}/values/{range}?key={apiKey}";

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string data = www.downloadHandler.text;
                // Parse and process the data as needed (e.g., using JSON utility)
                Debug.Log(data);
                string csvData = JsonToCSVConverter.ConvertJsonToCSV(data);
                Debug.Log(csvData);
            }
        }
    }
}
