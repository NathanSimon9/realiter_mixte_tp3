using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevierExpertDebug : MonoBehaviour
{

    public Animator AnimLevierLora;
    public Animator AnimLevier2Lora;
    public Animator Animtrape1l;
    public Animator Animtrape2l;
    public Animator Animportefinl;
  

    private bool dejaActive = false;
    private bool dejaActive2 = false;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie que c'est le joueur et qu'on n'a pas déjà activé le levier
        if (other.CompareTag("Player") )
        {

            if(gameObject.tag == "levier" && !dejaActive)
            {
                StartCoroutine("activationLevier1");
                dejaActive = true;
            } else if (gameObject.tag == "levier2" && !dejaActive2)
            {
                StartCoroutine("activationLevier2");
                dejaActive2 = true;
            }




            

        }
    }

    public IEnumerator activationLevier1()
    {
        AnimLevierLora.Play("levier");
        yield return new WaitForSeconds(1f);
        Animtrape1l.Play("trape_Sol_01");
        yield break;
    }
    public IEnumerator activationLevier2()
    {
        AnimLevier2Lora.Play("levier_02");
        yield return new WaitForSeconds(1f);
        Animtrape2l.Play("trape_02_sol");
        yield return new WaitForSeconds(15f);
        Animportefinl.Play("Door_fin");
        Animportefinl.gameObject.GetComponent<BoxCollider>().enabled = false; 
        yield break;
    }
}