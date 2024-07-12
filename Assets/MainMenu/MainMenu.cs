using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public int PlayLevel;

    public void StartGame()
    {
        SceneManager.LoadScene(PlayLevel);
    }
}
