using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {
    
    List<GameObject> L_CharcList = new List<GameObject>();
    Vector3 tempPos;

    void Start()
    {
        foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            L_CharcList.Add(Obj);
        }
    }

    void Update()
    {
        foreach( GameObject Obj in L_CharcList)
        {
            tempPos += Obj.transform.position / L_CharcList.Count;
        }
        transform.position = Vector3.Lerp(transform.position, tempPos, 0.5f);
        tempPos = new Vector3(0.0f, 0.0f, -10);
    }
}
