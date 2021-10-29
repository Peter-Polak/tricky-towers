using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModesMenu : MonoBehaviour
{
    public void StartRaceGame()
    {
        SceneManager.LoadScene(2);
    }

    public void StartSurvivalGame()
    {
        SceneManager.LoadScene(3);
    }

    public void StartPuzzleGame()
    {
        SceneManager.LoadScene(4);
    }
}
