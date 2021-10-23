using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReset : MonoBehaviour
{
    [Scene][SerializeField] private string scene;
    public KeyCode resetKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(resetKey))
        {
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }
}
