using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace MenuAsset
{
    /*
     * Code by Kristopher Kath
     */


    public class MainMenu : MonoBehaviour
    {
        [Tooltip("Input the desired play game index. If left at default value then next build index will be loaded.")]
        [SerializeField] private int playGameIndex = -1;

        [Tooltip("Input the desired tutorial scene index. If left at default value then next build index will be loaded.")]
        [SerializeField] private int tutorialIndex = -1;

        [Tooltip("Input the desired credits scene index. If left at default value then next build index will be loaded.")]
        [SerializeField] private int creditsIndex = -1;

        [Tooltip("The first button selected to hover on when navigating to options menu.")]
        [SerializeField] private GameObject optionsFirstButton;

        public void PlayGame()
        {
            //load given scene value
            if (playGameIndex != -1)
            {
                SceneManager.LoadScene(playGameIndex);
            }
            //load next scene in build order
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public void LoadTutorial()
        {
            //load given scene value
            if (tutorialIndex != -1)
            {
                SceneManager.LoadScene(tutorialIndex);
            }
            //load next scene in build order
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public void OpenCredits()
        {
            //load given scene value
            if (creditsIndex != -1)
            {
                SceneManager.LoadScene(creditsIndex);
            }
            //load next scene in build order
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }

        public void QuitGame()
        {
            Debug.Log("Game Quit");
            Application.Quit();
        }

        //Navigates to options menu
        public void ToOptionsPage()
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(optionsFirstButton);
        }

    }
}