using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PickUpsManager : MonoBehaviour {

    public List<GameObject> L_PickUps = new List<GameObject>();
    public List<GameObject> L_PickUps_Spawn = new List<GameObject>();
    List<bool> L_PickUps_Spawn_Bool = new List<bool>();

    public float SpawnRate;
    float tempTime;

    public static PickUpsManager PickMng_instance;

    void Awake()
    {
        if (PickMng_instance == null)
            PickMng_instance = this;
    }

    void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("PickUps_Spawn"))
        {
            L_PickUps_Spawn.Add(obj);
            L_PickUps_Spawn_Bool.Add(false);
        }
    }


    void Update()
    {
        tempTime += Time.deltaTime;
        if (tempTime >= SpawnRate)
        {
            StartCoroutine(SpawnPickUps());
        }
    }

    IEnumerator SpawnPickUps()
    {
        int i = Random.Range(0, L_PickUps_Spawn.Count);
        if (!L_PickUps_Spawn_Bool[i])
        {
            L_PickUps_Spawn_Bool[i] = true;
            int j = Random.Range(0, L_PickUps.Count);
            GameObject tempObj = Instantiate(L_PickUps[j], L_PickUps_Spawn[i].transform.position, Quaternion.identity) as GameObject;
        }
        tempTime = 0;
        yield return null;
    }

    public void PickUpsUsed(Vector3 Pos)
    {
        for (int i = 0; i < L_PickUps_Spawn.Count; i++)
        {
            if (L_PickUps_Spawn[i].transform.position == Pos)
            {
                L_PickUps_Spawn_Bool[i] = false;
            }
        }
    }
}
