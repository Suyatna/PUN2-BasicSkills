using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{

    #region Private Field

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "v.alpha.1";

    /// <summary>
    /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
    /// </summary>
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon.
    /// We need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    bool isConnecting;

    #endregion

    #region Public Variable

    [Tooltip("The UI Panel to left the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    #endregion

    #region MonoBehaviour Callbacks

    private void Awake()
    {
        // #Critical
        // This makes sure we can use PhotonNetwork.Loadlevel() on the master client in the same room sync their level automaticlly
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Start the connection process.
    /// - if already connected, we attempt joining a random room
    /// - if not yet connected, connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // Keep track of the will join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then.
        isConnecting = true;

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        // We check if we were connected or not, we join if we are, else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #endregion

    #region MonoBehaviourCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        // We don't want to do anything if we are not attempting to join a room.
        // This case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster() will be called, in that case.
        // We don't want to do anything.
        if (isConnecting)
        {
            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }        

        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

        Debug.Log("PUN Basics Tutorial/Launcher: OnDisconnected by PUN with reasin {0}");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basic Tutorial/Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we create one. \n Calling: PhotonNetwork.CreateRoom()");

        // #Critical: we failed to join a random room, maybe none exists or they full. No worries, we a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Basic Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client in the room.");

        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySycnScene` to sycn our instance

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the `Room for 1` ");

            // #Critical: We load the Room level.
            PhotonNetwork.LoadLevel("PunBasics-Room for 1");
        }
    }

    #endregion
}