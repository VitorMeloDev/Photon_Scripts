using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class PlayerListInRoom : MonoBehaviourPunCallbacks
{
    public Text PlayerNameText;
    //public Text readyButton;
    private int ownerId;        
//    private bool isPlayerReady;

    //public Button PlayerReadyButton;


    // Start is called before the first frame update
    void Start()
    {
        /*if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
            {
                PlayerReadyButton.gameObject.SetActive(false);
            }
            else
            {
                Hashtable initialProps = new Hashtable() {{"PLAYER_READY", isPlayerReady}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

                PlayerReadyButton.onClick.AddListener(() =>
                {
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);

                    Hashtable props = new Hashtable() {{"PLAYER_READY", isPlayerReady}};
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        //FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                    }
                });
            }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
    }

    public void SetPlayerReady(bool playerReady)
    {
        //readyButton.text = playerReady ? "Ready!" : "No Ready";
        //PlayerReadyImage.enabled = playerReady;
    }

}
