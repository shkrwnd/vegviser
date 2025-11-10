using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages events per room and communicates with React Native via JSON messages.
/// Handles Unity-to-Native bridge communication.
/// </summary>
public class UnityEventManager : MonoBehaviour
{
    [Header("Event Configuration")]
    [SerializeField] private bool autoSendEventsOnStart = true;
    
    // Dictionary to store events per room
    private Dictionary<string, List<RoomEvent>> roomEvents = new Dictionary<string, List<RoomEvent>>();
    
    // Singleton instance for easy access
    public static UnityEventManager Instance { get; private set; }
    
    // Event data structure
    [Serializable]
    public class RoomEvent
    {
        public string eventID;
        public string eventName;
        public string eventDescription;
        public string eventTime;
        public string eventType;
        
        public RoomEvent(string id, string name, string description, string time, string type)
        {
            eventID = id;
            eventName = name;
            eventDescription = description;
            eventTime = time;
            eventType = type;
        }
    }
    
    // JSON-serializable structure for sending to React Native
    [Serializable]
    public class RoomEventData
    {
        public string roomID;
        public string roomName;
        public List<RoomEvent> events;
    }
    
    [Serializable]
    public class AllEventsData
    {
        public List<RoomEventData> rooms;
    }
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDefaultEvents();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (autoSendEventsOnStart)
        {
            SendAllEventsToNative();
        }
    }
    
    /// <summary>
    /// Initialize default events for demonstration
    /// </summary>
    void InitializeDefaultEvents()
    {
        // Example events for different rooms
        AddEventToRoom("lobby", "Lobby", new RoomEvent(
            "evt_001",
            "Welcome Reception",
            "Daily welcome reception for new visitors. Refreshments provided.",
            "09:00 - 10:00",
            "Reception"
        ));
        
        AddEventToRoom("lobby", "Lobby", new RoomEvent(
            "evt_002",
            "Security Briefing",
            "Mandatory security briefing for all staff members.",
            "14:00 - 15:00",
            "Meeting"
        ));
        
        AddEventToRoom("conference", "Conference Room", new RoomEvent(
            "evt_003",
            "Quarterly Review",
            "Q4 quarterly business review meeting with stakeholders.",
            "10:00 - 12:00",
            "Meeting"
        ));
        
        AddEventToRoom("conference", "Conference Room", new RoomEvent(
            "evt_004",
            "Product Launch",
            "New product launch presentation and demo.",
            "15:00 - 16:30",
            "Presentation"
        ));
        
        AddEventToRoom("office", "Office Space", new RoomEvent(
            "evt_005",
            "Team Standup",
            "Daily team standup meeting for development team.",
            "09:30 - 10:00",
            "Meeting"
        ));
        
        AddEventToRoom("cafeteria", "Cafeteria", new RoomEvent(
            "evt_006",
            "Lunch Service",
            "Lunch service available with daily specials.",
            "12:00 - 14:00",
            "Service"
        ));
    }
    
    /// <summary>
    /// Add an event to a specific room
    /// </summary>
    public void AddEventToRoom(string roomID, string roomName, RoomEvent roomEvent)
    {
        if (!roomEvents.ContainsKey(roomID))
        {
            roomEvents[roomID] = new List<RoomEvent>();
        }
        
        roomEvents[roomID].Add(roomEvent);
        
        // Notify React Native of the update
        SendRoomEventsToNative(roomID, roomName);
    }
    
    /// <summary>
    /// Remove an event from a room
    /// </summary>
    public void RemoveEventFromRoom(string roomID, string eventID)
    {
        if (roomEvents.ContainsKey(roomID))
        {
            roomEvents[roomID].RemoveAll(e => e.eventID == eventID);
            SendRoomEventsToNative(roomID, GetRoomName(roomID));
        }
    }
    
    /// <summary>
    /// Get all events for a specific room
    /// </summary>
    public List<RoomEvent> GetRoomEvents(string roomID)
    {
        if (roomEvents.ContainsKey(roomID))
        {
            return new List<RoomEvent>(roomEvents[roomID]);
        }
        return new List<RoomEvent>();
    }
    
    /// <summary>
    /// Get all events from all rooms
    /// </summary>
    public AllEventsData GetAllEvents()
    {
        AllEventsData allData = new AllEventsData();
        allData.rooms = new List<RoomEventData>();
        
        foreach (var kvp in roomEvents)
        {
            RoomEventData roomData = new RoomEventData
            {
                roomID = kvp.Key,
                roomName = GetRoomName(kvp.Key),
                events = new List<RoomEvent>(kvp.Value)
            };
            allData.rooms.Add(roomData);
        }
        
        return allData;
    }
    
    /// <summary>
    /// Send all events to React Native as JSON
    /// </summary>
    public void SendAllEventsToNative()
    {
        AllEventsData allData = GetAllEvents();
        string json = JsonUtility.ToJson(allData);
        
        Debug.Log($"[UnityEventManager] Sending all events to React Native: {json}");
        SendMessageToNative("EVENTS_UPDATE", json);
    }
    
    /// <summary>
    /// Send events for a specific room to React Native
    /// </summary>
    void SendRoomEventsToNative(string roomID, string roomName)
    {
        RoomEventData roomData = new RoomEventData
        {
            roomID = roomID,
            roomName = roomName,
            events = GetRoomEvents(roomID)
        };
        
        string json = JsonUtility.ToJson(roomData);
        Debug.Log($"[UnityEventManager] Sending room events to React Native: {json}");
        SendMessageToNative("ROOM_EVENTS_UPDATE", json);
    }
    
    /// <summary>
    /// Send message to React Native via bridge
    /// </summary>
    void SendMessageToNative(string messageType, string jsonData)
    {
        // Format: TYPE|JSON_DATA
        string message = $"{messageType}|{jsonData}";
        
        // Use Unity's native messaging system
        // This will be handled by react-native-unity-view bridge
        try
        {
            // For react-native-unity-view, we use UnityMessageManager
            #if UNITY_ANDROID || UNITY_IOS
            UnityMessageManager.Instance.SendMessageToRN(message);
            #else
            Debug.Log($"[UnityEventManager] Would send to RN: {message}");
            #endif
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[UnityEventManager] Failed to send message to RN: {e.Message}");
        }
    }
    
    /// <summary>
    /// Receive message from React Native
    /// </summary>
    public void OnMessageFromNative(string message)
    {
        Debug.Log($"[UnityEventManager] Received from React Native: {message}");
        
        // Parse message format: COMMAND|PARAMS
        string[] parts = message.Split('|');
        if (parts.Length < 1) return;
        
        string command = parts[0];
        string paramsJson = parts.Length > 1 ? parts[1] : "";
        
        switch (command)
        {
            case "NAVIGATE_TO_ROOM":
                HandleNavigateToRoom(paramsJson);
                break;
            case "HIGHLIGHT_ROOM":
                HandleHighlightRoom(paramsJson);
                break;
            case "REQUEST_EVENTS":
                SendAllEventsToNative();
                break;
            default:
                Debug.LogWarning($"[UnityEventManager] Unknown command: {command}");
                break;
        }
    }
    
    /// <summary>
    /// Handle navigation command from React Native
    /// </summary>
    void HandleNavigateToRoom(string paramsJson)
    {
        try
        {
            var data = JsonUtility.FromJson<NavigateParams>(paramsJson);
            RoomMarker room = FindRoomByID(data.roomID);
            
            if (room != null)
            {
                MobileInteractionController controller = FindObjectOfType<MobileInteractionController>();
                if (controller != null)
                {
                    controller.NavigateToRoom(room);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[UnityEventManager] Failed to navigate: {e.Message}");
        }
    }
    
    /// <summary>
    /// Handle room highlighting command from React Native
    /// </summary>
    void HandleHighlightRoom(string paramsJson)
    {
        // Implement room highlighting logic here
        Debug.Log($"[UnityEventManager] Highlight room: {paramsJson}");
    }
    
    /// <summary>
    /// Find room marker by ID
    /// </summary>
    RoomMarker FindRoomByID(string roomID)
    {
        RoomMarker[] allRooms = FindObjectsOfType<RoomMarker>();
        foreach (var room in allRooms)
        {
            if (room.RoomID == roomID)
            {
                return room;
            }
        }
        return null;
    }
    
    /// <summary>
    /// Get room name by ID
    /// </summary>
    string GetRoomName(string roomID)
    {
        RoomMarker room = FindRoomByID(roomID);
        return room != null ? room.RoomName : roomID;
    }
    
    // Parameter classes for JSON deserialization
    [Serializable]
    class NavigateParams
    {
        public string roomID;
    }
}

