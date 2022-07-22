using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    //public float azimuth;
    //public float elevation;
    //public float radius;
    //public float rotSpeed = 1f;

    //public Camera cam;
    //public GameObject stage;
    //public Vector3 stagePos;

    //void Start()
    //{
    //    cam = GetComponentInChildren<Camera>();
    //    stagePos = stage.transform.position;// + new Vector3(-1.5f, 0, -1.5f);
    //    var pos = cam.transform.position;

    //    radius = Vector3.Distance(pos,stagePos);
    //    azimuth = Mathf.Atan2(pos.z, pos.x);
    //    elevation = Mathf.Acos(pos.y / radius);

    //    RotatingCamera();
    //}

    //void Update()
    //{
    //    if (Input.GetMouseButton(1))
    //    {
    //        azimuth += Input.GetAxisRaw("Mouse X") * rotSpeed * Time.deltaTime;
    //        elevation += Input.GetAxisRaw("Mouse Y") * rotSpeed * Time.deltaTime;
    //        RotatingCamera();
    //        cam.transform.LookAt(stagePos);
    //    }       
    //}

    //private void RotatingCamera()
    //{
    //    var t = radius * Mathf.Sin(elevation);
    //    var x = t * Mathf.Cos(azimuth);
    //    var y = radius * Mathf.Cos(elevation);
    //    var z = t * Mathf.Sin(azimuth);
    //    cam.transform.position = new Vector3(x, y, z);
    //    cam.transform.LookAt(stagePos);
    //}

    [SerializeField]
    private GameObject target = null;       // 타겟이 될 게임오브젝트
    private Vector3 point = Vector3.zero;   // 타겟의 위치(바라볼 위치)

    private float rotationX = 0.0f;         // X축 회전값
    private float rotationY = 0.0f;         // Y축 회전값
    private float speed = 100.0f;           // 회전속도


    void Start()
    {
        // 바라볼 위치 얻기
        point = target.transform.position;
    }

    void Update()
    {
        // 마우스가 눌러지면,
        if (Input.GetMouseButton(1))
        {
            // 마우스 변화량을 얻고, 그 값에 델타타임과 속도를 곱해서 회전값 구하기
            rotationX = Input.GetAxis("Mouse X") * Time.deltaTime * speed;
            rotationY = Input.GetAxis("Mouse Y") * Time.deltaTime * speed;

            // 각 축으로 회전
            // Y축은 마우스를 내릴때 카메라는 올라가야 하므로 반대로 적용
            transform.RotateAround(point, Vector3.right, -rotationY);
            transform.RotateAround(point, Vector3.up, rotationX);

            // 회전후 타겟 바라보기
            transform.LookAt(point);
        }
    }
}
