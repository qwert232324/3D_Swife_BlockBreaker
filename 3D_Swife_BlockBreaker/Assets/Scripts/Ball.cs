using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rBody;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BASE")
        {
            //Destroy(gameObject);
            GameManager.instance.NextStage();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.AddForce((Vector3.up+new Vector3(0.1f, 0, 0.4f)) * 300f);
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
