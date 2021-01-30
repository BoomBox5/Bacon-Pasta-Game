using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunCheck : MonoBehaviour
{
    public GameObject Player;
    public bool colliding;
    public bool CanWallRun;
    public float WallRunTime;
    private float wallrunt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position; 
        transform.rotation = Player.transform.rotation;

        wallrunt -= 1 * Time.deltaTime;
    }
    private void OnTriggerExit(Collider col)
    { colliding = false; }
    private void OnTriggerEnter(Collider col)
    { wallrunt = WallRunTime; }
    private void OnTriggerStay(Collider col)
    { if (col.tag != "Player")
        { colliding=true;} }
}
