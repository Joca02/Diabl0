using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public GameObject player;
    private Vector3 cameraPos;
    private float camFOV;
    public float zoomSpeed = 70f;
    void Start()
    {
        cameraPos= transform.position-player.transform.position;
        camFOV= Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position= player.transform.position+cameraPos;

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        camFOV-=mouseScroll*zoomSpeed;
        camFOV=Mathf.Clamp(camFOV, 20, 60);
        Camera.main.fieldOfView= camFOV;
    }
}
