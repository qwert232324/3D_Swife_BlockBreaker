using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Light : MonoBehaviour
{
    private Rigidbody rBody;
    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        rBody.AddForce(GameManager.instance.currPoint * 100f);
    }
    private void OnDisable()
    {        
        rBody.Sleep();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BASE")
        {
            this.gameObject.SetActive(false);
        }
    }

}
