using UnityEngine;

public class AlarmLightManager : MonoBehaviour
{
    [Header("Toutes les Point Lights à contrôler")]
    public Light[] alarmLights;

    [Header("Clignotement")]
    public float blinkSpeed = 6f;
    public float minIntensity = 0f;
    public float maxIntensity = 6f;

    private bool alarmActive = false;

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
        alarmActive = true;
    }

    public void StopAlarm()
    {
        alarmActive = false;

        foreach (Light l in alarmLights)
        {
            l.intensity = minIntensity;
        }
    }
}