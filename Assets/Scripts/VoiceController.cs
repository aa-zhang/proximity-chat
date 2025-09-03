using Steamworks;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Vivox;
using UnityEngine;

public class VoiceController : MonoBehaviour
{
    public static VoiceController instance;
    private void Awake() => instance = this;
    public event Action OnVivoxReady;


    private async void Start()
    {
        await InitializeServicesAsync();
        await LoginVivoxAsync();
        await JoinPositionalChannelAsync();
        OnVivoxReady?.Invoke();
    }

    private async System.Threading.Tasks.Task InitializeServicesAsync()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        await VivoxService.Instance.InitializeAsync();

    }

    private async System.Threading.Tasks.Task LoginVivoxAsync()
    {
        Debug.Log("Logging into Vivox...");
        LoginOptions options = new LoginOptions();
        options.DisplayName = SteamFriends.GetPersonaName();
        options.EnableTTS = true;

        await VivoxService.Instance.LoginAsync(options);
        Debug.Log("Vivox login successful.");
    }

    private async System.Threading.Tasks.Task JoinPositionalChannelAsync()
    {
        try
        {
            Debug.Log("Joining positional channel...");

            string channelName = "GameWorld";

            // Choose what the channel supports
            ChatCapability chatCapability = ChatCapability.AudioOnly;

            // Define 3D positional properties
            Channel3DProperties positionalChannelProperties = new Channel3DProperties(
                audibleDistance: 50,   // Max distance a player can be heard
                conversationalDistance: 1, // Full volume within this distance
                audioFadeIntensityByDistanceaudio: 1.0f, // How fast volume fades with distance
                audioFadeModel: AudioFadeModel.InverseByDistance // Fade curve
            );

            await VivoxService.Instance.JoinPositionalChannelAsync(
                channelName,
                chatCapability,
                positionalChannelProperties
            );

            Debug.Log("Successfully joined positional channel: " + channelName);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to join positional channel: " + e.Message);
        }
    }

}
