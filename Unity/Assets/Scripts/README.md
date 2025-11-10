# Unity Scripts Documentation

## Overview

This directory contains all C# scripts for the Unity 3D building explorer.

## Scripts

### RoomMarker.cs
**Purpose**: Marker component attached to each room in the 3D scene.

**Properties**:
- `RoomID`: Unique identifier for the room
- `RoomName`: Display name of the room
- `CameraAnchor`: Transform where the camera should focus when navigating to this room

**Usage**:
```csharp
RoomMarker marker = roomObject.GetComponent<RoomMarker>();
marker.Initialize("lobby", "Main Lobby", anchorTransform);
```

### MobileInteractionController.cs
**Purpose**: Handles all mobile touch interactions and camera control.

**Features**:
- **Tap to Navigate**: Tap a room to smoothly animate camera to that room
- **Pinch-to-Zoom**: Two-finger pinch gesture to zoom in/out
- **One-Finger Orbit**: Single finger drag to orbit around current focus point

**Configuration**:
- `orbitSpeed`: Speed of orbit rotation
- `zoomSpeed`: Speed of zoom
- `minZoom` / `maxZoom`: Zoom limits
- `transitionDuration`: Camera animation duration

**Usage**:
Attach to a GameObject in the scene. Assign the Cinemachine Virtual Camera and Main Camera references.

### UnityEventManager.cs
**Purpose**: Manages events per room and communicates with React Native.

**Features**:
- Stores events for each room
- Sends JSON updates to React Native
- Receives commands from React Native (navigation, highlighting)

**Event Structure**:
```csharp
RoomEvent {
    eventID: string
    eventName: string
    eventDescription: string
    eventTime: string
    eventType: string
}
```

**Message Format**:
- To React Native: `EVENTS_UPDATE|{json}`
- From React Native: `NAVIGATE_TO_ROOM|{roomID}`

### UnityMessageManager.cs
**Purpose**: Bridge for bidirectional communication between Unity and React Native.

**Methods**:
- `SendMessageToRN(string message)`: Send message to React Native
- `OnMessageFromRN(string message)`: Receive message from React Native (called by native code)

**Usage**:
Automatically initialized as singleton. Other scripts can access via `UnityMessageManager.Instance`.

### BuildingSceneSetup.cs
**Purpose**: Helper script to generate placeholder building scene.

**Features**:
- Creates 4 placeholder rooms (cubes)
- Sets up colliders and RoomMarker components
- Creates camera anchors
- Configures Cinemachine camera
- Sets up MobileInteractionController

**Usage**:
1. Attach to empty GameObject in scene
2. Configure room settings in Inspector
3. Right-click script → "Generate Building Scene"

## Integration Notes

### Adding Real Models
1. Replace placeholder cubes with your 3D models
2. Ensure each room has:
   - Collider component
   - RoomMarker component with proper ID/Name
   - Camera anchor transform

### Customizing Events
Edit `UnityEventManager.InitializeDefaultEvents()` to add your own events, or call `AddEventToRoom()` at runtime.

### Camera Configuration
Modify `MobileInteractionController` settings in Inspector or via code to adjust camera behavior.

## Mobile Optimization

All scripts are designed with mobile performance in mind:
- Efficient touch input handling
- Smooth camera animations
- Minimal allocations
- JSON serialization optimized for mobile

