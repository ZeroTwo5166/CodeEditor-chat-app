import { Component} from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';


@Component({
  selector: 'app-chat-box',
  templateUrl: './chat-box.component.html',
  styleUrl: './chat-box.component.css'
})
export class ChatBoxComponent{
  private connection: HubConnection;
  public messages: string[] = [];
  public user: string = "";
  public message: string = "";
 
  constructor()
  {
    this.connection = new HubConnectionBuilder()
      .withUrl('https://localhost:7235/chat')
      .build();
  }


  async ngOnInit() {
    this.connection.on('ReceiveMessage', (user, message) => {
      this.messages.push(`${user}: ${message}`);
    });

    try {
      await this.connection.start();
      console.log('Connected to SignalR hub');
    } catch (error) {
      console.error('Failed to connect to SignalR hub', error);
    }
  }

  async sendMessage() {
    if (!this.user || !this.message) return;
    await this.connection.invoke('SendMessage', this.user, this.message);
    this.message = '';
  }

  
  

}
