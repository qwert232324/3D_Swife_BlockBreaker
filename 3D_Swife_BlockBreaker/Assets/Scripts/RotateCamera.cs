using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float azimuth;
    public float elevation;
    public float radius;
    public float rotSpeed = 1f;

    public Camera cam;
    public GameObject stage;
    public Vector3 stagePos;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        stagePos = stage.transform.position;// + new Vector3(-1.5f, 0, -1.5f);
        var pos = cam.transform.position;

        radius = Vector3.Distance(pos,stagePos);
        azimuth = Mathf.Atan2(pos.z, pos.x);
        elevation = Mathf.Acos(pos.y / radius);
        
        RotatingCamera();
    }

    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            azimuth += Input.GetAxisRaw("Mouse X") * rotSpeed * Time.deltaTime;
            elevation += Input.GetAxisRaw("Mouse Y") * rotSpeed * Time.deltaTime;
            RotatingCamera();
            cam.transform.LookAt(stagePos);
        }       
    }

    private void RotatingCamera()
    {
        var t = radius * Mathf.Sin(elevation);
        var x = t * Mathf.Cos(azimuth);
        var y = radius * Mathf.Cos(elevation);
        var z = t * Mathf.Sin(azimuth);
        cam.transform.position = new Vector3(x, y, z);
        cam.transform.LookAt(stagePos);
    }
}
