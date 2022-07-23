using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rBody;
    private Transform tr;
    GameObject arrow;
    Vector3 currPos;
    private float h;
    private float v;
    private float rotH;
    private float rotV;
    public float speed = 3.0f;
    public float rotSpeed = 100f;
    void Start()
    {
        arrow = this.gameObject.transform.GetChild(0).gameObject;
        rBody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        if (GameManager.instance.isShoot) return;

        arrow.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        rotH = Input.GetAxis("RotLR");
        rotV = Input.GetAxis("RotUD");
        Vector3 position = tr.position;
        position.x += h * speed * Time.deltaTime;
        position.z += v * speed * Time.deltaTime;

        tr.position = position;
        tr.Rotate(Vector3.up * rotH * rotSpeed * Time.deltaTime, Space.World);
        tr.Rotate(Vector3.forward * rotV * rotSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BASE")
        {
            GameManager.instance.isBased = true;
            rBody.Sleep();
            for (int i = 0; i < GameManager.instance.objPool.Count; i++)
            {
                if (GameManager.instance.objPool[i].activeInHierarchy) return;
            }
            GameManager.instance.NextStage();
            GameManager.instance.isShoot = false;
        }
    }
    void Shoot()
    {
        GameManager.instance.isShoot = true;
        GameManager.instance.isBased = false;
        arrow.SetActive(false);
        rBody.velocity = tr.right * 10f;
        GameManager.instance.currPoint = tr.right;
        currPos = tr.position;
        StartCoroutine(ShootingBall());
    }

    IEnumerator ShootingBall()
    {        
        for(int i = 1; i < GameManager.instance.stage; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject obj = GameManager.instance.GetBall();
            obj.transform.position = currPos;
            obj.SetActive(true);
        }        
    }
    public void Reset()
    {
        rBody.Sleep();
        tr.position = new Vector3(-2.22f, -1.77f, -2.22f);
        StopAllCoroutines();
    }
}
