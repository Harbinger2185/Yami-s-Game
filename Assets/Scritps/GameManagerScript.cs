using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    private GameObject Player;
    public bool CanTelePort;
    public bool CanBeDamaged;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        CanTelePort = true;
    }
    public void InPortal(string type, float PlayerSpeed, GameObject NextPortal){
        if (CanTelePort)
        {
            CanTelePort = false;
            if (type == "0")
            {
                Player.transform.position = NextPortal.transform.position;
            }
        }
    }
}
