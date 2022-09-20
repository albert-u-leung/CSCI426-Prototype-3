using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public float StartSpeed, gameOverSpeed;
    public PlayerController player;
    private Transform defaultTarget;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        defaultTarget = FindObjectOfType<DefaultTarget>().gameObject.transform;
        player = FindObjectOfType<PlayerController>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = (defaultTarget.position-transform.position).normalized * StartSpeed;
    }

    private void FixedUpdate()
    {
        if (!gameManager.gameOver)
        {
            rb.velocity = StartSpeed * (rb.velocity.normalized);
        }
        else
        {
            rb.velocity = gameOverSpeed * (rb.velocity.normalized);
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bat")
        {
            rb.velocity = (transform.position - other.gameObject.transform.parent.transform.position).normalized * StartSpeed;
        }
    }
}
