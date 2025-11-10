using UnityEngine;
using System;

/// <summary>
/// Bridge manager for communication between Unity and React Native.
/// Handles message passing via react-native-unity-view.
/// </summary>
public class UnityMessageManager : MonoBehaviour
{
    public static UnityMessageManager Instance { get; private set; }
    
    // Callback for when messages are received from React Native
    public event Action<string> OnMessageReceived;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Send message to React Native
    /// </summary>
    public void SendMessageToRN(string message)
    {
        #if UNITY_ANDROID
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject unityMessageReceiver = currentActivity.Call<AndroidJavaObject>("getUnityMessageReceiver");
            if (unityMessageReceiver != null)
            {
                unityMessageReceiver.Call("onUnityMessage", message);
            }
        }
        #elif UNITY_IOS
        // iOS implementation via native plugin
        #endif
        
        Debug.Log($"[UnityMessageManager] Sent to RN: {message}");
    }
    
    /// <summary>
    /// Receive message from React Native (called by native code)
    /// </summary>
    public void OnMessageFromRN(string message)
    {
        Debug.Log($"[UnityMessageManager] Received from RN: {message}");
        
        OnMessageReceived?.Invoke(message);
        
        // Forward to UnityEventManager if it exists
        UnityEventManager eventManager = FindObjectOfType<UnityEventManager>();
        if (eventManager != null)
        {
            eventManager.OnMessageFromNative(message);
        }
    }
}

