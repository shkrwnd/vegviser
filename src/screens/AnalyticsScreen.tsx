import React, {useState, useEffect} from 'react';
import {
  View,
  Text,
  StyleSheet,
  ScrollView,
  SafeAreaView,
  ActivityIndicator,
} from 'react-native';
import AsyncStorage from '@react-native-async-storage/async-storage';

const AnalyticsScreen = () => {
  const [stats, setStats] = useState({
    totalEvents: 0,
    roomsVisited: 0,
    appUsageTime: 0,
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadAnalytics();
  }, []);

  const loadAnalytics = async () => {
    try {
      // Load analytics data from storage
      const totalEvents = await AsyncStorage.getItem('analytics_totalEvents');
      const roomsVisited = await AsyncStorage.getItem('analytics_roomsVisited');
      const usageTime = await AsyncStorage.getItem('analytics_usageTime');

      setStats({
        totalEvents: totalEvents ? parseInt(totalEvents, 10) : 6,
        roomsVisited: roomsVisited ? parseInt(roomsVisited, 10) : 0,
        appUsageTime: usageTime ? parseInt(usageTime, 10) : 0,
      });
    } catch (error) {
      console.error('Error loading analytics:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <SafeAreaView style={styles.container}>
        <View style={styles.loadingContainer}>
          <ActivityIndicator size="large" color="#6200ee" />
        </View>
      </SafeAreaView>
    );
  }

  return (
    <SafeAreaView style={styles.container}>
      <ScrollView style={styles.scrollView}>
        <View style={styles.section}>
          <Text style={styles.title}>Analytics</Text>
          <Text style={styles.description}>
            Track your usage and engagement with the building explorer.
          </Text>
        </View>

        <View style={styles.statCard}>
          <Text style={styles.statValue}>{stats.totalEvents}</Text>
          <Text style={styles.statLabel}>Total Events Viewed</Text>
        </View>

        <View style={styles.statCard}>
          <Text style={styles.statValue}>{stats.roomsVisited}</Text>
          <Text style={styles.statLabel}>Rooms Visited</Text>
        </View>

        <View style={styles.statCard}>
          <Text style={styles.statValue}>
            {Math.floor(stats.appUsageTime / 60)}m
          </Text>
          <Text style={styles.statLabel}>App Usage Time</Text>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Event Distribution</Text>
          <View style={styles.chartPlaceholder}>
            <Text style={styles.chartText}>
              Chart visualization would go here
            </Text>
            <Text style={styles.chartSubtext}>
              Integrate with your preferred charting library
            </Text>
          </View>
        </View>

        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Popular Rooms</Text>
          <Text style={styles.bulletPoint}>1. Conference Room</Text>
          <Text style={styles.bulletPoint}>2. Lobby</Text>
          <Text style={styles.bulletPoint}>3. Office Space</Text>
          <Text style={styles.bulletPoint}>4. Cafeteria</Text>
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
  loadingContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
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
  statCard: {
    backgroundColor: '#fff',
    padding: 24,
    marginHorizontal: 20,
    marginBottom: 12,
    borderRadius: 12,
    alignItems: 'center',
    shadowColor: '#000',
    shadowOffset: {width: 0, height: 2},
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 3,
  },
  statValue: {
    fontSize: 48,
    fontWeight: 'bold',
    color: '#6200ee',
    marginBottom: 8,
  },
  statLabel: {
    fontSize: 16,
    color: '#666',
  },
  sectionTitle: {
    fontSize: 20,
    fontWeight: '600',
    marginBottom: 12,
    color: '#333',
  },
  chartPlaceholder: {
    height: 200,
    backgroundColor: '#f8f8f8',
    borderRadius: 8,
    justifyContent: 'center',
    alignItems: 'center',
    marginTop: 12,
  },
  chartText: {
    fontSize: 16,
    color: '#999',
    marginBottom: 4,
  },
  chartSubtext: {
    fontSize: 12,
    color: '#bbb',
  },
  bulletPoint: {
    fontSize: 16,
    lineHeight: 28,
    color: '#666',
    marginLeft: 8,
  },
});

export default AnalyticsScreen;

