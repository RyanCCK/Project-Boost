﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketMan : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {Alive, Dying, Progressing};
    State state = State.Alive;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 4f;

    [SerializeField] AudioClip thrustSound;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip successSound;

    [SerializeField] ParticleSystem leftThrustParticles;
    [SerializeField] ParticleSystem rightThrustParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    float nextSceneLoadTime = 1.5f;
    float deathTime = 1.5f;

    // Start is called before the first frame update.
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame.
    void Update()
    {
        if (state == State.Alive)
        {
            HandleThrustInput();
            HandleRotationInput();
        }
    }

    // Apply thrust to rocket and play thrust sound effect when space key is pressed.
    private void HandleThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else audioSource.Stop();
        leftThrustParticles.Stop();
        rightThrustParticles.Stop();
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(thrustSound);
        }
        leftThrustParticles.Play();
        rightThrustParticles.Play();
    }

    // Rotate rocket clockwise or counterclockwise when right or left arrow keys are pressed.
    private void HandleRotationInput()
    {
        // Freeze physics control of rotation
        rigidBody.freezeRotation = true;

        // For framerate independence.
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }

        // Resume physics control of rotation
        rigidBody.freezeRotation = false;
    }

    // Handle collisions.
    private void OnCollisionEnter(Collision collision)
    {
        // Ignore collisions when not alive.
        if (state != State.Alive) {return;} 

        // Handle collisions based on tag of game object the player collides with.
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;

            case "Finish":
                ProgressPlayer();
                break;

            default:
                KillPlayer();
                break;
        }
    }

    // Move to the next stage of the game when the player completes the objective in the current stage.
    private void ProgressPlayer()
    {
        state = State.Progressing;
        audioSource.Stop();
        audioSource.PlayOneShot(successSound);
        successParticles.Play();
        //Invoke("LoadNextScene", nextSceneLoadTime);
        Invoke("LoadFirstScene", nextSceneLoadTime);    //Only 1 scene exists, so reload it on win. ONLY FOR BETA BUILD
    }

    // Handle all events that occur when the player dies.
    private void KillPlayer()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        deathParticles.Play();
        Invoke("LoadFirstScene", deathTime);
    }

    // Load the next scene.
    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    // Load the first scene.
    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }
}
