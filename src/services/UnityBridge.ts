import {NativeModules, NativeEventEmitter} from 'react-native';

/**
 * Bridge service for communication between React Native and Unity
 * Handles bidirectional message passing
 */
class UnityBridgeService {
  private eventEmitter: NativeEventEmitter | null = null;
  private listeners: Array<() => void> = [];

  initialize() {
    try {
      // Initialize Unity bridge
      // react-native-unity-view provides UnityModule
      const {UnityModule} = NativeModules;
      if (UnityModule) {
        this.eventEmitter = new NativeEventEmitter(UnityModule);
      }
    } catch (error) {
      console.warn('Unity bridge initialization warning:', error);
    }
  }

  /**
   * Send message to Unity
   */
  sendMessageToUnity(message: string) {
    try {
      const {UnityModule} = NativeModules;
      if (UnityModule && UnityModule.postMessage) {
        UnityModule.postMessage('UnityMessageManager', 'OnMessageFromRN', message);
      } else {
        console.log('[UnityBridge] Would send to Unity:', message);
      }
    } catch (error) {
      console.error('[UnityBridge] Error sending message to Unity:', error);
    }
  }

  /**
   * Listen for messages from Unity
   */
  onMessage(callback: (message: string) => void) {
    if (!this.eventEmitter) {
      console.warn('[UnityBridge] Event emitter not initialized');
      return;
    }

    const subscription = this.eventEmitter.addListener(
      'UnityMessage',
      (data: {message: string}) => {
        callback(data.message);
      },
    );

    this.listeners.push(() => {
      subscription.remove();
    });
  }

  /**
   * Remove all listeners
   */
  removeListeners() {
    this.listeners.forEach(remove => remove());
    this.listeners = [];
  }
}

export const UnityBridge = new UnityBridgeService();

