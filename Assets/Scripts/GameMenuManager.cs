using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    private static GameMenuManager instance;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private TextMeshProUGUI lobbyIDText;

    private void Awake() => instance = this;


    void Start()
    {
        instance.menuUI.SetActive(false);
    }


    public static bool ToggleGameMenu()
    {
        if (instance.menuUI.activeSelf)
        {
            instance.SetMenuState(false);
        }
        else
        {
            instance.SetMenuState(true);
        }

        return instance.menuUI.activeSelf;
    }


    private void SetMenuState(bool isOpen)
    {
        instance.menuUI.SetActive(isOpen);
    }

    public void CopyLobbyID()
    {
        GUIUtility.systemCopyBuffer = lobbyIDText.text;
    }



}
