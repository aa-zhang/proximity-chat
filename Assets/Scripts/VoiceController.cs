using System;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Vivox;

public class VoiceController : MonoBehaviour
{
    public static VoiceController instance;
    private void Awake() => instance = this;
    public event Action OnVivoxReady;


    private async void Start()
    {
        await InitializeServicesAsync();
        await LoginVivoxAsync();
        //await JoinEchoChannelAsync();
        await JoinPositionalChannelAsync();
        OnVivoxReady?.Invoke();
    }

    private async System.Threading.Tasks.Task InitializeServicesAsync()
    {
        await UnityServices.InitializeAsync();
        Debug.Log("IsSignedIn before login: " + AuthenticationService.Instance.IsSignedIn);
        Debug.Log("PlayerID: " + AuthenticationService.Instance.PlayerId);


        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in. PlayerID: " + AuthenticationService.Instance.PlayerId);
        }
        else
        {
            Debug.Log("Already signed in. PlayerID: " + AuthenticationService.Instance.PlayerId);
        }
        await VivoxService.Instance.InitializeAsync();

    }

    private async System.Threading.Tasks.Task LoginVivoxAsync()
    {
        Debug.Log("Logging into Vivox...");
        LoginOptions options = new LoginOptions();
        options.DisplayName = "Test User 2";
        options.EnableTTS = true;

        await VivoxService.Instance.LoginAsync(options);
        Debug.Log("Vivox login successful.");
    }

    private async System.Threading.Tasks.Task JoinEchoChannelAsync()
    {
        Debug.Log("Joining echo channel...");
        string channelName = "Lobby";
        await VivoxService.Instance.JoinEchoChannelAsync(channelName, ChatCapability.TextAndAudio);
        Debug.Log("Joined echo channel: " + channelName);
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
                audibleDistance: 10,   // Max distance a player can be heard
                conversationalDistance: 2, // Full volume within this distance
                audioFadeIntensityByDistanceaudio: 1.0f, // How fast volume fades with distance
                audioFadeModel: AudioFadeModel.InverseByDistance // Fade curve
            );

            // Optional: you can set this null unless you need special join behavior
            ChannelOptions channelOptions = null;

            await VivoxService.Instance.JoinPositionalChannelAsync(
                channelName,
                chatCapability,
                positionalChannelProperties,
                channelOptions
            );

            Debug.Log("Successfully joined positional channel: " + channelName);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to join positional channel: " + e.Message);
        }
    }

}
