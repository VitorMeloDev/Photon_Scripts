using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class SpawManager : MonoBehaviour
{
    [SerializeField] private Transform[] posSpaw;
    int playerPos;
    //[SerializeField] private Transform posPlayer;

    private void Awake()
    {
        //PhotonNetwork.Instantiate(Persistent.instance.heros[Persistent.instance.idCharacter].name,posSpaw[Random.Range(0,3)].position,Quaternion.identity, 0);
        SpawPos();
        
    }

    void SpawPos()
    {
        playerPos = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        PhotonNetwork.Instantiate(Persistent.instance.heros[Persistent.instance.idCharacter].name,posSpaw[playerPos].position,Quaternion.identity, 0);
        
    }
}
