using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    int hitCount= 9;
    MeshRenderer mesh;
    public Material[] colors = new Material[9];
    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }
    private void OnEnable()
    {
        hitCount = GameManager.instance.stage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BALL")
        {
            hitCount--;            
            if (hitCount <= 0) Break();
            mesh.material = colors[hitCount - 1];
        }
    }
    private void Break()
    {
        this.gameObject.SetActive(false);
    }

}
