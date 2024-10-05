using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMgr : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GoToNextLevel();
        }
    }
    public void GoToNextLevel()
    {
        int nextBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextBuildIndex >= SceneManager.sceneCountInBuildSettings) nextBuildIndex = 0;
        GoToLevel(nextBuildIndex);
    }

    public void GoToLevel(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
