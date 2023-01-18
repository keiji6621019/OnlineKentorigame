using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static PhotonManager instance;
    public GameObject LoadingPanel;
    public Text LoadingText;
    public GameObject buttons;
    public GameObject firstLobbybuttons;
    public GameObject createRoomPanel;
    public Text enterRoomName;
    public GameObject roomPanel;
    public Text roomName;
    public GameObject errorPanel;
    public Text errorText;
    public GameObject roomListPanel;
    public Roomview originalRoomButton;
    public GameObject roomButtonContent;
    public GameObject RoomexistButton;
    public GameObject BackButton;
    Dictionary<string, RoomInfo> roomsList = new Dictionary<string, RoomInfo>();

    private List<Roomview> allRoomButtons = new List<Roomview>();

    public Text playerNameText;
    private List<Text> allPlayerNames = new List<Text>();
    public GameObject playerNameContent;

    public GameObject nameInputPanel;
    public Text placeHolderText;
    public InputField nameInput;
    private bool setName;
    private bool atRandam;
    public GameObject startButton;
    public string LevelToPlay;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CloseMenuUI();
        LoadingPanel.SetActive(true);
        LoadingText.text = "ネットワークに接続中...";
        atRandam = false;

        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("接続");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public void CloseMenuUI()
    {
        LoadingPanel.SetActive(false);
        buttons.SetActive(false);
        createRoomPanel.SetActive(false);
        roomPanel.SetActive(false);
        errorPanel.SetActive(false);
        roomListPanel.SetActive(false);
        nameInputPanel.SetActive(false);
        firstLobbybuttons.SetActive(false);
        RoomexistButton.SetActive(false);
        BackButton.SetActive(false);
    }

    public void LobbyMenuDisplay()
    {
        CloseMenuUI();
        buttons.SetActive(true);
        BackButton.SetActive(true);

    }

    
    public void FirstLobbyMenuDisplay()
    {
        CloseMenuUI();
        firstLobbybuttons.SetActive(true);
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        LoadingText.text = "ロビーに参加中...";
        PhotonNetwork.AutomaticallySyncScene = true;


    }

    public override void OnJoinedLobby()
    {
        FirstLobbyMenuDisplay();
        roomsList.Clear();
        PhotonNetwork.NickName = Random.Range(0, 1000).ToString();
        ConfirmationName();
    }

    public void RandamRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        atRandam = true; 
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ルームの参加人数を2人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(null, roomOptions);
        Debug.Log("待機中");

    }

    public void OpenCreateRoomPanel()
    {
        CloseMenuUI();
        createRoomPanel.SetActive(true);
    } 
    
    public void OpenCreateExitPanel()
    {
        CloseMenuUI();
        RoomexistButton.SetActive(true);      
    }
    
    
    public void CreateRoomButton()
    {
        if (!string.IsNullOrEmpty(enterRoomName.text))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(enterRoomName.text, options);

            CloseMenuUI();

            LoadingText.text = "ルーム作成中...";
            LoadingPanel.SetActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        if (!atRandam)
        {
            CloseMenuUI();
            roomPanel.SetActive(true);
            roomName.text = "ルーム名：" + PhotonNetwork.CurrentRoom.Name;
            GetAllPlayer();
            CheckRoomMaster();
        }
        else
        {


            CloseMenuUI();
            LoadingPanel.SetActive(true);
            RoomexistButton.SetActive(true);
            StartCoroutine(LoadingRandamText());

        }
        
    }

    IEnumerator LoadingRandamText()
    {
        float elapsedTime = 0;

        elapsedTime += Time.deltaTime;
        while (elapsedTime < 50)
        {
            LoadingText.text = "対戦相手を探しています.";
            yield return new WaitForSeconds(0.3f);

            LoadingText.text = "対戦相手を探しています..";
            yield return new WaitForSeconds(0.3f);

            LoadingText.text = "対戦相手を探しています...";
            yield return new WaitForSeconds(0.3f);
        }      

    }



    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseMenuUI();
        LoadingText.text = "退出中...";
        LoadingPanel.SetActive(true);
        atRandam = false;
    }

    public override void OnLeftRoom()
    {
        FirstLobbyMenuDisplay();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CloseMenuUI();
        errorText.text = "ルーム作成に失敗しました" + message;
        errorPanel.SetActive(true);
        OpenCreateExitPanel();
    }
    public void FindRoom()
    {
        CloseMenuUI();
        roomListPanel.SetActive(true);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        RoomUIInitialization();

        UpdateRoomList(roomList);
        
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];

            if (info.RemovedFromList)
            {
                roomsList.Remove(info.Name);
            }
            else
            {
                roomsList[info.Name] = info;
            }
        }

        RoomListDisplay(roomsList);
    }

    public void RoomListDisplay(Dictionary<string,RoomInfo> cachedRoomList)
    {
        Debug.Log(cachedRoomList.Count);
        foreach (var roomInfo in cachedRoomList)
        {
            Roomview newButton = Instantiate(originalRoomButton);

            newButton.ResisterRoomDetails(roomInfo.Value);
            

            newButton.transform.SetParent(roomButtonContent.transform);
            allRoomButtons.Add(newButton);
        }
    }

    public void RoomUIInitialization()
    {
        foreach (Roomview rm in allRoomButtons)
        {
            Destroy(rm.gameObject);
        }

        allRoomButtons.Clear();
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);

        CloseMenuUI();

        LoadingText.text = "ルーム参加中";
        LoadingPanel.SetActive(true);
      
    }

    public void GetAllPlayer()
    {
        IntializePlayerList();

        PlayerDisplay();
    }

    public void IntializePlayerList()
    {
        foreach (var rm in allPlayerNames)
        {
            Destroy(rm.gameObject);
        }

        allPlayerNames.Clear();
    }

    public void PlayerDisplay()
    {
        foreach (var players in PhotonNetwork.PlayerList)
        {
            PlayerTextGeneration(players);
        }
    }

    public void PlayerTextGeneration(Player players)
    {
        Text newPlayerText = Instantiate(playerNameText);
        newPlayerText.text = players.NickName;
        newPlayerText.transform.SetParent(playerNameContent.transform);
        allPlayerNames.Add(newPlayerText);
    }

    public void ConfirmationName()
    {
        if (!setName)
        {
            CloseMenuUI();
            nameInputPanel.SetActive(true);

            if (PlayerPrefs.HasKey("playerName"))
            {
                placeHolderText.text = PlayerPrefs.GetString("playerName");
                nameInput.text = PlayerPrefs.GetString("playerName");
            }
        }
        else
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("playerName");
        }

        
    }

    public void SetName()
    {
        if (!string.IsNullOrEmpty(nameInput.text))
        {
            PhotonNetwork.NickName = nameInput.text;

            PlayerPrefs.SetString("playerName", nameInput.text);

            // LobbyMenuDisplay();
            FirstLobbyMenuDisplay();

            setName = true;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PlayerTextGeneration(newPlayer);

        if (atRandam)
        {
            Debug.Log("走るよ1");
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PlayGame();
                Debug.Log("走るよ2");
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetAllPlayer();
    }

    public void CheckRoomMaster()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }

    [PunRPC]
    public void PlayGame()
    {
        PhotonNetwork.LoadLevel(LevelToPlay);
    }
    

    public void Qiut()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif
    }
}
