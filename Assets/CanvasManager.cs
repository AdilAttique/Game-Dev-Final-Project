using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Assets.SuperGoalie.Scripts.Managers
{
public class CanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainPanel;

    public GameObject howToPlay;

    public GameObject enterNames;

    public TMP_InputField TeamA;

    
    public TMP_InputField TeamB;

    void Start()
    {
        PlayerPrefs.DeleteAll();
        mainPanel.SetActive(true);
        howToPlay.SetActive(false);
        enterNames.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(howToPlay.activeSelf)
            {
                mainPanel.SetActive(true);
                howToPlay.SetActive(false);
                enterNames.SetActive(false);
            }
            else if(enterNames.activeSelf)
            {
                mainPanel.SetActive(false);
                howToPlay.SetActive(true);
                enterNames.SetActive(false);
            }
        }
    }

    public void proceedToHowToPlay()
    {
        mainPanel.SetActive(false);
        howToPlay.SetActive(true);
        enterNames.SetActive(false);
    }

    public void proceedToEnterNames()
    {
        mainPanel.SetActive(false);
        howToPlay.SetActive(false);
        enterNames.SetActive(true);
    }


    public void startGame()
    {
        PlayerPrefs.SetString("teamAName", TeamA.text);
        PlayerPrefs.SetString("teamBName", TeamB.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene("Main Scene");
    }
}
}