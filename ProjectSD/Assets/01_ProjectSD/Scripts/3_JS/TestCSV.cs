using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestCSV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, List<string>> dataDictionary = default;
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Enemy");
        Debug.Log($"ID:{dataDictionary["ID"][0]}");

        DataManager.SetData(dataDictionary);
        //int value = (int)DataManager.GetData(3000, "HP");
        //Debug.Log(value);

        //Dictionary<string, string> table = default;
        //table = DataManager.GetData(2001);
        //Debug.Log($"ID:{table["ID"]} HP:{table["HP"]}");

        //int num = (int)DataManager.GetData(2000, "ID");
        //Debug.Log($"가져온 값 {num} 타입 {num.GetType()}");

        int count = DataManager.GetCount(2000);
        Debug.Log($"카운트 갯수 {count}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
