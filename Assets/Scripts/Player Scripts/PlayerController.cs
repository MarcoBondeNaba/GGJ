﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public float runningSpeed;


    private CharacterController controller;

    public Animator animator;
    private bool isRunning;
    private float isWalking;

    RaycastHit hit;
    GameObject pickedUpObject;

    public float runMultiplier = 1;
    public float screamMultiplier = 20;
    public float radioMultiplier = 2;
    public bool scream = false;
    public Slider slider;

    public bool berserker = false;

    public GameObject storageLight ;

    Vector3 forward;
    float curSpeed;

    public GameObject winCube;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

        forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = movementSpeed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * curSpeed);


        if (Input.GetAxis("Vertical") > 0.1f)
            animator.SetBool("move", true);
        else
            animator.SetBool("move", false);

        if (Input.GetButton("Fire3"))
        {
            animator.SetBool("isRunning", true);
            movementSpeed = runningSpeed;

            slider.value += (runMultiplier * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isRunning", false);
            movementSpeed = 2;
        }

        animator.SetBool("isRunning", isRunning);

        //Scream
        if (Input.GetButtonDown("Fire1") && !scream)
        {
            slider.value += screamMultiplier;
            scream = true;
            StartCoroutine("WaitButtomToBePressed");

        }
        if(slider.value < 100)
        {
            //Movement
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

            forward = transform.TransformDirection(Vector3.forward);
            curSpeed = movementSpeed * Input.GetAxis("Vertical");
            controller.SimpleMove(forward * curSpeed);

            isWalking = Input.GetAxis("Horizontal");
            isWalking = Input.GetAxis("Vertical");

            //Running
            isRunning = Input.GetButton("Fire3");
        }
        //Berserker
        else if (slider.value >= 100)
        {
            berserker = true;

            movementSpeed = 8;

            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

            forward = transform.TransformDirection(Vector3.forward);
            curSpeed = movementSpeed * Input.GetAxis("Vertical");
            controller.SimpleMove(forward * curSpeed);

            StartCoroutine("Berserker");
        }

        //Interaction with Raycast
        if (Input.GetKeyDown(KeyCode.E))
        {
            winCube.SetActive(true);

                if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
                {
                    if (hit.collider.gameObject.tag == "Switch")
                    {
                        //pickedUpObject = hit.collider.gameObject;
                        //hit.collider.gameObject.transform.parent = transform;
                        //hit.collider.gameObject.transform.position = transform.position - transform.forward;
                        print("Hit!");
                        storageLight.SetActive(true);
                    }
                }
        }
        else
        {
            //winCube.SetActive(false);

               if (pickedUpObject != null)
               {
                pickedUpObject.transform.parent = null;
                pickedUpObject = null;
               }
        }


        
        forward = transform.TransformDirection(Vector3.forward) * 100;
        Debug.DrawLine(transform.position, forward, Color.magenta);
    }

    private void OnTriggerExit (Collider collider)
    {
        //Stun by an object
        if (collider.tag == "StunCollider")
        {
            print("Stunned");
            controller.enabled = false;
            rotationSpeed = 0;
            StartCoroutine("Stun");
        }

    }

    public void startStun() {
        controller.enabled = false;
        rotationSpeed = 0;
        StartCoroutine("Stun");
    }

    IEnumerator WaitButtomToBePressed()
    {
            yield return new WaitForSeconds(10f);
            scream = false;
        
    }

    IEnumerator Berserker()
    {
            yield return new WaitForSeconds(10f);
            berserker = false;
            slider.value = 0;
    }

    IEnumerator Stun()
    {
            yield return new WaitForSeconds(4f);
            controller.enabled = true;
            rotationSpeed = 1f;
    }
}

