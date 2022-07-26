using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Material[] colors = new Material[9]; // 맞을때마다 바뀔 색상이 저장되어져 있는 배열
    private MeshRenderer mesh; // 내 Material 변환을 위해 선언
    private int hitCount; // 남은 맞춰야 할 횟수

    void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        // 초기값 : 현재 stage 점수만큼 부여, 그 값에 따라 블럭의 색상을 변환
        // [빨,주,노,초,파,남,보,흰,검] 의 순환형태
        hitCount = GameManager.instance.stage;
        mesh.material = colors[(hitCount - 1) % 9];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BALL")
        {
            // hitCount가 0이 되면 Break()로 넘어간다.
            if (--hitCount == 0) Break();

            // 0이 아닐경우 색상을 변환하며 효과음을 출력
            else
            {
                mesh.material = colors[(hitCount - 1) % 9];
                SoundManager.instance.PlaySound("shoot");
            }
        }
    }
    private void Break()
    {
        // 블럭을 비활성화 시키며 효과음을 출력
        this.gameObject.SetActive(false);
        SoundManager.instance.PlaySound("break");
    }

}
