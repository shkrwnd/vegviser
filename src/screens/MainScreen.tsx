import React, {useState, useEffect, useRef} from 'react';
import {
  View,
  StyleSheet,
  Text,
  ScrollView,
  TouchableOpacity,
  SafeAreaView,
} from 'react-native';
import UnityView from 'react-native-unity-view';
import RNPickerSelect from 'react-native-picker-select';
import {useNavigation} from '@react-navigation/native';
import {UnityBridge} from '../services/UnityBridge';
import {RoomEvent} from '../types/EventTypes';

const MainScreen = () => {
  const navigation = useNavigation();
  const unityRef = useRef<any>(null);
  const [events, setEvents] = useState<RoomEvent[]>([]);
  const [selectedEvent, setSelectedEvent] = useState<RoomEvent | null>(null);
  const [allRoomsData, setAllRoomsData] = useState<any[]>([]);

  useEffect(() => {
    // Initialize Unity bridge
    UnityBridge.initialize();

    // Listen for messages from Unity
    UnityBridge.onMessage((message: string) => {
      handleUnityMessage(message);
    });

    // Request initial events
    UnityBridge.sendMessageToUnity('REQUEST_EVENTS|{}');

    return () => {
      UnityBridge.removeListeners();
    };
  }, []);

  const handleUnityMessage = (message: string) => {
    console.log('Received from Unity:', message);

    try {
      const [type, data] = message.split('|');
      const jsonData = JSON.parse(data);

      if (type === 'EVENTS_UPDATE' || type === 'ROOM_EVENTS_UPDATE') {
        if (type === 'EVENTS_UPDATE') {
          // All events update
          const allEvents: RoomEvent[] = [];
          jsonData.rooms?.forEach((room: any) => {
            room.events?.forEach((event: any) => {
              allEvents.push({
                ...event,
                roomID: room.roomID,
                roomName: room.roomName,
              });
            });
          });
          setEvents(allEvents);
          setAllRoomsData(jsonData.rooms || []);
        } else {
          // Single room events update
          const roomEvents: RoomEvent[] = jsonData.events?.map((event: any) => ({
            ...event,
            roomID: jsonData.roomID,
            roomName: jsonData.roomName,
          })) || [];
          setEvents(prev => {
            const filtered = prev.filter(e => e.roomID !== jsonData.roomID);
            return [...filtered, ...roomEvents];
          });
        }
      }
    } catch (error) {
      console.error('Error parsing Unity message:', error);
    }
  };

  const handleEventSelect = (value: string) => {
    const event = events.find(e => `${e.roomName} - ${e.eventName}` === value);
    setSelectedEvent(event || null);
  };

  const handleNavigateToRoom = (roomID: string) => {
    UnityBridge.sendMessageToUnity(
      `NAVIGATE_TO_ROOM|${JSON.stringify({roomID})}`,
    );
  };

  const pickerItems = events.map(event => ({
    label: `${event.roomName} - ${event.eventName}`,
    value: `${event.roomName} - ${event.eventName}`,
  }));

  return (
    <SafeAreaView style={styles.container}>
      {/* Unity 3D View - Top 70% */}
      <View style={styles.unityContainer}>
        <UnityView
          ref={unityRef}
          style={styles.unityView}
          onUnityMessage={handleUnityMessage}
        />
      </View>

      {/* React Native UI Panel - Bottom 30% */}
      <View style={styles.uiPanel}>
        <View style={styles.header}>
          <Text style={styles.headerTitle}>Events</Text>
          <TouchableOpacity
            style={styles.navButton}
            onPress={() => navigation.navigate('BuildingInfo' as never)}>
            <Text style={styles.navButtonText}>Info</Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={styles.navButton}
            onPress={() => navigation.navigate('Analytics' as never)}>
            <Text style={styles.navButtonText}>Analytics</Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={styles.navButton}
            onPress={() => navigation.navigate('Notifications' as never)}>
            <Text style={styles.navButtonText}>🔔</Text>
          </TouchableOpacity>
        </View>

        {/* Dropdown/Picker */}
        <View style={styles.pickerContainer}>
          <RNPickerSelect
            onValueChange={handleEventSelect}
            items={pickerItems}
            placeholder={{
              label: 'Select an event...',
              value: null,
            }}
            style={pickerSelectStyles}
          />
        </View>

        {/* Event Details Text Area */}
        <ScrollView style={styles.detailsContainer}>
          {selectedEvent ? (
            <View>
              <Text style={styles.eventTitle}>{selectedEvent.eventName}</Text>
              <Text style={styles.eventRoom}>
                Room: {selectedEvent.roomName}
              </Text>
              <Text style={styles.eventTime}>Time: {selectedEvent.eventTime}</Text>
              <Text style={styles.eventType}>Type: {selectedEvent.eventType}</Text>
              <Text style={styles.eventDescription}>
                {selectedEvent.eventDescription}
              </Text>
              <TouchableOpacity
                style={styles.navigateButton}
                onPress={() => handleNavigateToRoom(selectedEvent.roomID)}>
                <Text style={styles.navigateButtonText}>
                  Navigate to Room
                </Text>
              </TouchableOpacity>
            </View>
          ) : (
            <Text style={styles.placeholderText}>
              Select an event to view details
            </Text>
          )}
        </ScrollView>
      </View>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  unityContainer: {
    flex: 0.7, // 70% of screen
    backgroundColor: '#000',
  },
  unityView: {
    flex: 1,
  },
  uiPanel: {
    flex: 0.3, // 30% of screen
    backgroundColor: '#fff',
    borderTopWidth: 1,
    borderTopColor: '#e0e0e0',
  },
  header: {
    flexDirection: 'row',
    alignItems: 'center',
    paddingHorizontal: 16,
    paddingVertical: 12,
    backgroundColor: '#f8f8f8',
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  headerTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    flex: 1,
  },
  navButton: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    marginLeft: 8,
    backgroundColor: '#6200ee',
    borderRadius: 4,
  },
  navButtonText: {
    color: '#fff',
    fontSize: 12,
    fontWeight: '600',
  },
  pickerContainer: {
    paddingHorizontal: 16,
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  detailsContainer: {
    flex: 1,
    padding: 16,
  },
  eventTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 8,
    color: '#333',
  },
  eventRoom: {
    fontSize: 16,
    color: '#6200ee',
    marginBottom: 4,
  },
  eventTime: {
    fontSize: 14,
    color: '#666',
    marginBottom: 4,
  },
  eventType: {
    fontSize: 14,
    color: '#666',
    marginBottom: 8,
  },
  eventDescription: {
    fontSize: 14,
    color: '#333',
    lineHeight: 20,
    marginBottom: 12,
  },
  navigateButton: {
    backgroundColor: '#6200ee',
    paddingVertical: 12,
    paddingHorizontal: 24,
    borderRadius: 8,
    alignSelf: 'flex-start',
  },
  navigateButtonText: {
    color: '#fff',
    fontSize: 14,
    fontWeight: '600',
  },
  placeholderText: {
    fontSize: 14,
    color: '#999',
    textAlign: 'center',
    marginTop: 20,
  },
});

const pickerSelectStyles = {
  inputIOS: {
    fontSize: 16,
    paddingVertical: 12,
    paddingHorizontal: 10,
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 8,
    color: '#333',
    paddingRight: 30,
  },
  inputAndroid: {
    fontSize: 16,
    paddingHorizontal: 10,
    paddingVertical: 8,
    borderWidth: 1,
    borderColor: '#ccc',
    borderRadius: 8,
    color: '#333',
    paddingRight: 30,
  },
  iconContainer: {
    top: 10,
    right: 12,
  },
};

export default MainScreen;

