using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rBody;
    private Transform tr;
    private float h;
    private float v;
    public float speed = 3.0f;
    Vector3 currPos;
    Ray ray;
    RaycastHit hit;
    LineRenderer line;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }
    void Update()
    {
        line.SetPosition(0, tr.position);
        for (int i = 0; i < GameManager.instance.objPool.Count; i++)
        {
            if (GameManager.instance.objPool[i].activeInHierarchy) return;
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 1 << 8))
        {
            line.SetPosition(1, hit.point);
        }
        
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BASE")
        {
            rBody.Sleep();
            if (GameManager.instance.stage == 1) GameManager.instance.NextStage();
        }
    }
    void Shoot()
    {        
        if (Physics.Raycast(ray, out hit, 1 << 8))
        {
            GameManager.instance.currPoint = hit.point;
            rBody.AddForce(hit.point.normalized * 1000f);
            currPos = tr.position;
            line.enabled = false;
            StartCoroutine(ShootingBall());
            line.enabled = true;
        }
        
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

}
