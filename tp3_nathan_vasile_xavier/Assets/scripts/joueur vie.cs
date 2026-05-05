using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControllerVR : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController controller;
    public float speed = 2f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Lives")]
    public int maxLives = 3;
    public int currentLives;

    [Header("Death System")]
    public GameObject gameOverCanvas;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isDead = false;
    private bool invincible = false;

    void Start()
    {
        currentLives = maxLives;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;

        HandleMovement();
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // 💀 appelé par la lave ou ennemis
    public void TakeDamage(int damage)
    {
        if (isDead || invincible) return;

        currentLives -= damage;

        if (currentLives > 0)
        {
            StartCoroutine(HitCooldown());
        }
        else
        {
            Die();
        }
    }

    IEnumerator HitCooldown()
    {
        invincible = true;
        yield return new WaitForSeconds(1.5f);
        invincible = false;
    }

    void Die()
    {
        isDead = true;

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    // 🔘 bouton UI RESTART
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 🔘 bouton UI QUITTER
    public void QuitGame()
    {
        Application.Quit();
    }
}