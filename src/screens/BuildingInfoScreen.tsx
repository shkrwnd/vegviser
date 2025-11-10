import React from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  SafeAreaView,
} from 'react-native';

const BuildingInfoScreen = () => {
  return (
    <SafeAreaView style={styles.container}>
      <ScrollView style={styles.scrollView}>
        <View style={styles.section}>
          <Text style={styles.title}>Building Information</Text>
          <Text style={styles.description}>
            Welcome to the Vegviser Building Explorer. This app provides an
            interactive 3D view of the building with real-time event information.
          </Text>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Features</Text>
          <Text style={styles.bulletPoint}>• Interactive 3D building exploration</Text>
          <Text style={styles.bulletPoint}>• Real-time event listings</Text>
          <Text style={styles.bulletPoint}>• Room navigation and details</Text>
          <Text style={styles.bulletPoint}>• Analytics and insights</Text>
          <Text style={styles.bulletPoint}>• Push notifications</Text>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Building Details</Text>
          <Text style={styles.infoText}>
            <Text style={styles.label}>Total Rooms: </Text>4
          </Text>
          <Text style={styles.infoText}>
            <Text style={styles.label}>Floors: </Text>1
          </Text>
          <Text style={styles.infoText}>
            <Text style={styles.label}>Total Area: </Text>~500 sqm
          </Text>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Rooms</Text>
          <Text style={styles.bulletPoint}>• Lobby - Main entrance and reception</Text>
          <Text style={styles.bulletPoint}>• Conference Room - Meetings and presentations</Text>
          <Text style={styles.bulletPoint}>• Office Space - Workspace area</Text>
          <Text style={styles.bulletPoint}>• Cafeteria - Dining and refreshments</Text>
        </View>
      </ScrollView>
    </SafeAreaView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
  },
  scrollView: {
    flex: 1,
  },
  section: {
    padding: 20,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
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
  sectionTitle: {
    fontSize: 20,
    fontWeight: '600',
    marginBottom: 12,
    color: '#333',
  },
  bulletPoint: {
    fontSize: 16,
    lineHeight: 28,
    color: '#666',
    marginLeft: 8,
  },
  infoText: {
    fontSize: 16,
    lineHeight: 28,
    color: '#666',
  },
  label: {
    fontWeight: '600',
    color: '#333',
  },
});

export default BuildingInfoScreen;

