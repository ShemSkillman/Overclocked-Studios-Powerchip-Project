using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ExitLevel exitTrigger, bottomTrigger;

    [SerializeField] private string exitTriggerLoad, bottomTriggerLoad;
    [SerializeField] public bool enemyGoal, switchGoal;
    [SerializeField] public Animation doorOpenAnim;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] AudioClip doorOpenAudio;
    private bool openDoor = false;

    
    private static int enemyCounter = 0;
    public static int EnemyCounter { get {  return enemyCounter; } set { enemyCounter = value; } }

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

            if(enemyCounter == 0)
            {
                if(!openDoor)
                {
                    openDoor = true;

                    audioSource.clip = doorOpenAudio;
                    audioSource.Play();
                    doorOpenAnim.Play();
                }                            
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
