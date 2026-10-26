using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class SceneReloader : MonoBehaviour
{

    public DynamicMoveProvider moveProvider; // посилання на скрипт руху
    public float speedChange = 0.5f;

    public void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void LaunchSceneOne()
    {
        SceneManager.LoadScene("ScenarioLAN");
    }

    public void BackToHub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void ExitScene()
    {
        Application.Quit();
    }

    public void ChangeSpeed()
    {
        if (moveProvider != null)
        {
            moveProvider.moveSpeed += speedChange;
            Debug.Log("Speed changed to: " + moveProvider.moveSpeed);

        }

    }
}