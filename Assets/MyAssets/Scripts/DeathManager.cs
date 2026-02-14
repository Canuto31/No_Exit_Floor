using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    
    public PlayerInput playerInput;

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        panel.SetActive(true);
        
        if (playerInput != null)
            playerInput.enabled = false;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
