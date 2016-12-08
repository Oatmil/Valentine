using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
    
    public void RESETButton()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

}
