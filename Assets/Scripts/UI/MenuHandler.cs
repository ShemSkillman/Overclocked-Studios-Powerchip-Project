using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] Text loadingText;
    [SerializeField] GameObject levelSelectMenu;

    Coroutine inProgress;

    public void StartGame(string sceneName)
    {
        if(inProgress == null)
        {
            inProgress = StartCoroutine(LoadLevel(sceneName));
        }
    }

    IEnumerator LoadLevel(string sceneName)
    {
        levelSelectMenu.SetActive(false);

        while (background.color.a <= 1)
        {
            Color c = background.color;
            c.a += 0.01f;
            background.color = c;

            yield return new WaitForSeconds(0.01f);
        }

        loadingText.gameObject.SetActive(true);
        SceneManager.LoadScene(sceneName);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
