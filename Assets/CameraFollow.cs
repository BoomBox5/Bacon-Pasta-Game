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
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
        Yrot += (Input.GetAxis("Mouse Y")) * 360 * Time.deltaTime;
        Yrot = Mathf.Clamp(Yrot, -90, 90);
        Xrot += (Input.GetAxis("Mouse X")) * 360 * Time.deltaTime;

        //Vector3 rotation = transform.eulerAngles;

        //rotation.x += (Input.GetAxis("Mouse Y")) * 360 * Time.deltaTime;
        //rotation.x = Mathf.Clamp(rotation.x + (Input.GetAxis("Mouse Y") * 360 * Time.deltaTime), 0, 90);
        //rotation.y += (Input.GetAxis("Mouse X")) * 360 * Time.deltaTime;

        transform.rotation = Quaternion.Euler(Yrot, Xrot, 0);

        transform.position = Player.transform.position + (transform.right * Offset.x) + (transform.up * Offset.y) + (transform.forward * Offset.z);
    }
}
