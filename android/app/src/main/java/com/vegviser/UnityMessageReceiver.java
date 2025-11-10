package com.vegviser;

import android.app.Activity;
import com.facebook.react.bridge.ReactApplicationContext;
import com.facebook.react.bridge.ReactContextBaseJavaModule;
import com.facebook.react.bridge.ReactMethod;
import com.facebook.react.bridge.WritableMap;
import com.facebook.react.bridge.Arguments;
import com.facebook.react.modules.core.DeviceEventManagerModule;

/**
 * Native module for receiving messages from Unity and forwarding to React Native
 */
public class UnityMessageReceiver extends ReactContextBaseJavaModule {
    private ReactApplicationContext reactContext;

    public UnityMessageReceiver(ReactApplicationContext reactContext) {
        super(reactContext);
        this.reactContext = reactContext;
    }

    @Override
    public String getName() {
        return "UnityMessageReceiver";
    }

    /**
     * Called by Unity to send messages to React Native
     */
    @ReactMethod
    public void onUnityMessage(String message) {
        WritableMap params = Arguments.createMap();
        params.putString("message", message);

        reactContext
            .getJSModule(DeviceEventManagerModule.RCTDeviceEventEmitter.class)
            .emit("UnityMessage", params);
    }

    /**
     * Get the Unity message receiver instance
     * Called by Unity C# code
     */
    public static UnityMessageReceiver getInstance(Activity activity) {
        // This will be set by React Native when the module is initialized
        // In a real implementation, you'd maintain a reference
        return null; // Placeholder - actual implementation depends on your setup
    }
}

