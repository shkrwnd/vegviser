# How to Get the Unity Framework for iOS

This guide will walk you through building and exporting the Unity framework so it can be integrated with your React Native app.

## Prerequisites

1. **Unity Hub** installed
   - Download from: https://unity.com/download
   - Install Unity Hub first

2. **Unity 2021.3 LTS or later**
   - Open Unity Hub
   - Go to "Installs" tab
   - Click "Install Editor"
   - Select **Unity 2021.3 LTS** (recommended) or later
   - Make sure to include **iOS Build Support** module

3. **Xcode** (already installed ✅)

## Step-by-Step Instructions

### Step 1: Open Unity Project

1. **Launch Unity Hub**
2. **Click "Add"** button
3. **Navigate to** `/Users/raj/Documents/vegviser/Unity/`
4. **Select the Unity folder** and click "Add"
5. **Click "Open"** to open the project in Unity

### Step 2: Configure Unity Project Settings

1. **Open Build Settings**:
   - Go to `File → Build Settings` (or press `⌘⇧B`)

2. **Select iOS Platform**:
   - In the Platform list, select **iOS**
   - Click **"Switch Platform"** (if not already selected)
   - Wait for Unity to reimport assets

3. **Configure Player Settings**:
   - Click **"Player Settings"** button
   - In the Inspector panel, configure:
     - **Company Name**: Your company name
     - **Product Name**: VegviserBuildingApp
     - **Bundle Identifier**: `com.vegviser.buildingapp` (or your own)
     - **Target minimum iOS Version**: `13.4` (or higher)
     - **Architecture**: `ARM64` (required)
     - **Scripting Backend**: `IL2CPP` (required)
     - **API Compatibility Level**: `.NET Standard 2.1` or `.NET Framework`

4. **Graphics Settings** (Optional but recommended):
   - **Graphics API**: `Metal` (for iOS)
   - **Color Space**: `Linear` or `Gamma` (your choice)
   - **Auto Graphics API**: Uncheck (use Metal only)

5. **Other Settings**:
   - **Bitcode**: **Disable** (Unity doesn't support Bitcode)
   - **Requires ARKit support**: Uncheck (unless you need AR)
   - **Camera Usage Description**: Add if using camera

### Step 3: Set Up the Scene (If Not Done)

1. **Open or Create Main Scene**:
   - If you have a scene, open it
   - If not, create a new scene: `File → New Scene`

2. **Add Building Scene Setup** (if using the provided script):
   - Create empty GameObject: `GameObject → Create Empty`
   - Name it "SceneSetup"
   - Add component: `BuildingSceneSetup` script
   - In Inspector, right-click the script → "Generate Building Scene"

3. **Save the Scene**:
   - `File → Save As...`
   - Save as `MainScene.unity` in `Assets/Scenes/`

### Step 4: Build Unity for iOS

1. **Open Build Settings** again: `File → Build Settings` (`⌘⇧B`)

2. **Add Scene to Build**:
   - Click **"Add Open Scenes"** to add your main scene
   - Make sure it's at index 0 (drag to reorder if needed)

3. **Configure Build Settings**:
   - **Development Build**: Check this for debugging
   - **Script Debugging**: Check if you want to debug C# scripts
   - **Autoconnect Profiler**: Optional

4. **Build and Export**:
   - Click **"Build"** button
   - **IMPORTANT**: When prompted, choose **"Export Project"** (NOT "Build and Run")
   - Select export location: `/Users/raj/Documents/vegviser/unity/builds/ios/`
   - Click "Choose"
   - Unity will export an Xcode project

### Step 5: Build UnityFramework from Xcode

After Unity exports, you need to build the UnityFramework:

1. **Open the Exported Xcode Project**:
   ```bash
   open /Users/raj/Documents/vegviser/unity/builds/ios/Unity-iPhone.xcodeproj
   ```

2. **Select UnityFramework Target**:
   - In Xcode, look at the top toolbar
   - Click the scheme dropdown (next to Play/Stop buttons)
   - Select **"UnityFramework"** (NOT Unity-iPhone)

3. **Select iOS Device or Simulator**:
   - Choose **"Any iOS Device"** or a specific simulator
   - For simulator: Choose "iPhone 15" or similar

4. **Build UnityFramework**:
   - Press `⌘B` (Command + B) to build
   - Wait for build to complete (may take 2-5 minutes)

5. **Locate the Built Framework**:
   - After build, the framework will be in:
   - `~/Library/Developer/Xcode/DerivedData/Unity-iPhone-*/Build/Products/Debug-iphonesimulator/UnityFramework.framework`
   - Or check: `Product → Show Build Folder in Finder`

### Step 6: Copy Framework to React Native Project

1. **Find the Built Framework**:
   - In Xcode, right-click on `UnityFramework.framework` in Products folder
   - Select **"Show in Finder"**

2. **Copy Framework**:
   ```bash
   # Copy the entire UnityFramework.framework folder
   cp -R ~/Library/Developer/Xcode/DerivedData/Unity-iPhone-*/Build/Products/Debug-iphonesimulator/UnityFramework.framework \
        /Users/raj/Documents/vegviser/unity/builds/ios/
   ```

   Or manually:
   - Navigate to the framework in Finder
   - Copy the entire `UnityFramework.framework` folder
   - Paste it into `/Users/raj/Documents/vegviser/unity/builds/ios/`

3. **Verify Framework Structure**:
   The framework should contain:
   ```
   UnityFramework.framework/
   ├── Headers/
   │   ├── UnityFramework.h
   │   └── NativeCallProxy.h
   ├── Info.plist
   ├── Modules/
   └── UnityFramework (binary)
   ```

### Step 7: Rebuild React Native App

Now that the Unity framework is in place:

1. **Reinstall Pods** (if needed):
   ```bash
   cd /Users/raj/Documents/vegviser/ios
   pod install
   cd ..
   ```

2. **Build React Native App**:
   ```bash
   npm run ios
   ```

   Or in Xcode:
   - Open `Vegviser.xcworkspace`
   - Select "VegviserTemp" scheme
   - Press `⌘R` to build and run

## Troubleshooting

### Issue: "UnityFramework/UnityFramework.h not found"

**Solution**: Make sure the framework is in the correct location:
- Path should be: `/Users/raj/Documents/vegviser/unity/builds/ios/UnityFramework.framework`
- The framework must contain the `Headers/` folder with the header files

### Issue: "No such module 'UnityFramework'"

**Solution**: 
1. Clean build folder in Xcode: `Product → Clean Build Folder` (`⇧⌘K`)
2. Rebuild: `Product → Build` (`⌘B`)

### Issue: Build fails with linking errors

**Solution**:
1. Make sure you built UnityFramework for the correct architecture (ARM64 for device, x86_64/arm64 for simulator)
2. Check that the framework binary exists: `ls -la UnityFramework.framework/UnityFramework`

### Issue: Unity exports but no framework

**Solution**: 
- Make sure you selected "Export Project" not "Build and Run"
- You need to build UnityFramework separately in Xcode (Step 5)

## Quick Reference

**Unity Project Path**: `/Users/raj/Documents/vegviser/Unity/`

**Framework Destination**: `/Users/raj/Documents/vegviser/unity/builds/ios/UnityFramework.framework`

**Unity Version**: 2021.3 LTS or later

**iOS Minimum Version**: 13.4

**Architecture**: ARM64

## Next Steps After Framework is Ready

1. ✅ Framework copied to correct location
2. ✅ Rebuild React Native app
3. ✅ Test Unity integration
4. ✅ Uncomment Unity code in `MainScreen.tsx`
5. ✅ Test 3D building visualization

---

**Note**: The first Unity build can take 10-20 minutes. Subsequent builds are faster (2-5 minutes).

**Tip**: For development, you can build for iOS Simulator. For production/testing on device, build for "Any iOS Device" or a specific device.

