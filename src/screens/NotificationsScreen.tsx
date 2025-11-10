import React, {useState} from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  SafeAreaView,
  Switch,
  TouchableOpacity,
} from 'react-native';

const NotificationsScreen = () => {
  const [notifications, setNotifications] = useState({
    eventReminders: true,
    roomUpdates: true,
    systemAlerts: false,
  });

  const toggleNotification = (key: keyof typeof notifications) => {
    setNotifications(prev => ({
      ...prev,
      [key]: !prev[key],
    }));
  };

  return (
    <SafeAreaView style={styles.container}>
      <ScrollView style={styles.scrollView}>
        <View style={styles.section}>
          <Text style={styles.title}>Notifications</Text>
          <Text style={styles.description}>
            Manage your notification preferences for the building explorer.
          </Text>
        </View>

        <View style={styles.section}>
          <View style={styles.settingRow}>
            <View style={styles.settingInfo}>
              <Text style={styles.settingTitle}>Event Reminders</Text>
              <Text style={styles.settingDescription}>
                Get notified before events start
              </Text>
            </View>
            <Switch
              value={notifications.eventReminders}
              onValueChange={() => toggleNotification('eventReminders')}
              trackColor={{false: '#ccc', true: '#6200ee'}}
              thumbColor="#fff"
            />
          </View>

          <View style={styles.divider} />

          <View style={styles.settingRow}>
            <View style={styles.settingInfo}>
              <Text style={styles.settingTitle}>Room Updates</Text>
              <Text style={styles.settingDescription}>
                Notifications when rooms are updated
              </Text>
            </View>
            <Switch
              value={notifications.roomUpdates}
              onValueChange={() => toggleNotification('roomUpdates')}
              trackColor={{false: '#ccc', true: '#6200ee'}}
              thumbColor="#fff"
            />
          </View>

          <View style={styles.divider} />

          <View style={styles.settingRow}>
            <View style={styles.settingInfo}>
              <Text style={styles.settingTitle}>System Alerts</Text>
              <Text style={styles.settingDescription}>
                Important system and maintenance alerts
              </Text>
            </View>
            <Switch
              value={notifications.systemAlerts}
              onValueChange={() => toggleNotification('systemAlerts')}
              trackColor={{false: '#ccc', true: '#6200ee'}}
              thumbColor="#fff"
            />
          </View>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Recent Notifications</Text>
          <View style={styles.notificationItem}>
            <Text style={styles.notificationTitle}>
              Conference Room Event Starting Soon
            </Text>
            <Text style={styles.notificationTime}>2 hours ago</Text>
            <Text style={styles.notificationText}>
              Quarterly Review meeting starts in 15 minutes
            </Text>
          </View>

          <View style={styles.divider} />

          <View style={styles.notificationItem}>
            <Text style={styles.notificationTitle}>New Event Added</Text>
            <Text style={styles.notificationTime}>1 day ago</Text>
            <Text style={styles.notificationText}>
              Product Launch presentation added to Conference Room
            </Text>
          </View>
        </View>

        <View style={styles.section}>
          <TouchableOpacity style={styles.clearButton}>
            <Text style={styles.clearButtonText}>Clear All Notifications</Text>
          </TouchableOpacity>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  scrollView: {
    flex: 1,
  },
  section: {
    padding: 20,
    backgroundColor: '#fff',
    marginBottom: 12,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 12,
    color: '#333',
  },
  description: {
    fontSize: 16,
    lineHeight: 24,
    color: '#666',
  },
  settingRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 12,
  },
  settingInfo: {
    flex: 1,
    marginRight: 16,
  },
  settingTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#333',
    marginBottom: 4,
  },
  settingDescription: {
    fontSize: 14,
    color: '#666',
  },
  divider: {
    height: 1,
    backgroundColor: '#e0e0e0',
    marginVertical: 8,
  },
  sectionTitle: {
    fontSize: 20,
    fontWeight: '600',
    marginBottom: 16,
    color: '#333',
  },
  notificationItem: {
    paddingVertical: 12,
  },
  notificationTitle: {
    fontSize: 16,
    fontWeight: '600',
    color: '#333',
    marginBottom: 4,
  },
  notificationTime: {
    fontSize: 12,
    color: '#999',
    marginBottom: 8,
  },
  notificationText: {
    fontSize: 14,
    color: '#666',
    lineHeight: 20,
  },
  clearButton: {
    backgroundColor: '#f44336',
    paddingVertical: 14,
    borderRadius: 8,
    alignItems: 'center',
  },
  clearButtonText: {
    color: '#fff',
    fontSize: 16,
    fontWeight: '600',
  },
});

export default NotificationsScreen;

