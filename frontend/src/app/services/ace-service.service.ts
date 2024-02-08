import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class AceServiceService {

  private hubConnection: signalR.HubConnection;


  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7235/chat') // Replace with your backend SignalR hub URL
      .configureLogging(signalR.LogLevel.Information)
      .build();
  }

  //start connection
  public async start(){
    try {
      await this.hubConnection.start();
      console.log("Connection is established!")
    } catch (error) {
      console.log(error);
    }
  }

  onSyncDocument(callback: (documentContent: string) => void): void {
    this.hubConnection.on('SyncDocument', (documentContent: string) => {
      callback(documentContent);
    });
  }

   // Method to send document synchronization to the server
   syncDocument(documentContent: string): void {
    this.hubConnection.invoke('SyncDocument', documentContent)
      .catch((err) => console.error('Error while syncing document:', err));
  }

  onReceiveMessage(callback: (message: string) => void): void {
    this.hubConnection.on('ReceiveMessage', (message: string) => {
      callback(message);
    });
  }
}
