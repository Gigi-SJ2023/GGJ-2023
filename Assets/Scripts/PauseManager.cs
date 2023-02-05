using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject startMenu;
    public UnityEvent onGameStart;

    private void Start()
    {
        onGameStart?.Invoke();
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }
    
    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }
    
    public void CloseStartMenu()
    {
        startMenu.SetActive(false);
    }

    public void PauseGame ()
    {
        Time.timeScale = 0f;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
    }
}
