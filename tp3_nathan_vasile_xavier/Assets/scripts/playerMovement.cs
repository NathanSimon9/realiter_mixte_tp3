using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // On vérifie le tag ET si on n'a pas encore atteint la limite
        if (other.CompareTag("green") && nbVertes < limiteVerte)
        {
            nbVertes++;
            Debug.Log("Bouteille verte récupérée ! Total : " + nbVertes + "/" + limiteVerte);
            Destroy(other.gameObject);

            if (nbVertes == limiteVerte)
            {
                Debug.Log("Limite de bouteilles VERTES atteinte.");
            }
        }
        else if (other.CompareTag("red") && nbRouges < limiteRouge)
        {
            nbRouges++;
            Debug.Log("Bouteille rouge récupérée ! Total : " + nbRouges + "/" + limiteRouge);
            Destroy(other.gameObject);

            if (nbRouges == limiteRouge)
            {
                Debug.Log("Limite de bouteilles ROUGES atteinte.");
            }
        }
    }
}