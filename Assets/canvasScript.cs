using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class canvasScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Restart()
    {
        PlayerPrefs.SetString("A", "0");
        PlayerPrefs.SetString("B", "0");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Scene");
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Starting Scene");
    }
}
