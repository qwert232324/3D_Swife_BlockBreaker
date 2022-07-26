using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Light : MonoBehaviour
{
    // 힘을 가하기 위한 rigidbody 선언
    private Rigidbody rBody;
    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // 활성화 되었을 경우, MainBall이 발사 되었던 방향과 같은 방향으로 힘을 가하며 효과음 재생
        rBody.velocity = GameManager.instance.currPoint * 10f;
        SoundManager.instance.PlaySound("shoot");
    }
    private void OnDisable()
    {
        // 비활성화 되었을 경우, 힘 제거
        rBody.Sleep();
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.tag == "BASE")
        {
            // 바닥에 닿았을 경우, 비활성화 한다
            this.gameObject.SetActive(false);
            // GameManager에서 다음 스테이지로 넘어갈 수 있는 상황임을 판단 후, 값이 true면 다음 스테이지로 넘어간다.
            if (GameManager.instance.canNext()) GameManager.instance.NextStage();
        }
    }

}
