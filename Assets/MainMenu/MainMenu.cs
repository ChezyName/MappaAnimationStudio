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
