using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rBody;
    private Transform tr;
    public LineRenderer line; // 가이드 라인을 제공할 Line Renderer
    public Transform laserPos; // Line을 발사할 시작점

    // 공의 상태를 나타내는 bool 변수들
    public bool isShoot; // 현재 발사 중인지 판단 (Main Ball 뿐만이 아니라 어느 하나라도)
    public bool isBased; // 현재 바닥에 있는지를 판단 (Main Ball 만)

    private float h; // 키보드 입력값_AD
    private float v; // 키보드 입력값_WS
    private float rotH; // 방향키 입력값_LR
    private float rotV; // 방향키 입력값_UD

    public float speed = 3.0f; // 이동 속도
    public float rotSpeed = 100f; // 회전 속도

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();
        laserPos = transform.GetChild(0).GetComponent<Transform>(); // Laser Pos의 위치는 Ball의 자식에 있다.
        line.material.SetColor("_Color", Color.red); // Line의 색을 빨간색으로 설정
    }

    void Update()
    {
        if (isShoot) return; // 현재 발사 중일 경우 움직임을 제한한다.

        if (Input.GetKeyDown(KeyCode.Space)) Shoot(); // Space를 누르면 발사

        // 이동 관련 코드. 사용자에게 WASD 값을 읽어와 position을 이동시킨다.
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 position = tr.position;
        position.x += h * speed * Time.deltaTime;
        position.z += v * speed * Time.deltaTime;
        tr.position = position;

        // 회전 관련 코드. 사용자에게 방향키 값을 읽어와 Rotate
        rotH = Input.GetAxis("RotLR");
        rotV = Input.GetAxis("RotUD");
        tr.Rotate(Vector3.up * rotH * rotSpeed * Time.deltaTime, Space.World); // World의 축을 기준으로 움직인다. 대각선으로 회전함을 방지
        tr.Rotate(Vector3.forward * rotV * rotSpeed * Time.deltaTime);

        // 회전각 제한 관련 코드
        Vector3 currRot = tr.rotation.eulerAngles; // 현재 Rotation을 Vector3로 변환하여 가져옴
        currRot.z = Mathf.Clamp(currRot.z, 10.0f, 90.0f); // Rotation의 z값(높이 조절용)을 10~90도 까지만 움직이도록 설정
        tr.rotation = Quaternion.Euler(currRot); // 이를 현재 Rotation에 넣어준다
        
        // 가이드라인 발사 관련 코드
        RaycastHit hit;
        line.SetPosition(0, laserPos.position); // Line의 시작점은 laserPos의 위치
        if (Physics.Raycast(laserPos.position, laserPos.right, out hit, 5000f)) line.SetPosition(1, hit.point); // laserPos에서 출발해 Object에 충돌한 지점을 Line의 끝점으로 설정

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BASE")
        {
            // 바닥에 닿았을 경우, 힘을 제거하고 bool 변수로 바닥에 닿았음을 알림
            isBased = true;
            rBody.Sleep();
            // GameManager에서 다음 스테이지로 넘어갈 수 있는 상황임을 판단 후, 값이 true면 다음 스테이지로 넘어간다.
            if (GameManager.instance.canNext()) GameManager.instance.NextStage();
        }
    }

    // 발사 함수
    void Shoot()
    {
        isShoot = true; // 현재 발사중임을 알림
        isBased = false; // 현재 떠있음을 알림
        line.enabled = false; // 가이드라인이 보이지 않도록 설정
        rBody.velocity = tr.right * 10f; // 발사, 힘을 가한다.

        GameManager.instance.ShootingBall(tr); // GameManager가 추가로 공을 발사한다.
    }

    // RE 버튼을 누르면 작동하는 Event 함수. Inspector에서 다룬다
    public void Reset()
    {
        isShoot = false; // 발사가 끝났음을 알림
        rBody.Sleep(); // 힘 제거
        tr.position = new Vector3(-2.22f, -1.77f, -2.22f); // 초기 위치로 초기화
    }
}
