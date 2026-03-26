using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.ShaderData;
using UnityEngine.SceneManagement;
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
    public static Vector3 dernierePosition;


    void Start()
    {
        // Si on revient de la scène Chaudron, on se replace au bon endroit
        if (dernierePosition != Vector3.zero)
        {
            transform.position = dernierePosition;
        }
    }

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
        if (other.CompareTag("green") && nbVertes < limiteVerte)
        {
            nbVertes++;
            Debug.Log("Verte ramassée !");
            Destroy(other.gameObject);
            VerifierChaudron();
        }

        if (other.CompareTag("red") && nbRouges < limiteRouge)
        {
            nbRouges++;
            Debug.Log("Rouge ramassée !");
            Destroy(other.gameObject);
            VerifierChaudron();
        }
    }

    void VerifierChaudron()
    {
        if (nbVertes >= limiteVerte && nbRouges >= limiteRouge)
        {
            // On enregistre la position juste avant de partir
            dernierePosition = transform.position;
            SceneManager.LoadScene("Chaudron");
        }
    }
}

