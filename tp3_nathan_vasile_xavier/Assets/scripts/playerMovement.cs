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
    [Header("--- UI Donjon ---")]
      public CharacterController controller;

    public float speed = 2f;

    public float gravity = -9.81f;
    public float jumpHeight = 3f; 

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask; 

    private Vector3 velocity;
    private bool isGrounded;
  


   
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
    public class InteractionJoueur : MonoBehaviour
    {
        [Header("Référence de la Trappe")]
        public Animator trappeAnimator; // Glisse l'objet Trappe_sol_01 ici dans l'inspecteur

        private void OnCollisionEnter(Collision collision)
        {
            // 1. On vérifie si l'objet touché a le tag "levier"
            if (collision.gameObject.CompareTag("levier"))
            {
                // 2. On récupère l'Animator du levier pour jouer son animation
                Animator levierAnim = collision.gameObject.GetComponent<Animator>();

                if (levierAnim != null)
                {
                    levierAnim.SetTrigger("PlayAnim");
                    Debug.Log("Animation du levier lancée !");
                }

                // 3. On fait jouer l'animation de la trappe
                if (trappeAnimator != null)
                {
                    trappeAnimator.SetTrigger("OuvrirTrappe");
                    Debug.Log("Animation de la trappe lancée !");

                    // Optionnel : On désactive le collider du levier pour ne pas le réactiver par erreur
                    if (collision.gameObject.GetComponent<Collider>() != null)
                    {
                        collision.gameObject.GetComponent<Collider>().enabled = false;
                    }
                }
                else
                {
                    Debug.LogError("Erreur : Glisse l'objet avec le controller Trape_sol_01 dans la case 'Trappe Animator' sur le Joueur !");
                }
            }
        }
    }
}




