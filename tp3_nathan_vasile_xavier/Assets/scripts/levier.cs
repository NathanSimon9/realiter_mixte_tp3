using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevierExpertDebug : MonoBehaviour
{
    [Header("Animators")]
    public Animator AnimLevierLora;
    public Animator AnimLevier2Lora;
    public Animator Animtrape1l;
    public Animator Animtrape2l;
    public Animator Animportefinl;

    [Header("Sons")]
    public AudioClip sonLevier; // Le son pour le levier lui-même
    private AudioSource audioSourceLevier;

    private bool dejaActive = false;
    private bool dejaActive2 = false;

    private void Awake()
    {
        // On récupère l'AudioSource du levier
        audioSourceLevier = GetComponent<AudioSource>();
        if (audioSourceLevier == null) audioSourceLevier = gameObject.AddComponent<AudioSource>();
        audioSourceLevier.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.tag == "levier" && !dejaActive)
            {
                StartCoroutine("activationLevier1");
                dejaActive = true;
            }
            else if (gameObject.tag == "levier2" && !dejaActive2)
            {
                StartCoroutine("activationLevier2");
                dejaActive2 = true;
            }
        }
    }

    public IEnumerator activationLevier1()
    {
        // 1. Son du levier
        if (sonLevier != null) audioSourceLevier.PlayOneShot(sonLevier);
        AnimLevierLora.Play("levier");

        yield return new WaitForSeconds(1f);

        // 2. Son de la trappe 1 (récupéré directement sur l'objet de la trappe)
        if (Animtrape1l != null)
        {
            AudioSource audioTrappe = Animtrape1l.GetComponent<AudioSource>();
            if (audioTrappe != null) audioTrappe.Play();
        }
        Animtrape1l.Play("trape_Sol_01");
    }

    public IEnumerator activationLevier2()
    {
        // 1. Son du levier
        if (sonLevier != null) audioSourceLevier.PlayOneShot(sonLevier);
        AnimLevier2Lora.Play("levier_02");

        yield return new WaitForSeconds(1f);

        // 2. Son de la trappe 2 (récupéré directement sur l'objet de la trappe)
        if (Animtrape2l != null)
        {
            AudioSource audioTrappe = Animtrape2l.GetComponent<AudioSource>();
            if (audioTrappe != null) audioTrappe.Play();
        }
        Animtrape2l.Play("trape_02_sol");
    }
}