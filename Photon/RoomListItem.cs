using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomListItem : MonoBehaviourPunCallbacks
{

  public RectTransform rectTransform;
  public TMPro.TextMeshProUGUI roomName; //nomeSala
  public TMPro.TextMeshProUGUI roomPlayer; //NumPlayers
  public string roomInfoConcat = "|"; //Separador
  public GameObject modal;
   
  void Start()
  {
    //modal.transform.position = new Vector3(0,0,0);
    //rectTransform.position = new Vector3(0,0,0);
  }

  void Update()
  {
    
  }
  public void Inicialize(string ps_roomName, int pi_roomPlayers, int pi_roomPlayersMax)
  {
    roomName.text = ps_roomName; //Nome da sala
    roomPlayer.text = pi_roomPlayers + roomInfoConcat + pi_roomPlayersMax; //Numero de jogadores na sala/Max
  }


  public void ButtonJoinRoom()
  {
    if(PhotonNetwork.InLobby)
    {
      PhotonNetwork.JoinRoom(roomName.text); //Entrar na sala que está o botão
    }
  }
}
