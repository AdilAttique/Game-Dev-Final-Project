using System;
using System.Collections;
using Assets.SuperGoalie.Scripts.Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

namespace Assets.SuperGoalie.Scripts.Triggers
{
public class TouchlineScript : MonoBehaviour
{
    // Start is called before the first frame update
        public GameObject soundManager;
        private SoundManager soundManagerScript;
    void Start()
    {
        soundManagerScript = soundManager.GetComponent<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
        {
            Debug.Log("Ball Out of Play!");
            soundManagerScript.PlayWhistle();
            StartCoroutine(Reset());
        }
    }
            private IEnumerator Reset ()
        {
           
            Debug.Log("resetting");
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("Main Scene");
        }
}
}