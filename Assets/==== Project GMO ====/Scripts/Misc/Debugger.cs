using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Debugger : MonoBehaviour
{
    [Scene][SerializeField] private string scene;

    public void Start()
    {
        
    }

    IEnumerator LoadMap()
    {
        Debug.Log("Loading Scenes");

        yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            SceneManager.UnloadSceneAsync(scene);
            StartCoroutine(LoadMap());
        }
    }
}
