using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class StartMatch : MonoBehaviour
{

    public void CarregaCena(int x)
    {
        //Debug.Log("Carregar Cena " + x);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        LoadLevel(x);
        
    }

    public void LoadLevel(int level)
    {
        PhotonNetwork.LoadLevel(level);
    }
}
