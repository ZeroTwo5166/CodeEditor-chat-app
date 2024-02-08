import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr";

@Injectable({
  providedIn: 'root'
})

export class SignalRService {

  private hubConnection!: signalR.HubConnection;

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7085/chat')
      .build();
  }

  startConnection() {
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  }

  addMessageListener(callback: (user: string, message: string) => void) {
    this.hubConnection.on('ReceiveMessage', callback);
  }

  addRemoveMessageListener(callback: (index: number) => void) {
    this.hubConnection.on('RemoveMessage', callback);
  }

  sendMessage(user: string, message: string) {
    this.hubConnection.invoke('SendMessage', user, message);
  }

  removeMessage(index: number) {
    this.hubConnection.invoke('RemoveMessage', index);
  }

  stopConnection() {
    this.hubConnection.stop();
    console.log('Connection stopped');
  }

}
