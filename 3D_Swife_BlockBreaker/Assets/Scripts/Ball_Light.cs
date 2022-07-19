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
        rBody.AddForce(GameManager.instance.currPoint.normalized * 1000f);
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
            for (int i = 0; i < GameManager.instance.objPool.Count; i++)
            {
                if (GameManager.instance.objPool[i].activeInHierarchy) return;
            }
            GameManager.instance.NextStage();
        }
    }

}
