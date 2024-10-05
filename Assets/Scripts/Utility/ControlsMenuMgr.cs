using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject _player1Ready;
    [SerializeField] GameObject _player2Ready;

    bool[] _players;

    private void Awake()
    {
        _player1Ready.SetActive(false);
        _player2Ready.SetActive(false);

        _players = new bool[2];
        for (int i = 0; i < _players.Length; i++)
        {
            _players[i] = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A)
            || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            _players[0] = true;
            _player1Ready.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.J)
            || Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.L))
        {
            _players[1] = true;
            _player2Ready.SetActive(true);
        }

        bool startGame = true;
        foreach (bool player in _players)
        {
            if (!player)
            {
                startGame = false;
            }
        }
        if (startGame)
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        GoToNextLevel();
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
