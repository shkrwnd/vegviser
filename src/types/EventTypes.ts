export interface RoomEvent {
  eventID: string;
  eventName: string;
  eventDescription: string;
  eventTime: string;
  eventType: string;
  roomID?: string;
  roomName?: string;
}

export interface RoomEventData {
  roomID: string;
  roomName: string;
  events: RoomEvent[];
}

export interface AllEventsData {
  rooms: RoomEventData[];
}

