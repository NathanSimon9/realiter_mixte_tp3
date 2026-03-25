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
    public int limiteRouge = 4;
    public int limiteVerte = 4;
    public GameObject chaudron;

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
        // 1. Les bouteilles
        if (other.CompareTag("green") && nbVertes < limiteVerte)
        {
            nbVertes++;
            Debug.Log("Verte: " + nbVertes + "/" + limiteVerte);
            Destroy(other.gameObject);
            VerifierChaudron(); // On vérifie si on doit activer le chaudron
        }
        else if (other.CompareTag("red") && nbRouges < limiteRouge)
        {
            nbRouges++;
            Debug.Log("Rouge: " + nbRouges + "/" + limiteRouge);
            Destroy(other.gameObject);
            VerifierChaudron(); // On vérifie si on doit activer le chaudron
        }

        // 2. Le Chaudron (S'exécute seulement si l'objet touché a le tag "chaudron")
        else if (other.CompareTag("chaudron"))
        {
            if (nbVertes >= limiteVerte && nbRouges >= limiteRouge)
            {
                Debug.Log("Chaudron détecté ! La potion est prête.");
            }
            else
            {
                Debug.Log("Le chaudron est là, mais il vous manque des bouteilles !");
            }
        }
    }

    void VerifierChaudron()
    {
        // Cette fonction active le chaudron dans la scène dès que le score est atteint
        if (nbVertes >= limiteVerte && nbRouges >= limiteRouge)
        {
            if (chaudron != null)
            {
                chaudron.SetActive(true);
                Debug.Log("Le chaudron est maintenant actif dans la hiérarchie !");
            }
        }
    }
}
