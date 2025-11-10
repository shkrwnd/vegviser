using UnityEngine;

/// <summary>
/// Marker component attached to each room in the 3D building scene.
/// Stores room information and provides camera anchor point for navigation.
/// </summary>
public class RoomMarker : MonoBehaviour
{
    [Header("Room Information")]
    [SerializeField] private string roomID = "";
    [SerializeField] private string roomName = "";
    
    [Header("Camera Anchor")]
    [SerializeField] private Transform cameraAnchor;
    
    // Public properties for accessing room data
    public string RoomID => roomID;
    public string RoomName => roomName;
    public Transform CameraAnchor => cameraAnchor != null ? cameraAnchor : transform;
    
    /// <summary>
    /// Initialize room marker with data
    /// </summary>
    public void Initialize(string id, string name, Transform anchor = null)
    {
        roomID = id;
        roomName = name;
        if (anchor != null)
        {
            cameraAnchor = anchor;
        }
        else if (cameraAnchor == null)
        {
            // Create a child anchor if none exists
            GameObject anchorObj = new GameObject("CameraAnchor");
            anchorObj.transform.SetParent(transform);
            anchorObj.transform.localPosition = Vector3.zero;
            anchorObj.transform.localRotation = Quaternion.identity;
            cameraAnchor = anchorObj.transform;
        }
    }
    
    void OnValidate()
    {
        // Ensure room ID is set if name is provided
        if (string.IsNullOrEmpty(roomID) && !string.IsNullOrEmpty(roomName))
        {
            roomID = roomName.Replace(" ", "_").ToLower();
        }
    }
}

