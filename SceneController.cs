using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadFirstLevel()
    {
        SceneManager.LoadScene("FirstLevel");
    }

    public void LoadSecondLevel()
    {
        SceneManager.LoadScene("SecondLevel");
    }

    public void LoadThirdLevel()
    {
        SceneManager.LoadScene("ABC");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
