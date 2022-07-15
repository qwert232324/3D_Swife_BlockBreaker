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
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 position = tr.position;
        position.x += h * speed * Time.deltaTime;
        position.z += v * speed * Time.deltaTime;
        tr.position = position;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BASE")
        {
            //Destroy(gameObject);
            GameManager.instance.NextStage();
        }
    }
    void Shoot()
    {        
        if (Physics.Raycast(ray, out hit, 1 << 8))
        {
            isShoot = true;
            rBody.AddForce(hit.point * 100f);
        }
    }
}
