using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinMgr : MonoBehaviour
{
    [SerializeField] GameObject player01;
    [SerializeField] GameObject player02;
    [SerializeField] TextMeshProUGUI winnerText;

    private void Start()
    {
        player01.SetActive(false);
        player02.SetActive(false);

        int player = PlayerPrefs.GetInt("PlayerDied");
        if (player == 0)
        {
            player02.SetActive(true);
            winnerText.text = "PLAYER 2 WINS!";
        }
        else if (player == 1)
        {
            player01.SetActive(true);
            winnerText.text = "PLAYER 1 WINS!";
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GoToNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("MainScene");
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
