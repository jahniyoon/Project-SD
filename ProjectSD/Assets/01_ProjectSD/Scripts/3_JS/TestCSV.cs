using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCSV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, List<string>> dataDictionary;
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Enemy");
        Debug.Log($"ID:{dataDictionary["ID"].Count}");
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Enemy_Spawn");
        Debug.Log($"ID:{dataDictionary["Description"][0]}");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
