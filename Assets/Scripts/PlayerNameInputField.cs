using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNameInputField : MonoBehaviour {

    #region Private Variables

    // Store PlayerPref key to avoid typos
    const string playerNamePrefKey = "Playername";

    #endregion

    // Use this for initialization
    void Start ()
    {

        string defaultName = string.Empty;
        InputField inputField = this.GetComponent<InputField>();
        if (inputField != null)
        {
            if (PlayerPrefs.HasKey(playerNamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                inputField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
	}

    #region Public Methods

    /// <summary>
    /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
    /// </summary>
    /// <param name="value">The name of the player</param>
    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player name is null or empty");
            return;
        }

        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey, value);
    }

    #endregion
}
