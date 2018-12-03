using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    #region Photon Callbacks

    ///<summary>
    /// Called when local player left room. We need to load the launcher scene.
    ///</summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods

    static void LoadLevel()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("PhotonNetwork: Trying to Load a level but we are not the master client.");
        }

        Debug.LogFormat("PhotonNetwork: Loading level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("PunBasics-Room for " +PhotonNetwork.CurrentRoom.PlayerCount);
    }

    #endregion

    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {       
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayeEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            LoadLevel();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom() IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            LoadLevel();
        }
    }

    #endregion
}
