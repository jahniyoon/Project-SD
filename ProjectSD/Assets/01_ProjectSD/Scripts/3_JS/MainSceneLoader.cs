using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneLoader : MonoBehaviour
{
    public string sceneName = "JS_TestMain2"; // 로딩할 씬의 이름

    void Start()
    {
        // 3초 후에 LoadScene 함수를 호출하여 씬을 로딩합니다.
        Invoke("LoadSceneAfterDelay", 3f);
    }

    void LoadSceneAfterDelay()
    {
        // 지정된 이름의 씬을 로딩합니다.
        SceneManager.LoadScene(sceneName);
    }
}
