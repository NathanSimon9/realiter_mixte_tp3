using UnityEngine;
using System.Collections;

public class AlarmLightManager : MonoBehaviour
{
    public static AlarmLightManager Instance;

    [Header("Configuration")]
    public Light[] alarmLights;
    public AudioSource audioSource;
    public AudioClip alarmSound;

    [Header("Paramètres Alarme Intense")]
    public float blinkSpeed = 4f;
    public float maxIntensityAlerte = 6f;
    public float minIntensityAlerte = 0f;

    [Range(1f, 3f)]
    public float boostLuminositeRouge = 1.8f; // Multiplicateur pour compenser la noirceur du rouge

    [Header("🔥 Nouveau : Boost de Portée")]
    public float boostRangeRouge = 1.1f; // +10% de portée pendant l'alarme

    private bool alarmActive = false;
    private int guardsDetectingPlayer = 0;

    private float[] originalIntensities;
    private float[] originalRanges; // On ajoute la sauvegarde du Range
    private Color[] originalColors;
    private Coroutine stopRoutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (alarmLights == null || alarmLights.Length == 0)
            alarmLights = GetComponentsInChildren<Light>();

        originalIntensities = new float[alarmLights.Length];
        originalRanges = new float[alarmLights.Length]; // Initialisation
        originalColors = new Color[alarmLights.Length];

        for (int i = 0; i < alarmLights.Length; i++)
        {
            originalIntensities[i] = alarmLights[i].intensity;
            originalRanges[i] = alarmLights[i].range; // Sauvegarde du range initial
            originalColors[i] = alarmLights[i].color;
        }
    }

    void Update()
    {
        if (!alarmActive) return;

        float t = Mathf.PingPong(Time.time * blinkSpeed, 1);

        // Intensité avec ton boost de 1.8
        float intensity = Mathf.Lerp(minIntensityAlerte, maxIntensityAlerte * boostLuminositeRouge, t);

        for (int i = 0; i < alarmLights.Length; i++)
        {
            alarmLights[i].color = Color.red;
            alarmLights[i].intensity = intensity;

            // 🔥 Application du boost de 10% sur la portée
            alarmLights[i].range = Mathf.Lerp(originalRanges[i], originalRanges[i] * boostRangeRouge, t);
        }
    }

    public void StartAlarm()
    {
        guardsDetectingPlayer++;
        if (alarmActive) return;
        if (stopRoutine != null) StopCoroutine(stopRoutine);
        alarmActive = true;

        if (audioSource != null && alarmSound != null)
        {
            audioSource.clip = alarmSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopAlarm()
    {
        guardsDetectingPlayer--;
        if (guardsDetectingPlayer <= 0)
        {
            guardsDetectingPlayer = 0;
            if (alarmActive)
            {
                alarmActive = false;
                if (audioSource != null) audioSource.Stop();
                if (stopRoutine != null) StopCoroutine(stopRoutine);
                stopRoutine = StartCoroutine(FadeToOriginal(2f));
            }
        }
    }

    private IEnumerator FadeToOriginal(float duration)
    {
        float elapsed = 0f;
        float[] intensitiesAtStop = new float[alarmLights.Length];
        float[] rangesAtStop = new float[alarmLights.Length]; // Sauvegarde range à l'arrêt

        for (int i = 0; i < alarmLights.Length; i++)
        {
            intensitiesAtStop[i] = alarmLights[i].intensity;
            rangesAtStop[i] = alarmLights[i].range;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float p = elapsed / duration;

            for (int i = 0; i < alarmLights.Length; i++)
            {
                alarmLights[i].color = Color.Lerp(Color.red, originalColors[i], p);
                alarmLights[i].intensity = Mathf.Lerp(intensitiesAtStop[i], originalIntensities[i], p);
                // Retour fluide du Range
                alarmLights[i].range = Mathf.Lerp(rangesAtStop[i], originalRanges[i], p);
            }
            yield return null;
        }

        for (int i = 0; i < alarmLights.Length; i++)
        {
            alarmLights[i].color = originalColors[i];
            alarmLights[i].intensity = originalIntensities[i];
            alarmLights[i].range = originalRanges[i];
        }
    }
}