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

    private void Start()
    {
        //OpenMainMenu();
    }

    public void CreateLobby()
    {
        BootstrapManager.CreateLobby();

    }

    //public void OpenMainMenu()
    //{
    //    CloseAllScreens();
    //    menuScreen.SetActive(true);
    //}

    //public void OpenLobby()
    //{
    //    CloseAllScreens();
    //    lobbyScreen.SetActive(true);
    //}

    public static void LobbyEntered(string lobbyName, bool isHost)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    //void CloseAllScreens()
    //{
    //    menuScreen.SetActive(false);
    //    lobbyScreen.SetActive(false);
    //}

    public void JoinLobby()
    {
        CSteamID steamLobbyID = new CSteamID(Convert.ToUInt64(lobbyInput.text));
        BootstrapManager.JoinByID(steamLobbyID);
    }

    //public void LeaveLobby()
    //{
    //    BootstrapManager.LeaveLobby();
    //    OpenMainMenu();
    //}

    //public void StartGame()
    //{
    //    // Host only should call this.
    //    //if (!SteamManager.Initialized) return;

    //    // Close the menu scene so only the game scene stays.
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);

    //    // NOTE: The FishNet NetworkManager is inside GameScene, so it will boot up there.
    //    // The Steam lobby has already been created, so clients will join automatically.
    //}
}