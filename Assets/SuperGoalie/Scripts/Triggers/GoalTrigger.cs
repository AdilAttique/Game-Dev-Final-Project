using System;
using System.Collections;
using Assets.SuperGoalie.Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

namespace Assets.SuperGoalie.Scripts.Triggers
{
    public class GoalTrigger : MonoBehaviour
    {
       // public Action OnCollidedWithBall;
        public GameObject soundManager;
        private SoundManager soundManagerScript;

        public GameObject gameManager;
        private GameManager gameManagerScript;

        public GameObject WinningPanel;

        public GameObject LoosingPanel;
        public UnityEngine.UI.Text Score;

        public GameObject goall;
        int _score = 0;
        public string temp;
        private void Start() 
        {
           if(PlayerPrefs.HasKey(temp))
           {
                Score.text = PlayerPrefs.GetString(temp);
                _score = int.Parse(Score.text);
           }
            soundManagerScript = soundManager.GetComponent<SoundManager>();
            gameManagerScript = gameManager.GetComponent<GameManager>();
        }
        private void OnTriggerEnter(Collider other)
        {
            //if tag is ball
            if(other.tag == "Ball")
            {
                
                //invoke that the wall has collided with the ball
                // Action temp = OnCollidedWithBall;
                // if (temp != null)
                //     temp.Invoke();

                // // disable
                _score++;
                Score.text = _score.ToString();
                soundManagerScript.PlayWhistle();
                soundManagerScript.PlayGoalScoredSound();
                gameManagerScript.animator.Play("Celebrate");
                PlayerPrefs.SetString(temp, Score.text);
                goall.SetActive(true);

                if(_score == 5)
                {
                    goall.SetActive(false);
                    Time.timeScale = 0f;
                    if(temp == "A")
                    {
                        WinningPanel.SetActive(true);
                    }
                    else
                    {
                        LoosingPanel.SetActive(true);
                    }
                }
                StartCoroutine(Reset());
                gameObject.GetComponent<BoxCollider>().enabled = false;
                
            }
        }

        private IEnumerator Reset ()
        {
           
            Debug.Log("resetting");
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("Main Scene");
        }

    }
}
