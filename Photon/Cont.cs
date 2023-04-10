using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Cont : MonoBehaviourPunCallbacks
{
    [SerializeField]private int level;
    public void LoadLevel()
    {
        PhotonNetwork.LoadLevel(level);
    }
    
}
