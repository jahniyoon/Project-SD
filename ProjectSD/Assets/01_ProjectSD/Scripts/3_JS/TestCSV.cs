using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCSV : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, List<string>> dataDictionary = default;
        dataDictionary = CSVReader.ReadCSVFile("CSVFiles/Enemy_Spawn");
        Debug.Log($"ID:{dataDictionary["ID"][0]}");

        //DataManager.SetData(dataDictionary);
        //string value = (string)DataManager.GetData(2000, "HP");
        //Debug.Log(value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
