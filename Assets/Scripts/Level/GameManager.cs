using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private EntityHealth player;

    private void OnEnable()
    {
        player.OnDead += RestartScene;
    }

    private void OnDisable()
    {
        player.OnDead -= RestartScene;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}