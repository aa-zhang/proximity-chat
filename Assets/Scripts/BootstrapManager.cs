using FishNet;
using FishNet.Connection;
using FishNet.Managing;
using FishNet.Transporting;
using Steamworks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    private static BootstrapManager instance;
    private void Awake() => instance = this;

    [SerializeField] private string menuName = "MenuScene";
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private FishySteamworks.FishySteamworks fishySteamworks;

    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    public static ulong CurrentLobbyID;

    private void Start()
    {
        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void GoToMenu()
    {
        // This method gets called in the inspector once steam is initialized
        SceneManager.LoadScene(menuName, LoadSceneMode.Additive);
    }

    public static void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 4);
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        Debug.Log("Creating Lobby");
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            Debug.Log("Lobby creation failed: " + callback.m_eResult.ToString());
            return;
        }

        CurrentLobbyID = callback.m_ulSteamIDLobby;

        // Store host info in lobby data
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "HostAddress", SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(new CSteamID(CurrentLobbyID), "name", SteamFriends.GetPersonaName() + "'s lobby");
        fishySteamworks.SetClientAddress(SteamUser.GetSteamID().ToString());
        fishySteamworks.StartConnection(true); // Start server

        Debug.Log("Lobby creation was successful");
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        Debug.Log("Joined lobby: " + callback.m_ulSteamIDLobby);
        CurrentLobbyID = callback.m_ulSteamIDLobby;

        fishySteamworks.SetClientAddress(SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "HostAddress"));
        fishySteamworks.StartConnection(false); // Start client connection
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public static void JoinByID(CSteamID steamID)
    {
        Debug.Log("Attempting to join lobby with ID: " + steamID.m_SteamID);
        if (SteamMatchmaking.RequestLobbyData(steamID))
        {
            SteamMatchmaking.JoinLobby(steamID);
        }
        else
        {
            Debug.Log("Failed to join lobby with ID: " + steamID.m_SteamID);
        }
    }

    public static void LeaveLobby()
    {
        SteamMatchmaking.LeaveLobby(new CSteamID(CurrentLobbyID));
        CurrentLobbyID = 0;

        Debug.Log("Left lobby");
    }
}
