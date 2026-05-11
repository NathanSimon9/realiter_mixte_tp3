using UnityEngine;

public class AlarmLightManager : MonoBehaviour
{
    [Header("Toutes les Point Lights à contrôler")]
    public Light[] alarmLights;

    [Header("Audio")]
    public AudioSource audioSource; // Glisse ton AudioSource ici
    public AudioClip alarmSound;   // Glisse ton clip d'alarme ici (mets-le en "Loop" dans l'AudioSource si besoin)

    [Header("Clignotement")]
    public float blinkSpeed = 6f;
    public float minIntensity = 0f;
    public float maxIntensity = 6f;

    private bool alarmActive = false;

    void Start()
    {
        // Optionnel : s'assurer que le son ne joue pas au lancement
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
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
            l.color = Color.red;
            l.intensity = intensity;
        }
    }

    public void StartAlarm()
    {
        if (alarmActive) return; // Évite de relancer si déjà actif

        alarmActive = true;

        // Jouer le son
        if (audioSource != null && alarmSound != null)
        {
            audioSource.clip = alarmSound;
            audioSource.loop = true; // On veut que l'alarme boucle
            audioSource.Play();
        }
    }

    public void StopAlarm()
    {
        alarmActive = false;

        // Arrêter le son
        if (audioSource != null)
        {
            audioSource.Stop();
        }

        foreach (Light l in alarmLights)
        {
            l.intensity = minIntensity;
        }
    }
}