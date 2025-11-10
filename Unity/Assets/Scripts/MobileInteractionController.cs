using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

/// <summary>
/// Handles mobile touch interactions: tap to navigate, pinch-to-zoom, and one-finger orbit.
/// Manages Cinemachine camera animations for smooth room transitions.
/// </summary>
public class MobileInteractionController : MonoBehaviour
{
    [Header("Camera Setup")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Camera mainCamera;
    
    [Header("Interaction Settings")]
    [SerializeField] private float orbitSpeed = 2f;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float transitionDuration = 1.5f;
    
    [Header("Room Navigation")]
    [SerializeField] private LayerMask roomLayerMask = -1;
    
    private CinemachineOrbitalTransposer orbitalTransposer;
    private float currentZoom = 10f;
    private float currentOrbitAngle = 0f;
    
    // Touch input tracking
    private Vector2 lastTouchPosition;
    private float lastTouchDistance;
    private bool isTransitioning = false;
    private RoomMarker currentRoom;
    
    // Room markers in scene
    private List<RoomMarker> roomMarkers = new List<RoomMarker>();
    
    void Start()
    {
        // Find all room markers in scene
        roomMarkers.AddRange(FindObjectsOfType<RoomMarker>());
        
        // Setup Cinemachine
        if (virtualCamera != null)
        {
            orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            if (orbitalTransposer != null)
            {
                currentOrbitAngle = orbitalTransposer.m_XAxis.Value;
            }
            
            // Get initial zoom from camera distance
            if (orbitalTransposer != null)
            {
                currentZoom = orbitalTransposer.m_FollowOffset.magnitude;
            }
        }
        else
        {
            // Try to find virtual camera automatically
            virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            if (virtualCamera != null)
            {
                orbitalTransposer = virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
                if (orbitalTransposer != null)
                {
                    currentOrbitAngle = orbitalTransposer.m_XAxis.Value;
                    currentZoom = orbitalTransposer.m_FollowOffset.magnitude;
                }
            }
        }
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    
    void Update()
    {
        if (isTransitioning) return;
        
        HandleTouchInput();
    }
    
    /// <summary>
    /// Process touch input for mobile interactions
    /// </summary>
    void HandleTouchInput()
    {
        if (Input.touchCount == 0) return;
        
        Touch touch = Input.GetTouch(0);
        
        // Single touch: tap to navigate or orbit
        if (Input.touchCount == 1)
        {
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                
                // Check for tap (quick touch and release)
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, roomLayerMask))
                {
                    RoomMarker room = hit.collider.GetComponent<RoomMarker>();
                    if (room != null)
                    {
                        NavigateToRoom(room);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // One-finger orbit
                Vector2 deltaPosition = touch.position - lastTouchPosition;
                OrbitCamera(deltaPosition.x * orbitSpeed * Time.deltaTime);
                lastTouchPosition = touch.position;
            }
        }
        // Two touches: pinch-to-zoom
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            
            if (touch2.phase == TouchPhase.Began)
            {
                lastTouchDistance = Vector2.Distance(touch1.position, touch2.position);
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float deltaDistance = lastTouchDistance - currentDistance;
                
                ZoomCamera(deltaDistance * zoomSpeed);
                lastTouchDistance = currentDistance;
            }
        }
    }
    
    /// <summary>
    /// Navigate camera to a specific room with smooth animation
    /// </summary>
    public void NavigateToRoom(RoomMarker room)
    {
        if (room == null || isTransitioning) return;
        
        currentRoom = room;
        isTransitioning = true;
        
        // Notify UnityEventManager of room change
        UnityEventManager eventManager = FindObjectOfType<UnityEventManager>();
        if (eventManager != null)
        {
            // Optionally send room change event to React Native
        }
        
        // Animate camera to room anchor
        StartCoroutine(AnimateToRoom(room.CameraAnchor));
    }
    
    /// <summary>
    /// Smoothly animate camera to target room anchor
    /// </summary>
    System.Collections.IEnumerator AnimateToRoom(Transform targetAnchor)
    {
        if (virtualCamera == null || targetAnchor == null) 
        {
            isTransitioning = false;
            yield break;
        }
        
        Transform followTarget = virtualCamera.Follow;
        if (followTarget == null)
        {
            // Create follow target if it doesn't exist
            GameObject followObj = new GameObject("CameraFollowTarget");
            followObj.transform.position = targetAnchor.position;
            virtualCamera.Follow = followObj.transform;
            followTarget = followObj.transform;
        }
        
        Vector3 startPosition = followTarget.position;
        Quaternion startRotation = followTarget.rotation;
        
        Vector3 targetPosition = targetAnchor.position;
        Quaternion targetRotation = targetAnchor.rotation;
        
        float elapsed = 0f;
        
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);
            
            followTarget.position = Vector3.Lerp(startPosition, targetPosition, t);
            followTarget.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            
            yield return null;
        }
        
        // Ensure final position
        followTarget.position = targetPosition;
        followTarget.rotation = targetRotation;
        
        isTransitioning = false;
    }
    
    /// <summary>
    /// Orbit camera around current focus point
    /// </summary>
    void OrbitCamera(float deltaAngle)
    {
        if (orbitalTransposer != null)
        {
            currentOrbitAngle += deltaAngle;
            orbitalTransposer.m_XAxis.Value = currentOrbitAngle;
        }
    }
    
    /// <summary>
    /// Zoom camera in/out
    /// </summary>
    void ZoomCamera(float deltaZoom)
    {
        if (orbitalTransposer != null)
        {
            currentZoom = Mathf.Clamp(currentZoom + deltaZoom, minZoom, maxZoom);
            Vector3 offset = orbitalTransposer.m_FollowOffset.normalized * currentZoom;
            orbitalTransposer.m_FollowOffset = offset;
        }
    }
    
    /// <summary>
    /// Get current room being viewed
    /// </summary>
    public RoomMarker GetCurrentRoom()
    {
        return currentRoom;
    }
    
    /// <summary>
    /// Get all available rooms
    /// </summary>
    public List<RoomMarker> GetAvailableRooms()
    {
        return new List<RoomMarker>(roomMarkers);
    }
    
    /// <summary>
    /// Set virtual camera reference (for programmatic setup)
    /// </summary>
    public void SetVirtualCamera(CinemachineVirtualCamera vcam)
    {
        virtualCamera = vcam;
        if (vcam != null)
        {
            orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            if (orbitalTransposer != null)
            {
                currentOrbitAngle = orbitalTransposer.m_XAxis.Value;
                currentZoom = orbitalTransposer.m_FollowOffset.magnitude;
            }
        }
    }
    
    /// <summary>
    /// Set main camera reference (for programmatic setup)
    /// </summary>
    public void SetMainCamera(Camera cam)
    {
        mainCamera = cam;
    }
}
