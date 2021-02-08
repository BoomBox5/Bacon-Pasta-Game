using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject Camera;
    public GameObject WallRunCheck;
    public GameObject SprintParticles;
    public Vector3 RespawnCoords;

    public float speedCap;
    public float walk;
    public float run;
    public float acceleration;
    public float deccel;
    public float turnSpeed;
    public float jumpHeight;
    public float jumpsMax;
    public float height;
    public float dashDistance;
    public float dashCooldown;

    private float speed = 0;
    private float jumps = 0;
    private bool grounded;
    private float dashT;
    private float dashC;

    void Start()
    {
        speedCap = walk;
    }

    void Update()
    {
        if (dashT <= 0)
        {
            if (Input.GetButton("Xbut"))
            {
                speedCap = run;
            }
            else
            {
                speedCap = walk;
            }
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
                Vector3 face = Quaternion.Euler(0, Camera.transform.eulerAngles.y, 0) * input;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(face), turnSpeed * Time.deltaTime);
                speed = Mathf.Clamp(speed + acceleration, 0, speedCap);
                rb.velocity = face * speed + new Vector3(0, rb.velocity.y, 0);
            }
            else
            {
                speed = Mathf.Clamp(speed - deccel, 0, speedCap);
            }
            if (Input.GetButtonDown("Abut") && jumps > 0)
            {
                jumps -= 1;
                rb.velocity += new Vector3(0, jumpHeight, 0);
            }
            if (Input.GetButtonDown("Xbut"))
            {
                if (!grounded && dashC <= 0)
                {
                    rb.velocity = transform.forward * 60;
                    dashT = dashDistance;
                }
            }
        }
        else
        {
            dashT -= Time.deltaTime;
            if(dashT <= 0)
            {
                rb.velocity = Vector3.zero;
                speed = 0;
                dashC = dashCooldown;
            }
        }
        if(dashC > 0)
        {
            dashC -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        Debug.DrawLine(transform.position, transform.position - new Vector3(0, (height / 2) + 0.1f, 0));
        if (Physics.Raycast(transform.position, Vector3.down, (height / 2) + 0.1f,3))
        {
            grounded = true;
            jumps = jumpsMax;
            dashC = 0;
        }
        else
        {
            if(grounded == true && jumps == jumpsMax)
            {
                jumps -= 1;
            }
            grounded = false;
        }
    }
}