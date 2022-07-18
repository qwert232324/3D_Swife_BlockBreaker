using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool isShoot = false;
    private Rigidbody rBody;
    private Transform tr;
    private float h;
    private float v;
    public float speed = 3.0f;
    Vector3 currPos;
    Ray ray;
    RaycastHit hit;
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        
    }
    void Update()
    {       
        if (isShoot) return;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
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
            isShoot = false;
            rBody.Sleep();
            GameManager.instance.NextStage();
        }
    }
    void Shoot()
    {        
        if (Physics.Raycast(ray, out hit, 1 << 8))
        {
            GameManager.instance.currPoint = hit.point;
            isShoot = true;
            rBody.AddForce(hit.point * 100f);
            currPos = tr.position;
            StartCoroutine(ShootingBall());
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
