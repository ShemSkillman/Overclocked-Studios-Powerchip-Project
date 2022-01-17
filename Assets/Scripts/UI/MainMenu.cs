using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject startGameMenu;
    [SerializeField] GameObject levelSelectMenu;

    public void GoToLevelSelect()
    {
        startGameMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
