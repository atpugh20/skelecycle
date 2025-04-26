using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InterfaceController : MonoBehaviour {
    public GameObject GameOverScreen;
    public GameObject PauseScreen;
    public GameObject WinScreen;
    public GameObject Restart1;
    public GameObject Restart2;

    public GameObject Player;
    public PlayerController playerController;
    public Rigidbody2D PlayerRigidBody;
    public Animator PlayerAnim;

    private RigidbodyConstraints2D SavedConstraints;

    InputAction pauseAction;
    bool isPaused = false;
    bool isDead = false;
    int counter = 0;
    
    void Start() {
        pauseAction = InputSystem.actions.FindAction("Pause");
        SavedConstraints = PlayerRigidBody.constraints;
    }

    // Update is called once per frame
    void Update() {
        // Check for win
        if (playerController.WonGame && !isDead) {
            if (counter < 1) {
                counter++;
                EventSystem.current.SetSelectedGameObject(Restart2);
            }
            WinScreen.SetActive(true);
            Time.timeScale = 0;
        }

        // Check for death
        isDead = PlayerAnim.GetBool("DeathComplete");
        if (isDead) {
            if (counter < 1) {
                counter++;
                EventSystem.current.SetSelectedGameObject(Restart1);
            }
            GameOverScreen.SetActive(true); 
            Time.timeScale = 0;
        }

        // Check for pause
        if (pauseAction.WasPerformedThisFrame() && 
            !isDead && 
            !playerController.WonGame) 
            {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            PauseScreen.SetActive(isPaused);

            if (isPaused) {
                SavedConstraints = PlayerRigidBody.constraints;
                PlayerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
            } else {
                PlayerRigidBody.constraints = SavedConstraints;
            }
        } 
    }

    public bool GetIsPaused() { return isPaused; }

    public void ReloadScene() {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
