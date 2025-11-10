import React from 'react';
import {NavigationContainer} from '@react-navigation/native';
import {createStackNavigator} from '@react-navigation/stack';
import {StatusBar, StyleSheet} from 'react-native';
import MainScreen from './src/screens/MainScreen';
import BuildingInfoScreen from './src/screens/BuildingInfoScreen';
import AnalyticsScreen from './src/screens/AnalyticsScreen';
import NotificationsScreen from './src/screens/NotificationsScreen';

const Stack = createStackNavigator();

const App = () => {
  return (
    <NavigationContainer>
      <StatusBar barStyle="dark-content" />
      <Stack.Navigator
        initialRouteName="Main"
        screenOptions={{
          headerStyle: {
            backgroundColor: '#6200ee',
          },
          headerTintColor: '#fff',
          headerTitleStyle: {
            fontWeight: 'bold',
          },
        }}>
        <Stack.Screen
          name="Main"
          component={MainScreen}
          options={{title: 'Building Explorer'}}
        />
        <Stack.Screen
          name="BuildingInfo"
          component={BuildingInfoScreen}
          options={{title: 'Building Information'}}
        />
        <Stack.Screen
          name="Analytics"
          component={AnalyticsScreen}
          options={{title: 'Analytics'}}
        />
        <Stack.Screen
          name="Notifications"
          component={NotificationsScreen}
          options={{title: 'Notifications'}}
        />
      </Stack.Navigator>
    </NavigationContainer>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
  },
});

export default App;

