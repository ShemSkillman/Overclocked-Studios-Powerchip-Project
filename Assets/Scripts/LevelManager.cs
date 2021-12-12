using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private ExitLevel exitTrigger, bottomTrigger;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        exitTrigger.OnPlayerEnter += ReturnToMainMenu;
        bottomTrigger.OnPlayerEnter += ReturnToMainMenu;
    }

    private void OnDisable()
    {
        exitTrigger.OnPlayerEnter -= ReturnToMainMenu;
        bottomTrigger.OnPlayerEnter -= ReturnToMainMenu;
    }

    private void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
