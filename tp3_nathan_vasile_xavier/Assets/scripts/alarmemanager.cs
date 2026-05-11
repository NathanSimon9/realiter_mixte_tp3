using UnityEngine;

public class AlarmLightManager : MonoBehaviour
{
    public static AlarmLightManager Instance;

    [Header("Configuration")]
    public Light[] alarmLights;
    public AudioSource audioSource;
    public AudioClip alarmSound;

    [Header("Paramètres Clignotement")]
    public float blinkSpeed = 6f;
    public float minIntensity = 0f;
    public float maxIntensity = 6f;

    private bool alarmActive = false;
    private int guardsDetectingPlayer = 0;

    // Tableaux pour stocker l'état initial
    private float[] originalIntensities;
    private Color[] originalColors;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (alarmLights == null || alarmLights.Length == 0)
        {
            alarmLights = GetComponentsInChildren<Light>();
        }

        // On sauvegarde TOUT : intensité ET couleur
        originalIntensities = new float[alarmLights.Length];
        originalColors = new Color[alarmLights.Length];

        for (int i = 0; i < alarmLights.Length; i++)
        {
            originalIntensities[i] = alarmLights[i].intensity;
            originalColors[i] = alarmLights[i].color;
        }
    }

    void Update()
    {
        if (!alarmActive) return;

        float intensity = Mathf.Lerp(
            minIntensity,
            maxIntensity,
            Mathf.PingPong(Time.time * blinkSpeed, 1)
        );

        foreach (Light l in alarmLights)
        {
            l.color = Color.red; // Force le rouge pendant l'alerte
            l.intensity = intensity;
        }
    }

    public void StartAlarm()
    {
        guardsDetectingPlayer++;

        if (alarmActive) return;
        alarmActive = true;

        if (audioSource != null && alarmSound != null)
        {
            audioSource.clip = alarmSound;
            audioSource.loop = true;
            audioSource.spatialBlend = 0f;
            audioSource.Play();
        }
    }

    public void StopAlarm()
    {
        guardsDetectingPlayer--;

        if (guardsDetectingPlayer <= 0)
        {
            guardsDetectingPlayer = 0;
            alarmActive = false;

            if (audioSource != null) audioSource.Stop();

            // RÉINITIALISATION COMPLÈTE
            for (int i = 0; i < alarmLights.Length; i++)
            {
                alarmLights[i].intensity = originalIntensities[i]; // Remet l'intensité de base
                alarmLights[i].color = originalColors[i];         // Remet la couleur de base (blanc/jaune)
            }
        }
    }
}