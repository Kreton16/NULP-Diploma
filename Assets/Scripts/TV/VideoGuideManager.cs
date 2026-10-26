using UnityEngine;
using UnityEngine.Video;

public class VideoGuideManager : MonoBehaviour
{

    [Header("--- Налаштування Відео ---")]
    public VideoPlayer videoPlayer;
    public GameObject videoPanel; // UI панель або об'єкт, де рендериться відео

    public void PlayTutorialVideo()
    {
        videoPanel.SetActive(true);
        videoPlayer.Play();
    }

    public void CloseVideoAndStartTask()
    {
        videoPlayer.Stop();
        videoPanel.SetActive(false);
    }

}
