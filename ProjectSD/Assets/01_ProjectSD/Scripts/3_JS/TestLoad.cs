using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoad : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        if (GoogleSheetLoader.isDone)
        {
            int cost = (int)DataManager.GetData(20001, "Cost");
            Debug.Log($"[20001] cost = {cost}");
            cost = (int)DataManager.GetData(20002, "Cost");
            Debug.Log($"[20002] cost = {cost}");
            cost = (int)DataManager.GetData(20003, "Cost");
            Debug.Log($"[20003] cost = {cost}");

            gameObject.SetActive(false);
        }

        int cost2 = (int)DataManager.GetData(20001, "Cost");
    }

}
