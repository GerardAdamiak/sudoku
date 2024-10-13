using UnityEngine;
using System.Collections;

public class SceneLoaded : MonoBehaviour
{
    public static bool sceneReady = false;

    void Start()
    {
        Debug.Log("scene laded started");
        // Simulate some loading task
        StartCoroutine(SetSceneReady());
        
    }

    IEnumerator SetSceneReady()
    {
        Debug.Log("scene ready");
        yield return new WaitForSeconds(2); // Simulate some loading time
        sceneReady = true;
        
    }
}
