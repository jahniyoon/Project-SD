using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{


    public string sceneName = default; // 로딩할 씬의 이름

    void Start()
    {
        LoadSceneAfterDelay();
    }

    void LoadSceneAfterDelay()
    {
        // 지정된 이름의 씬을 로딩합니다.
        SceneManager.LoadScene(sceneName);
    }


}
