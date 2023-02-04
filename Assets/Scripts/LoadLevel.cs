using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    public string level = "Level1";
    public void LoadLevelHandler()
    {
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}
