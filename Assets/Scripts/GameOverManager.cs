using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    [SerializeField] int mainMenuSceneNumber = 0;

    public void QuitGame()
    {
        LoadLevel(mainMenuSceneNumber);

    }

    public void RetryLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }


    private void LoadLevel(int sceneNumber)
    {
        if (Time.timeScale <= 0 || SongManager.Instance.IsPaused)
        { SongManager.Instance.ToggleScaleAndPauseVar(false); }

        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
    }
}
