using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGameSingeplayer()
    {
        SceneManager.LoadScene("SinglePlayer");
    }

    public void LoadGameCoop()
    {
        SceneManager.LoadScene("Co-op");
    }
}
