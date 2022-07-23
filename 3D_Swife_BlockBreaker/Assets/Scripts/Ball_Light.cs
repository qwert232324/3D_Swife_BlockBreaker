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
        rBody.velocity = GameManager.instance.currPoint * 10f;
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
            if (!GameManager.instance.isBased) return;
            GameManager.instance.NextStage();
            GameManager.instance.isShoot = false;
        }
    }

}
