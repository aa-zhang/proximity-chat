using System;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    private static MainMenuManager instance;

    [SerializeField] private GameObject menuScreen, lobbyScreen;
    [SerializeField] private TMP_InputField lobbyInput;

    [SerializeField] private TextMeshProUGUI lobbyTitle, lobbyIDText;
    [SerializeField] private Button startGameButton;
    private void Awake() => instance = this;

    public void CreateLobby()
    {
        BootstrapManager.CreateLobby();
    }

    public void JoinLobby()
    {
        CSteamID steamLobbyID = new CSteamID(Convert.ToUInt64(lobbyInput.text));
        BootstrapManager.JoinByID(steamLobbyID);
    }

}