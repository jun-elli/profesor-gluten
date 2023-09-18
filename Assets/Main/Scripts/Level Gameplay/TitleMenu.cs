using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class TitleMenu : MonoBehaviour
{
    [SerializeField] private int _startingScene;

    public void StartGame()
    {
        SceneManager.LoadScene(_startingScene);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
