using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderData;

public class playerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;

    public float gravity = -9.81f;
    public float jumpHeight = 3f; 

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask; 

    private Vector3 velocity;
    private bool isGrounded;
    private int nbVertes = 0;
    private int nbRouges = 0;
    public int limiteRouge = 4;
    public int limiteVerte = 4;
    public GameObject chaudron;
    public GameObject panelChaudron;

    public TextMeshProUGUI texteRougeUI;
    public TextMeshProUGUI texteVertUI;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    
    }
   
    private void OnTriggerEnter(Collider other)
    {
        // --- 1. COLLECTE DES BOUTEILLES ---
        if (other.CompareTag("green") && nbVertes < limiteVerte)
        {
            nbVertes++;
            Debug.Log("VERTE RÉCUPÉRÉE ! Total: " + nbVertes + "/" + limiteVerte);
            Destroy(other.gameObject);
            VerifierChaudron(); // On appelle la vérification
        }

        if (other.CompareTag("red") && nbRouges < limiteRouge)
        {
            nbRouges++;
            Debug.Log("ROUGE RÉCUPÉRÉE ! Total: " + nbRouges + "/" + limiteRouge);
            Destroy(other.gameObject);
            VerifierChaudron(); // On appelle la vérification
        }

        // --- 2. DÉTECTION DU CHAUDRON ---
        if (other.CompareTag("chaudron"))
        {
            // Si le chaudron est apparu (donc actif), on affiche le message
            if (chaudron != null && chaudron.activeSelf)
            {
                Debug.Log("COLLISION CHAUDRON : Le chaudron est prêt !");
                // On peut quand même activer le panel ici si tu veux juste voir s'il s'affiche
                if (panelChaudron != null) panelChaudron.SetActive(true);
            }
            else
            {
                Debug.Log("Le chaudron est là, mais il manque des bouteilles.");
            }
        }
    }

    // --- CETTE FONCTION DOIT ÊTRE EXACTEMENT COMME ÇA ---
    void VerifierChaudron()
    {
        // Ce message s'affichera à CHAQUE bouteille ramassée
        Debug.Log("Vérification : R=" + nbRouges + "/" + limiteRouge + " V=" + nbVertes + "/" + limiteVerte);

        if (nbVertes >= limiteVerte && nbRouges >= limiteRouge)
        {
            Debug.Log("LA CONDITION EST ENFIN VRAIE !");
            if (chaudron != null)
            {
                chaudron.SetActive(true);
                Debug.Log("Appel de SetActive(true) sur : " + chaudron.name);
            }
            else
            {
                Debug.LogError("ERREUR : Tu as oublié de glisser le chaudron dans l'inspecteur !");
            }
        }
    }
}
