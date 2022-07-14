using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    int hitCount;
    MeshRenderer mesh;
    public Material[] colors = new Material[9];
    // Start is called before the first frame update
    void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        hitCount = GameManager.instance.stage;
        mesh.material = colors[(hitCount - 1) % 9];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BALL")
        {
            if (--hitCount == 0)
            {
                Break();
                return;
            }
            mesh.material = colors[(hitCount - 1) % 9];
        }
    }
    private void Break()
    {
        this.gameObject.SetActive(false);
    }

}
