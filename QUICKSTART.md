# Quick Start Guide

## 🚀 Get Started in 5 Minutes

### Step 1: Install Dependencies
```bash
npm install
cd ios && pod install && cd ..  # macOS only
```

### Step 2: Setup Unity Scene
1. Open Unity Hub
2. Add project: Select `Unity/` folder
3. Open project in Unity 2021.3 LTS+
4. In Unity:
   - Create empty GameObject
   - Add `BuildingSceneSetup` component
   - Click "Generate Building Scene" button
5. Build Unity for your platform (see main README)

### Step 3: Run React Native
```bash
# Start Metro
npm start

# Run on Android (new terminal)
npm run android

# Or iOS (macOS only)
npm run ios
```

## ⚡ Key Features to Test

1. **Unity 3D View**: Tap rooms to navigate
2. **Pinch to Zoom**: Two-finger pinch gesture
3. **Orbit Camera**: One-finger drag
4. **Event Dropdown**: Select events from dropdown
5. **Event Details**: View event information
6. **Navigate to Room**: Button to jump to room in 3D view

## 📝 Next Steps

1. Replace placeholder rooms with real 3D models
2. Add more events via `UnityEventManager`
3. Customize UI in React Native screens
4. Add analytics tracking
5. Implement push notifications

## 🐛 Common Issues

- **Unity not showing**: Ensure Unity build is integrated (see main README)
- **Events not appearing**: Check Unity → React Native bridge connection
- **Touch not working**: Verify Unity view is receiving touch events

For detailed setup, see [README.md](./README.md)

