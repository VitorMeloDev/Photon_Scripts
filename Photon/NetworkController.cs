using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks
{
    public Transform content,playersContent; //Lugar onde a lista será colocada
    public GameObject roomListItem,playersListItem,startMatchButton; //Lista a ser instanciada
    public Animator roomPlayerAnim;

    public Button buttonLeave;
    


    public TMPro.TextMeshProUGUI nickName, profileName, roomNameMenu;
    public TMPro.TMP_InputField namePlayerInput;
    public TMPro.TMP_InputField roomNameImput;
    public int maxPlayersInRoom, minPlayersInRoom;

    // public Text infoStatus;
    // public Text infoPlayerLobby;
    // public Text infoPlayerRoom;

    // string playerLobbyTxt = "Player In Lobby: ";
    // string playerRoomTxt= "Player In Room: ";
    Dictionary<string, RoomInfo> cachedRoomList;
    Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    bool matchOn = false;
    bool roomPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>();
        roomListEntries = new Dictionary<string, GameObject>();
    }

    public void FixedUpdate()
    {

    }

    public void ClearPlayersRoom()// Limpara dador antes de carregar 
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (GameObject entry in playerListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        
        playerListEntries.Clear();
    }
    public void EnterRoom() // Metodo para rodar a lista de players e instanciar eles em cena
    {

        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach(Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(playersListItem);
            entry.transform.SetParent(playersContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListInRoom>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue("PLAYER_READY", out isPlayerReady))
            {
                entry.GetComponent<PlayerListInRoom>().SetPlayerReady((bool) isPlayerReady);
            }

            playerListEntries.Add(p.ActorNumber, entry);
        }

        //ClearPlayersRoom();

            
    }

    public void CarregarSala()
    {
        if(PhotonNetwork.IsMasterClient)
        {   
            if(PhotonNetwork.CurrentRoom.PlayerCount >= minPlayersInRoom)//Bloquear sala se cheia
            {        
                //Persistent.instance.SetCursorMouse(false);
                startMatchButton.SetActive(true); 
                //Debug.Log("Ativar Botão");
            }
            else
            {
                startMatchButton.SetActive(false);
                //contLoadLevel.SetActive(false);
            }
        }
    }

    public void ButtonEnterLogin()
    {
        if(namePlayerInput.text == "")
        {
            string playerName = "Player" + Random.Range(1000,10000);
            PhotonNetwork.NickName = playerName;
            nickName.text = PhotonNetwork.NickName;
            profileName.text = PhotonNetwork.NickName;
            if(!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.NickName = namePlayerInput.text;
            nickName.text = PhotonNetwork.NickName;
            profileName.text = PhotonNetwork.NickName;
            if(!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
        }
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Conectado");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        roomNameMenu.text = PhotonNetwork.CurrentRoom.Name;
        roomPlayerAnim.Play("Open");
        roomPlayer = true;
        ClearPlayersRoom();
        EnterRoom();
        CarregarSala();
    }

    public override void OnLeftRoom()
    {
        if(roomPlayer)
        {roomPlayerAnim.Play("Stand");}
        roomPlayer = false;
        ClearPlayersRoom();   
        startMatchButton.SetActive(false);
        Debug.Log("OnLeftRoom");
    }

    public void PlayerLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void ButtonCreatRoom() //Criar Sala
    {
        
        if(!PhotonNetwork.InLobby)// Verificar se o jogador está no Lobby
        {
            return;
        }
        string nameRoom = "Room" + Random.Range(1000,10000);
        byte roomMaxPlayers = (byte) maxPlayersInRoom;

        RoomOptions options = new RoomOptions{MaxPlayers = roomMaxPlayers, IsVisible = true};

        PhotonNetwork.CreateRoom(nameRoom,options);//Criar Sala
    }

    public void ButtonCreatSpecificRoom() //Criar Sala
    {
        if(!PhotonNetwork.InLobby)// Verificar se o jogador está no Lobby
        {
            return;
        }

        if(roomNameImput.text == "")
        {
            string nameRoom = "Room" + Random.Range(1000,10000);
            roomNameImput.text = nameRoom;
        }
        byte roomMaxPlayers = (byte) maxPlayersInRoom;

        RoomOptions options = new RoomOptions{MaxPlayers = roomMaxPlayers, IsVisible = true};

        PhotonNetwork.CreateRoom(roomNameImput.text,options);//Criar Sala
    }

    public void ButtonLeaveRoom()//Jogador deixar a sala
    {
        if(!PhotonNetwork.InRoom)// Verificar se o player está na sala
        {
            return;
        }

        PhotonNetwork.LeaveRoom();//Deixar sala
        PhotonNetwork.JoinLobby();//Entrar no Lobby
    }

    public override void OnCreateRoomFailed(short returnCode, string message)//Se criação de sala falhar
    {
        Debug.Log(message);
        PhotonNetwork.JoinLobby();//Entrar no lobby para evitar erro
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomList();
        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)//Verificação quando player entra na sala
    {
        
        //Para quando um novo player entra na sala
        GameObject entry = Instantiate(playersListItem);
        entry.transform.SetParent(playersContent.transform);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListInRoom>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        object isPlayerReady;
        if (newPlayer.CustomProperties.TryGetValue("PLAYER_READY", out isPlayerReady))
        {
            entry.GetComponent<PlayerListInRoom>().SetPlayerReady((bool) isPlayerReady);
        }

        playerListEntries.Add(newPlayer.ActorNumber, entry);     
        
        if(PhotonNetwork.CurrentRoom.PlayerCount >= minPlayersInRoom)
        {
            CarregarSala();

            if(PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount)//Bloquear sala se cheia
            {       
                Debug.Log(PhotonNetwork.CurrentRoom.Name);
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
            }
        }
        
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)//Verificação quando player deixa a sala
    {
        ClearPlayersRoom();
        EnterRoom();
        CarregarSala();

        if(!PhotonNetwork.CurrentRoom.IsVisible && !PhotonNetwork.CurrentRoom.IsOpen && PhotonNetwork.CurrentRoom.MaxPlayers > PhotonNetwork.CurrentRoom.PlayerCount)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
            
            //Liberar sala quando player sair
        }
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        ClearRoomList();
    }

    void ClearRoomList()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry);
        }
        roomListEntries.Clear();
    }

    void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {

            GameObject objRoomListItem = Instantiate(roomListItem);
            objRoomListItem.transform.SetParent(content);
            //objRoomListItem.transform.position = Vector3.zero;
            objRoomListItem.transform.localScale = Vector3.one;
            objRoomListItem.GetComponent<RoomListItem>().Inicialize(info.Name,info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, objRoomListItem);
        }
    }

    void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if(!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if(cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }
                continue;
            }

            if(cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
            
    }
}
