using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ExitLevel exitTrigger, bottomTrigger;

    [SerializeField] private string exitTriggerLoad, bottomTriggerLoad;
    [SerializeField] public bool enemyGoal, switchGoal;
    public int enemyCounter = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        //check win condition
        checkGoal();
    }

    private void checkGoal()
    {
        if(enemyGoal == true)
        {
            //if every enemy is killed 
            //if(enemie)
            {
                //enable exit trigger
                //play animation
            }
        }

        if(switchGoal == true)
        {
            //if every switch is activated
           // if()
            {
                //enable exit trigger
                //play animation
            }
        }
    }

    private void OnEnable()
    {
        exitTrigger.OnPlayerEnter += LoadNewLevel;
        bottomTrigger.OnPlayerEnter += ReturnToMainMenu;
        
    }

    private void OnDisable()
    {
        exitTrigger.OnPlayerEnter -= LoadNewLevel;
        bottomTrigger.OnPlayerEnter -= ReturnToMainMenu;
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene(bottomTriggerLoad);
    }

    private void LoadNewLevel()
    {
        SceneManager.LoadScene(exitTriggerLoad);
    }
}
