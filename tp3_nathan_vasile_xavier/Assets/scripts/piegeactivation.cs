using UnityEngine;

public class SceneMechanicsAudio : MonoBehaviour
{
    // Grinder Simple et Lava SUPPRIMÉS pour éviter les erreurs de son de départ

    [Header("Categorie 2 - Grinder Triple")]
    public AudioClip sonGrinderTriple_1;
    public AudioClip sonGrinderTriple_2;
    public AudioClip sonGrinderTriple_3;

    [Header("Categorie 3 - Sol Spike")]
    public AudioClip sonSolSpike;

    [Header("Categorie 4 - Scie")]
    public AudioClip sonScie;

    [Header("Categorie 5 - Axe Swing")]
    public AudioClip sonAxeSwing;

    [Header("Categorie 6 - Roof Spike")]
    public AudioClip sonRoofSpike;

    void Start()
    {
        // On lance uniquement les catégories qui ont des sons de départ
        PlayCategory2();
        PlayCategory3();
        PlayCategory4();
        PlayCategory5();
        PlayCategory6();
    }

    // 🔹 CATEGORIE 2
    void PlayCategory2()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("grinder_triple");

        if (obj == null) return;

        AudioSource source = GetOrAddAudio(obj);
        source.loop = false;
        source.spatialBlend = 1f;

        if (sonGrinderTriple_1 != null) source.PlayOneShot(sonGrinderTriple_1);
        if (sonGrinderTriple_2 != null) source.PlayOneShot(sonGrinderTriple_2);
        if (sonGrinderTriple_3 != null) source.PlayOneShot(sonGrinderTriple_3);
    }

    // 🔹 CATEGORIE 3
    void PlayCategory3()
    {
        PlaySingle("sol_spike", sonSolSpike, "Sol Spike");
    }

    // 🔹 CATEGORIE 4
    void PlayCategory4()
    {
        PlaySingle("scie", sonScie, "Scie");
    }

    // 🔹 CATEGORIE 5
    void PlayCategory5()
    {
        PlaySingle("axe_swing", sonAxeSwing, "Axe Swing");
    }

    // 🔹 CATEGORIE 6
    void PlayCategory6()
    {
        // Utilise le tag approprié pour tes spikes de plafond
        PlaySingle("sol_spike", sonRoofSpike, "Roof Spike");
    }

    // 🔧 UTILITAIRE
    void PlaySingle(string tag, AudioClip clip, string label)
    {
        if (clip == null) return;

        GameObject obj = GameObject.FindGameObjectWithTag(tag);
        if (obj == null) return;

        AudioSource source = GetOrAddAudio(obj);
        source.clip = clip;
        source.loop = false;
        source.spatialBlend = 1f;
        source.Play();
    }

    AudioSource GetOrAddAudio(GameObject obj)
    {
        AudioSource source = obj.GetComponent<AudioSource>();
        if (source == null)
            source = obj.AddComponent<AudioSource>();
        return source;
    }
}