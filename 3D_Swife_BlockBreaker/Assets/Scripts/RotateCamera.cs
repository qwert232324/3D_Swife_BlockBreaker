using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public GameObject target; // 쳐다보게 될 GameObject
    private Vector3 point; // 타겟의 위치

    private float x = 0.0f; // 마우스 입력값_X
    private float y = 0.0f; // 마우스 입력값_Y
    private float rotSpeed = 100.0f; // 회전 속도


    void Start()
    {
        // target 객체의 위치 얻기
        point = target.transform.position;
    }

    void Update()
    {
        // 우클릭 하고있는 상태에서 움직일 것
        if (Input.GetMouseButton(1))
        {
            // 사용자에게 마우스 값 입력받음
            x = Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed;
            y = Input.GetAxis("Mouse Y") * Time.deltaTime * rotSpeed;

            // 각 축으로 회전
            transform.RotateAround(point, Vector3.right, -y);
            transform.RotateAround(point, Vector3.up, x);

            // target 쳐다보기
            transform.LookAt(point);
        }
    }
}
