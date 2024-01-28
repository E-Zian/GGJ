using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pauseMenu;
    public static bool isPaused = false;
    void OnEnable()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else {
                Pause();
            }
        }
    }
    private void Pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }
    public void Resume() {
        
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void returnMainMenu() {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void quit()
    {
        Application.Quit();
    }
}
