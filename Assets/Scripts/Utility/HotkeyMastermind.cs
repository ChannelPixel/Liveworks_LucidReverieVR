using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class HotkeyMastermind : MonoBehaviour
{
    public GameObject Player;
    public WinSequenceManager winSequenceManager;
    public LoseSequenceManager loseSequenceManager;

    public KeyCode ResetScene = KeyCode.R;
    public KeyCode TriggerWin = KeyCode.W;
    public KeyCode TriggerLose = KeyCode.L;

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if(Input.GetKeyDown(ResetScene))
            {
                ReloadScene();
            }
            if(Input.GetKeyDown(TriggerWin))
            {
                winSequenceManager.TriggerWinSequence();
            }
            if(Input.GetKeyDown(TriggerLose))
            {
                loseSequenceManager.TriggerLoseSequence();
            }
        }
    }
    
    public void ReloadScene()
    {
        Destroy(Player);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Physics.gravity = new Vector3(0, -9.81f, 0);
    }
}
