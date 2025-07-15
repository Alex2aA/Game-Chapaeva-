using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnRandomMap()
    {
        int randomMap = Random.Range(1, 3);
        SceneManager.LoadScene(randomMap);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
