# Vegviser Building Explorer - Hybrid Mobile App

A hybrid mobile application combining Unity 3D for interactive building visualization and React Native for native UI and features. Built for both Android and iOS platforms.

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Setup Instructions](#setup-instructions)
- [Unity Setup](#unity-setup)
- [React Native Setup](#react-native-setup)
- [Building for Android](#building-for-android)
- [Building for iOS](#building-for-ios)
- [Adding Real Building Models](#adding-real-building-models)
- [Mobile Optimization](#mobile-optimization)
- [Testing](#testing)
- [Troubleshooting](#troubleshooting)

## 🎯 Overview

This app provides an interactive 3D building explorer with:
- **Unity 3D Section**: Interactive 3D building scene with 3-4 placeholder rooms
- **React Native UI**: Native UI panel with event listings, navigation, analytics, and notifications
- **Bidirectional Communication**: JSON-based messaging between Unity and React Native
- **Mobile Optimizations**: URP, LODs, compressed textures, occlusion culling

## 🏗️ Architecture

### Unity 3D Components (C#)
- **RoomMarker.cs**: Stores room information (ID, name, camera anchor)
- **MobileInteractionController.cs**: Handles touch gestures (tap, pinch, orbit)
- **UnityEventManager.cs**: Manages events per room and communicates with React Native
- **UnityMessageManager.cs**: Bridge for Unity ↔ React Native communication
- **BuildingSceneSetup.cs**: Helper script to generate placeholder rooms

### React Native Components (TypeScript)
- **MainScreen**: 70% Unity view + 30% UI panel with event dropdown and details
- **BuildingInfoScreen**: Building information and features
- **AnalyticsScreen**: Usage statistics and analytics
- **NotificationsScreen**: Notification preferences and history
- **UnityBridge**: Service for Unity communication

### Communication Flow
```
Unity → React Native: JSON events per room
React Native → Unity: Navigation commands, room highlighting
```

## 📁 Project Structure

```
vegviser/
├── Unity/                          # Unity 3D project
│   ├── Assets/
│   │   └── Scripts/
│   │       ├── RoomMarker.cs
│   │       ├── MobileInteractionController.cs
│   │       ├── UnityEventManager.cs
│   │       ├── UnityMessageManager.cs
│   │       └── BuildingSceneSetup.cs
│   └── ProjectSettings/
├── src/                            # React Native source
│   ├── screens/
│   │   ├── MainScreen.tsx
│   │   ├── BuildingInfoScreen.tsx
│   │   ├── AnalyticsScreen.tsx
│   │   └── NotificationsScreen.tsx
│   ├── services/
│   │   └── UnityBridge.ts
│   └── types/
│       └── EventTypes.ts
├── android/                        # Android native code
├── ios/                            # iOS native code
├── App.tsx                         # React Native entry point
└── package.json
```

## 🔧 Prerequisites

### Required Software
- **Node.js** (v16 or higher)
- **React Native CLI** (`npm install -g react-native-cli`)
- **Unity Hub** and **Unity 2021.3 LTS** or later
- **Android Studio** (for Android development)
- **Xcode** (for iOS development, macOS only)
- **CocoaPods** (`sudo gem install cocoapods`)

### Unity Packages
- **Universal Render Pipeline (URP)**
- **Cinemachine** (for camera animations)
- **react-native-unity-view** integration

## 🚀 Setup Instructions

### 1. Clone and Install Dependencies

```bash
# Navigate to project directory
cd vegviser

# Install React Native dependencies
npm install

# Install iOS dependencies (macOS only)
cd ios && pod install && cd ..
```

### 2. Unity Setup

1. Open Unity Hub
2. Click "Add" and select the `Unity/` folder
3. Open the project in Unity 2021.3 LTS or later
4. Install required packages:
   - Window → Package Manager → Install URP
   - Window → Package Manager → Install Cinemachine
5. Set up the scene:
   - Open the main scene (or create a new one)
   - Add an empty GameObject named "SceneSetup"
   - Attach `BuildingSceneSetup.cs` script
   - Right-click the script → "Generate Building Scene"
   - Configure camera and lighting as needed

### 3. React Native Setup

```bash
# Start Metro bundler
npm start

# In a new terminal, run on Android
npm run android

# Or run on iOS (macOS only)
npm run ios
```

## 🎮 Unity Setup Details

### Creating the Building Scene

1. **Generate Placeholder Rooms**:
   - Use `BuildingSceneSetup.cs` script
   - Right-click in Inspector → "Generate Building Scene"
   - This creates 4 placeholder rooms: Lobby, Conference Room, Office Space, Cafeteria

2. **Configure Room Markers**:
   - Each room has a `RoomMarker` component
   - Set Room ID and Room Name in Inspector
   - Camera anchor is automatically created

3. **Setup Camera**:
   - Cinemachine Virtual Camera is created automatically
   - Configure orbit and zoom settings in `MobileInteractionController`

4. **Mobile Optimization**:
   - Enable URP in Project Settings → Graphics
   - Configure Quality Settings for mobile
   - Enable Occlusion Culling
   - Use compressed textures (ASTC for Android, PVRTC for iOS)

### Adding Real Building Models

#### Step 1: Import Your 3D Models
1. Create `Assets/Models/` folder
2. Import your building model (FBX, OBJ, etc.)
3. Configure import settings:
   - **Scale Factor**: Adjust to match Unity units
   - **Mesh Compression**: Medium (balance quality/performance)
   - **Generate Colliders**: Enable for room detection

#### Step 2: Replace Placeholder Rooms
1. Delete placeholder room GameObjects
2. Import your actual room models
3. For each room:
   - Add `RoomMarker` component
   - Set Room ID (e.g., "lobby", "conference")
   - Set Room Name (e.g., "Main Lobby")
   - Create/assign Camera Anchor:
     - Create empty GameObject as child
     - Position it where camera should focus
     - Assign to RoomMarker's Camera Anchor field

#### Step 3: Assign Room Anchors
```csharp
// Example: In Unity Editor or via script
RoomMarker marker = roomObject.GetComponent<RoomMarker>();
Transform anchor = roomObject.transform.Find("CameraAnchor");
marker.Initialize("lobby", "Main Lobby", anchor);
```

#### Step 4: Configure Colliders
- Ensure each room has a Collider component
- Set appropriate Layer for room detection
- Configure `MobileInteractionController.roomLayerMask` to match

#### Step 5: Optimize for Mobile
- **LOD Groups**: Add LOD levels for complex models
- **Texture Compression**: Use ASTC 4x4 (Android) or PVRTC 4-bit (iOS)
- **Occlusion Culling**: Mark static objects as Occluder Static
- **Batching**: Mark static objects as Static for batching

## 📱 Building for Android

### Prerequisites
- Android Studio installed
- Android SDK (API 21+)
- Java Development Kit (JDK 11+)

### Build Steps

1. **Configure Unity Export**:
   - File → Build Settings → Android
   - Configure Player Settings:
     - Minimum API Level: 21
     - Target API Level: 33
     - Scripting Backend: IL2CPP
     - Architecture: ARM64
   - Export as Android Library (AAR)

2. **Integrate Unity with React Native**:
   - Copy Unity library files to `android/app/libs/`
   - Update `android/app/build.gradle` to include Unity dependencies

3. **Build React Native App**:
   ```bash
   cd android
   ./gradlew assembleDebug  # Debug build
   ./gradlew assembleRelease  # Release build
   ```

4. **Run on Device**:
   ```bash
   npm run android
   ```

### Android-Specific Optimizations
- Use **ASTC** texture compression
- Enable **Multithreaded Rendering**
- Configure **Graphics API**: OpenGL ES 3.0 or Vulkan
- Set **Target Devices**: ARM64-v8a

## 🍎 Building for iOS

### Prerequisites
- macOS with Xcode installed
- CocoaPods installed
- Apple Developer account (for device testing)

### Build Steps

1. **Configure Unity Export**:
   - File → Build Settings → iOS
   - Configure Player Settings:
     - Target minimum iOS Version: 13.4
     - Architecture: ARM64
     - Scripting Backend: IL2CPP
   - Export as Xcode Project

2. **Integrate Unity with React Native**:
   - Copy Unity framework to `ios/Unity/`
   - Update `ios/Podfile` to include Unity dependencies
   - Run `pod install`

3. **Build React Native App**:
   ```bash
   cd ios
   pod install
   cd ..
   npm run ios
   ```

### iOS-Specific Optimizations
- Use **PVRTC** texture compression
- Enable **Metal** graphics API
- Configure **App Transport Security** if needed
- Set **Bitcode**: Disable (Unity doesn't support)

## 🎨 Mobile Optimization Tips

### Unity Optimizations

1. **Rendering**:
   - Use URP (Universal Render Pipeline)
   - Enable GPU Instancing
   - Use Occlusion Culling
   - Configure LOD Groups

2. **Textures**:
   - Compress textures (ASTC/PVRTC)
   - Use appropriate texture sizes (1024x1024 max for mobile)
   - Enable Mipmaps

3. **Performance**:
   - Limit draw calls (< 100 per frame)
   - Use object pooling for dynamic objects
   - Optimize shaders (mobile-friendly)
   - Profile with Unity Profiler

4. **Build Settings**:
   - Strip Engine Code: Enable
   - Managed Stripping Level: Medium
   - Script Call Optimization: Fast but no exceptions

### React Native Optimizations

1. **Performance**:
   - Use `React.memo` for expensive components
   - Implement virtual lists for long lists
   - Optimize image loading
   - Use Hermes JavaScript engine

2. **Memory**:
   - Clean up event listeners
   - Use `useCallback` and `useMemo` appropriately
   - Monitor memory usage with React DevTools

## 🧪 Testing

### Unity Testing
- Test touch interactions in Unity Editor (Mobile Input simulation)
- Verify camera animations
- Check event JSON serialization

### React Native Testing
```bash
# Run tests
npm test

# Run on Android emulator
npm run android

# Run on iOS simulator (macOS only)
npm run ios
```

### Integration Testing
1. Test Unity → React Native: Verify events appear in dropdown
2. Test React Native → Unity: Verify room navigation works
3. Test on physical devices for performance

## 🔍 Troubleshooting

### Common Issues

#### Unity Build Fails
- **Issue**: Missing dependencies
- **Solution**: Install required Unity packages (URP, Cinemachine)

#### React Native Can't Find Unity Module
- **Issue**: Native module not linked
- **Solution**: 
  - Run `pod install` (iOS)
  - Rebuild native code: `cd android && ./gradlew clean`

#### Communication Not Working
- **Issue**: Messages not passing between Unity and RN
- **Solution**: 
  - Verify `UnityMessageManager` is initialized
  - Check message format: `TYPE|JSON_DATA`
  - Enable debug logging

#### Performance Issues
- **Issue**: Low FPS, stuttering
- **Solution**:
  - Reduce texture sizes
  - Enable LODs
  - Reduce polygon count
  - Profile with Unity Profiler

#### Build Size Too Large
- **Issue**: APK/IPA file too big
- **Solution**:
  - Enable code stripping
  - Compress textures
  - Remove unused assets
  - Use ProGuard/R8 (Android)

## 📚 Additional Resources

- [Unity Mobile Optimization Guide](https://docs.unity3d.com/Manual/MobileOptimization.html)
- [React Native Documentation](https://reactnative.dev/docs/getting-started)
- [react-native-unity-view](https://github.com/react-native-unity/react-native-unity-view)
- [Cinemachine Documentation](https://docs.unity3d.com/Packages/com.unity.cinemachine@latest)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test on both Android and iOS
5. Submit a pull request

## 📄 License

This project is provided as-is for educational and development purposes.

## 🆘 Support

For issues and questions:
1. Check the Troubleshooting section
2. Review Unity and React Native logs
3. Test on physical devices
4. Verify all dependencies are installed

---

**Note**: This is a skeleton project. You'll need to:
- Integrate actual Unity build output with React Native
- Configure native modules properly
- Test on physical devices
- Add real building models and textures
- Implement additional features as needed

Happy coding! 🚀

