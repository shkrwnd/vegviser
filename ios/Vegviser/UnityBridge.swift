import Foundation
import React

@objc(UnityMessageReceiver)
class UnityMessageReceiver: RCTEventEmitter {
    
    override static func requiresMainQueueSetup() -> Bool {
        return true
    }
    
    override func supportedEvents() -> [String]! {
        return ["UnityMessage"]
    }
    
    @objc
    func onUnityMessage(_ message: String) {
        sendEvent(withName: "UnityMessage", body: ["message": message])
    }
}

