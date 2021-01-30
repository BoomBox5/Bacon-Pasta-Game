using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;
    public Vector3 Offset;
    public float Xrot;
    public float Yrot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + Offset;
        Yrot += (Input.GetAxis("Mouse Y"))*45;
        Xrot += (Input.GetAxis("Mouse X"))*45;
        
        Vector3 rotation = transform.eulerAngles;

        rotation.x += (Input.GetAxis("Mouse Y")) * 360 * Time.deltaTime; 
        rotation.y += (Input.GetAxis("Mouse X")) * 360 * Time.deltaTime; 

        transform.eulerAngles = rotation;
    }
}
