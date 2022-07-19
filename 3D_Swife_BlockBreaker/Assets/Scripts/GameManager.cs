using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Floor
{
    public Transform floorPos;
    public GameObject[] blocks = new GameObject[25];
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Floor[] floors = new Floor[9];
    public int stage = 1;
    public Vector3 currPoint;
    private int idx = 0;

    public GameObject ballPrefab;
    public List<GameObject> objPool = new List<GameObject>();

    void Awake()
    {
        if (instance == null) instance = this;

        GameObject ballPool = new GameObject("Ball Pool");

        for (int i = 0; i < 100; i++)
        {
            GameObject obj = Instantiate(ballPrefab, ballPool.transform);
            obj.name = "Ball_" + i.ToString();
            obj.SetActive(false);
            objPool.Add(obj);
        }
    }

    void Start()
    {       
        for(int i = 0; i < floors[0].blocks.Length; i++)
        {
            floors[0].blocks[i].gameObject.SetActive(Random.value > 0.5f);
        }
    }

    public GameObject GetBall()
    {
        for (int i = 0; i < objPool.Count; i++)
        {
            if (objPool[i].activeSelf == false)
            {
                return objPool[i];
            }
        }
        return null;
    }
    public void NextStage()
    {
        stage++;

        for(int i = 0; i < floors.Length; i++)
        {
            if (floors[i].floorPos.position.y < 0.1f)
            {
                floors[i].floorPos.position = new Vector3(0, 4.8f, 0);
                continue;
            }
            floors[i].floorPos.position = Vector3.Lerp(floors[i].floorPos.position, floors[i].floorPos.position + Vector3.up * -0.6f, 1f);
        }

        for (int i = 0; i < floors[8-idx].blocks.Length; i++)
        {
            floors[8 - idx].blocks[i].gameObject.SetActive(Random.value > 0.5f);
        }

        if (++idx == 9) idx = 0;


    }
}