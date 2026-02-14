using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private bool paused;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;

        Debug.Log(paused ? "PAUSED" : "UNPAUSED");
        // тут UI паузы
    }
}
