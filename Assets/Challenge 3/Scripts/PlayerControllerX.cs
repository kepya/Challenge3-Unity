using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    public float gravityModifier;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip outGroundClip;
    public bool isLowEnough = true;
    private RepeatBackgroundX repeatBackgroundX;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);


        repeatBackgroundX = GameObject.Find("Background").GetComponent<RepeatBackgroundX>();

    }

    // Update is called once per frame
    void Update()
    {
        if (repeatBackgroundX.GetComponent<BoxCollider>().size.y > transform.position.y)
        {
            isLowEnough = true;
        }
        else
        {
            isLowEnough = false;
        }

        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver && isLowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce * Time.deltaTime, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

        if (other.gameObject.CompareTag("Ground"))
        {
            playerRb.AddForce(Vector3.up * 300 * Time.deltaTime, ForceMode.Impulse);
            playerAudio.PlayOneShot(outGroundClip, 1.0f); ;
            Debug.Log("Ground!");
        }

    }

}
