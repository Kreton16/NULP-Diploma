using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [Header("Бажаний ліміт кадрів")]
    [Tooltip("Для якісного відео зазвичай ставлять 30 або 60")]
    public int targetFPS = 30;

    void Start()
    {
        // ВАЖЛИВО: Вимикаємо V-Sync, інакше Unity проігнорує наш ліміт
        // і буде підлаштовуватись під частоту оновлення екрану/шолома
        QualitySettings.vSyncCount = 0;

        // Встановлюємо жорсткий ліміт кадрів
        Application.targetFrameRate = targetFPS;

        Debug.Log($"[FPS Limiter] Штучно зафіксовано на рівні {targetFPS} кадрів на секунду.");
    }
}