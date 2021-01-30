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
      //move
    public float SpeedCap;
    public float Acceleration; 
    public float TurnSpeed; //the default turn speed
    private float turnc; //the current turn speed
    private float speedm; //the current speed cap
    private float speedc; //the current speed
      //sprint
    public float SprintMultiplier; //how many times faster sprinting is
    public float SuperSprintMultiplier; //how many times faster the super sprint is
    private float sprintcharge; 
    public float SuperSprintCooldown; //max time you have to activate the super sprint
    private float sprintcooldown; //how long you have left to activate the super sprint
      //jump
    public float JumpHeight; //how high each jump can be
    public float JumpCount; //the maximum jumps allowed
    private float jumpc; //how many jumps are left
    private float jumph; //the current jump force
    private bool OnGround; //active if on ground
      //airdash
    public float AirDashDist; //how long you can dash in the air for
    private float airdasht; //how long you can still dash
      //wallrun
    private float wallrunt2;
    private bool wallcollision;


    void Start()
    {
        speedm = SpeedCap; 
    }

    // Update is called once per frame
    void Update()
    {
    //move
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) //basic move
        {
            Vector3 face = (new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")));
            transform.rotation = Quaternion.Slerp(transform.rotation, (Quaternion.LookRotation(face)), turnc); //rotates in input + camera direction
            if (speedc <= speedm)  {speedc+=Acceleration; } 
        }
        else { if (speedc >= 0) speedc -= Acceleration*6; } //reduce acceleration
        if (speedc<=-0.001) {speedc=0; } 
       if (!(Input.GetButton("Xbut") && !OnGround && airdasht >= 0)) { rb.velocity = (transform.forward * speedc + new Vector3(0f, rb.velocity.y, 0f)); }  //apply velocity

        if (Input.GetButton("Xbut")) //sprint
        {       sprintcooldown = SuperSprintCooldown; 
            if (sprintcharge <= 2)
              { speedm =SpeedCap*SprintMultiplier;
               sprintcharge += 1 * Time.deltaTime;}     }
        else { sprintcooldown -= 1 * Time.deltaTime; }
        if (Input.GetButtonDown("Xbut")&& sprintcharge >= 2) //super sprint
        { speedm = SpeedCap * SuperSprintMultiplier;
          turnc = TurnSpeed /(TurnSpeed*2); 
          Instantiate(SprintParticles, transform.position, transform.rotation); }
        if (sprintcooldown <= 0) { sprintcharge = 0; turnc = TurnSpeed; speedm = SpeedCap; }
        if (sprintcharge >= 2&&speedm<=SpeedCap*2.99f) { StartCoroutine(Flash()); }
    //airdash
        if (Input.GetButton("Xbut") && !OnGround && airdasht >=0) 
        { rb.velocity = (transform.forward * speedm*2f); 
          airdasht-=1*Time.deltaTime;}
    //jump
        if (jumpc>=0.1&& Input.GetButton("Abut")&&jumph>=0.01) 
        { rb.velocity = new Vector3(rb.velocity.x, JumpHeight, rb.velocity.z);  
        if (jumph>=0) {jumph-=JumpHeight*Time.deltaTime*20;}}
        if (Input.GetButtonDown("Abut")) { jumpc -= 1; jumph = JumpHeight; } //change to joystick
        if (Input.GetButtonUp("Abut")) { jumph = 0; } //stops jumping early
    //wallrun
        //WallRunCheck.GetComponent<WallRunCheck>(wallrunt);
    }
    IEnumerator Flash () 
    {
        var Renderer = gameObject.GetComponent<Renderer>();

        //Call SetColor using the shader property name "_Color" and setting the color to red
        Renderer.material.SetColor("_Color", Color.yellow);
        yield return new WaitForSeconds(0.2f);
        Renderer.material.SetColor("_Color", Color.white);
    }

    IEnumerator Death()
    { yield return new WaitForSeconds(0.2f);
        transform.position = RespawnCoords; }

        private void OnTriggerExit(Collider col)
    { if (Input.GetButton("Abut")==false) {jumpc-=1; }
      OnGround=false;
    }
    private void OnTriggerStay(Collider col)
    {
        
        if (col.tag == "KillPlane")
        { StartCoroutine(Death());  }
        if (col.tag == "Checkpoint")
        { RespawnCoords=col.transform.position; }
       //executes only on floor
        if (col.tag != "Checkpoint" && col.tag != "Player" && col.tag != "Detector") 
        { OnGround=true;
          jumpc = JumpCount; //resets jump
         if (speedc >= speedm) { speedc -= Acceleration; } //speed cap
         if (!Input.GetButton("Xbut")) { airdasht = AirDashDist; } //resets airdash (toggleable)
        }
    }
}
