using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{

    public bool Level1Active = false;
    public bool Level2Active = false;

    public GameObject btnLevel1;
    public GameObject btnLevel2;

    // Start is called before the first frame update
    void Start()
    {        

       btnLevel1.SetActive(false);
       btnLevel2.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        //check if Level has been used
        if(Level1Active == true)
        {
            btnLevel1.SetActive(true);
        }

        if(Level2Active == true)
        {
            btnLevel2.SetActive(true);
        }
    }

    public void Level1()
    {
        SceneManager.LoadScene("Sprint 5");
    }

    public void Level2()
    {
        SceneManager.LoadScene("Sprint 5-New Level");
    }
}
