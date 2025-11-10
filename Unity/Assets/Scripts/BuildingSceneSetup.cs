using UnityEngine;
using Cinemachine;

/// <summary>
/// Helper script to set up the building scene with placeholder rooms.
/// Attach this to an empty GameObject in the scene and run it once to generate rooms.
/// </summary>
public class BuildingSceneSetup : MonoBehaviour
{
    [Header("Room Configuration")]
    [SerializeField] private RoomConfig[] rooms = new RoomConfig[]
    {
        new RoomConfig { id = "lobby", name = "Lobby", position = new Vector3(0, 0, 0), size = new Vector3(10, 5, 10) },
        new RoomConfig { id = "conference", name = "Conference Room", position = new Vector3(15, 0, 0), size = new Vector3(8, 4, 8) },
        new RoomConfig { id = "office", name = "Office Space", position = new Vector3(0, 0, 15), size = new Vector3(12, 4, 10) },
        new RoomConfig { id = "cafeteria", name = "Cafeteria", position = new Vector3(-15, 0, 0), size = new Vector3(10, 5, 12) }
    };
    
    [Header("Materials")]
    [SerializeField] private Material roomMaterial;
    [SerializeField] private Material floorMaterial;
    
    [Header("Camera Setup")]
    [SerializeField] private bool setupCamera = true;
    [SerializeField] private Vector3 initialCameraPosition = new Vector3(0, 10, -15);
    
    [System.Serializable]
    public class RoomConfig
    {
        public string id;
        public string name;
        public Vector3 position;
        public Vector3 size;
    }
    
    [ContextMenu("Generate Building Scene")]
    void GenerateBuildingScene()
    {
        // Create parent object for rooms
        GameObject roomsParent = new GameObject("Rooms");
        
        // Generate each room
        foreach (var roomConfig in rooms)
        {
            CreateRoom(roomConfig, roomsParent.transform);
        }
        
        // Create floor
        CreateFloor();
        
        // Setup camera
        if (setupCamera)
        {
            SetupCamera();
        }
        
        Debug.Log("Building scene generated successfully!");
    }
    
    void CreateRoom(RoomConfig config, Transform parent)
    {
        // Create room GameObject
        GameObject roomObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        roomObj.name = config.name;
        roomObj.transform.SetParent(parent);
        roomObj.transform.position = config.position;
        roomObj.transform.localScale = config.size;
        
        // Apply material
        Renderer renderer = roomObj.GetComponent<Renderer>();
        if (roomMaterial != null)
        {
            renderer.material = roomMaterial;
        }
        else
        {
            // Create a simple colored material
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = new Color(
                UnityEngine.Random.Range(0.3f, 0.8f),
                UnityEngine.Random.Range(0.3f, 0.8f),
                UnityEngine.Random.Range(0.3f, 0.8f),
                1f
            );
            renderer.material = mat;
        }
        
        // Add collider (already added by CreatePrimitive)
        Collider collider = roomObj.GetComponent<Collider>();
        if (collider == null)
        {
            roomObj.AddComponent<BoxCollider>();
        }
        
        // Add RoomMarker component
        RoomMarker marker = roomObj.AddComponent<RoomMarker>();
        marker.Initialize(config.id, config.name);
        
        // Create camera anchor as child
        GameObject anchorObj = new GameObject("CameraAnchor");
        anchorObj.transform.SetParent(roomObj.transform);
        anchorObj.transform.localPosition = new Vector3(0, config.size.y * 0.5f + 2f, -config.size.z * 0.5f - 3f);
        anchorObj.transform.LookAt(roomObj.transform.position);
        
        // Update marker with anchor
        marker.Initialize(config.id, config.name, anchorObj.transform);
    }
    
    void CreateFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.name = "Floor";
        floor.transform.position = new Vector3(0, -0.5f, 0);
        floor.transform.localScale = new Vector3(5, 1, 5);
        
        Renderer renderer = floor.GetComponent<Renderer>();
        if (floorMaterial != null)
        {
            renderer.material = floorMaterial;
        }
        else
        {
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            renderer.material = mat;
        }
    }
    
    void SetupCamera()
    {
        // Find or create main camera
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            mainCam = camObj.AddComponent<Camera>();
            camObj.tag = "MainCamera";
        }
        
        // Setup Cinemachine Virtual Camera
        GameObject vcamObj = new GameObject("CM vcam1");
        CinemachineVirtualCamera vcam = vcamObj.AddComponent<CinemachineVirtualCamera>();
        vcam.Priority = 10;
        
        // Create follow target
        GameObject followTarget = new GameObject("CameraFollowTarget");
        followTarget.transform.position = initialCameraPosition;
        vcam.Follow = followTarget.transform;
        vcam.LookAt = followTarget.transform;
        
        // Add orbital transposer
        CinemachineOrbitalTransposer transposer = vcam.AddCinemachineComponent<CinemachineOrbitalTransposer>();
        transposer.m_FollowOffset = new Vector3(0, 5, -10);
        
        // Add MobileInteractionController
        GameObject interactionObj = new GameObject("MobileInteractionController");
        MobileInteractionController controller = interactionObj.AddComponent<MobileInteractionController>();
        
        // Set camera references programmatically
        controller.SetVirtualCamera(vcam);
        controller.SetMainCamera(mainCam);
        
        // Add UnityEventManager
        GameObject eventManagerObj = new GameObject("UnityEventManager");
        eventManagerObj.AddComponent<UnityEventManager>();
        
        // Add UnityMessageManager
        GameObject messageManagerObj = new GameObject("UnityMessageManager");
        messageManagerObj.AddComponent<UnityMessageManager>();
    }
}

