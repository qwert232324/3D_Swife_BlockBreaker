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
    public LineRenderer line;
    public Transform laserPos;
    void Start()
    {
        arrow = this.gameObject.transform.GetChild(0).gameObject;
        rBody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();
        laserPos = transform.GetChild(0).GetComponent<Transform>();
        line.material.SetColor("_Color", Color.red);
    }
    void Update()
    {
        if (GameManager.instance.isShoot) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 position = tr.position;
        position.x += h * speed * Time.deltaTime;
        position.z += v * speed * Time.deltaTime;
        tr.position = position;

        rotH = Input.GetAxis("RotLR");
        rotV = Input.GetAxis("RotUD");

        tr.Rotate(Vector3.up * rotH * rotSpeed * Time.deltaTime, Space.World);
        tr.Rotate(Vector3.forward * rotV * rotSpeed * Time.deltaTime);
        Vector3 currRot = tr.rotation.eulerAngles;
        currRot.z = Mathf.Clamp(currRot.z, 10.0f, 90.0f);
        tr.rotation = Quaternion.Euler(currRot);


        
        RaycastHit hit;

        line.SetPosition(0, laserPos.position);
        if (Physics.Raycast(laserPos.position, laserPos.right, out hit, 5000f))
        {
            line.SetPosition(1, hit.point);
        }

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
        line.enabled = false;
        rBody.velocity = tr.right * 10f;
        GameManager.instance.currPoint = tr.right;
        currPos = tr.position;
        GameManager.instance.audioSource.PlayOneShot(GameManager.instance.shootSound);
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
