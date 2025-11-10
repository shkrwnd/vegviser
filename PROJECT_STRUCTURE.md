# Project Structure Overview

## 📦 Complete Project Structure

```
vegviser/
│
├── Unity/                                    # Unity 3D Project
│   ├── Assets/
│   │   ├── Scripts/                         # C# Scripts
│   │   │   ├── RoomMarker.cs                # Room marker component
│   │   │   ├── MobileInteractionController.cs  # Touch gestures & camera
│   │   │   ├── UnityEventManager.cs         # Event management & RN bridge
│   │   │   ├── UnityMessageManager.cs       # Unity ↔ RN communication
│   │   │   ├── BuildingSceneSetup.cs        # Scene generator helper
│   │   │   └── README.md                    # Scripts documentation
│   │   └── Editor/
│   │       └── SceneSetupEditor.cs          # Custom editor for scene setup
│   └── ProjectSettings/
│       └── ProjectSettings.asset            # Unity project settings
│
├── src/                                      # React Native Source
│   ├── screens/
│   │   ├── MainScreen.tsx                   # Main screen (70% Unity + 30% UI)
│   │   ├── BuildingInfoScreen.tsx           # Building information
│   │   ├── AnalyticsScreen.tsx              # Analytics & stats
│   │   └── NotificationsScreen.tsx          # Notifications management
│   ├── services/
│   │   └── UnityBridge.ts                   # Unity communication service
│   └── types/
│       └── EventTypes.ts                    # TypeScript type definitions
│
├── android/                                  # Android Native Code
│   ├── app/
│   │   ├── src/main/
│   │   │   ├── java/com/vegviser/
│   │   │   │   ├── UnityMessageReceiver.java  # RN ↔ Unity bridge (Android)
│   │   │   │   └── UnityPackage.java          # React Native package
│   │   │   ├── AndroidManifest.xml
│   │   │   └── res/
│   │   │       └── values/
│   │   │           ├── strings.xml
│   │   │           └── styles.xml
│   │   └── build.gradle
│   ├── build.gradle
│   ├── settings.gradle
│   └── gradle.properties
│
├── ios/                                      # iOS Native Code
│   ├── Vegviser/
│   │   ├── UnityBridge.m                    # Objective-C bridge header
│   │   └── UnityBridge.swift                # Swift bridge implementation
│   └── Podfile                              # CocoaPods dependencies
│
├── App.tsx                                   # React Native entry point
├── index.js                                  # App registration
├── package.json                              # NPM dependencies
├── tsconfig.json                             # TypeScript configuration
├── babel.config.js                           # Babel configuration
├── metro.config.js                           # Metro bundler config
├── .eslintrc.js                              # ESLint configuration
├── .prettierrc.js                            # Prettier configuration
├── jest.config.js                            # Jest test configuration
├── .gitignore                                # Git ignore rules
│
├── README.md                                 # Main documentation
├── QUICKSTART.md                             # Quick start guide
└── PROJECT_STRUCTURE.md                      # This file
```

## 🎯 Key Components

### Unity 3D (C#)
1. **RoomMarker.cs**: Room identification and camera anchors
2. **MobileInteractionController.cs**: Touch gestures (tap, pinch, orbit)
3. **UnityEventManager.cs**: Event storage and React Native communication
4. **UnityMessageManager.cs**: Message bridge between Unity and RN
5. **BuildingSceneSetup.cs**: Helper to generate placeholder scene

### React Native (TypeScript)
1. **MainScreen.tsx**: Primary screen with Unity view + UI panel
2. **BuildingInfoScreen.tsx**: Building information display
3. **AnalyticsScreen.tsx**: Usage analytics and statistics
4. **NotificationsScreen.tsx**: Notification preferences
5. **UnityBridge.ts**: Service for Unity communication

### Native Integration
- **Android**: Java modules for Unity ↔ RN bridge
- **iOS**: Swift/Objective-C modules for Unity ↔ RN bridge

## 🔄 Communication Flow

```
┌─────────────┐                    ┌──────────────┐
│   Unity 3D  │                    │ React Native │
│    (C#)     │                    │  (TypeScript)│
└──────┬──────┘                    └──────┬───────┘
       │                                    │
       │  JSON Events (EVENTS_UPDATE)      │
       ├──────────────────────────────────>│
       │                                    │
       │  Navigation (NAVIGATE_TO_ROOM)    │
       │<──────────────────────────────────┤
       │                                    │
       └────────────────────────────────────┘
              UnityMessageManager
```

## 📱 Features Implemented

### Unity Side
- ✅ 3D building scene with placeholder rooms
- ✅ Room markers with IDs and camera anchors
- ✅ Touch interactions (tap, pinch, orbit)
- ✅ Cinemachine camera animations
- ✅ Event management system
- ✅ JSON communication with React Native

### React Native Side
- ✅ 70/30 split layout (Unity view / UI panel)
- ✅ Event dropdown/picker
- ✅ Event details display
- ✅ Navigation to other screens
- ✅ Analytics screen
- ✅ Notifications screen
- ✅ Unity bridge integration

### Native Integration
- ✅ Android bridge setup
- ✅ iOS bridge setup
- ✅ Bidirectional messaging

## 🚀 Next Steps

1. **Build Unity Project**:
   - Export Unity build for Android/iOS
   - Integrate Unity library with React Native

2. **Test Integration**:
   - Verify Unity view renders in React Native
   - Test message passing both ways
   - Test on physical devices

3. **Add Real Content**:
   - Replace placeholder rooms with real 3D models
   - Add real events data
   - Customize UI styling

4. **Optimize**:
   - Profile performance
   - Optimize textures and models
   - Test on various devices

## 📚 Documentation Files

- **README.md**: Complete setup and build instructions
- **QUICKSTART.md**: 5-minute quick start guide
- **Unity/Assets/Scripts/README.md**: Unity scripts documentation
- **PROJECT_STRUCTURE.md**: This file

## 🔧 Configuration Files

- **package.json**: NPM dependencies and scripts
- **tsconfig.json**: TypeScript compiler options
- **babel.config.js**: Babel transpilation config
- **metro.config.js**: Metro bundler config
- **android/build.gradle**: Android build configuration
- **ios/Podfile**: iOS CocoaPods dependencies

---

**Status**: ✅ Complete skeleton project ready for development

