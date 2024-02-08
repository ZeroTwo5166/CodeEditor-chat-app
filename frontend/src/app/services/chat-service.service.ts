import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatServiceService {
  private messages : string[] = [];
  private messagesSubject: Subject<string[]> = new Subject<string[]>();

  private connection : HubConnection = new HubConnectionBuilder()
    .withUrl('https://localhost:7085/chat')
    .build();

  
  constructor(){
    this.connection.on('ReceiveMessage', (user, message) => {
      this.messages.push(`${user}: ${message}`);
      this.messagesSubject.next(this.messages.slice()); // Notify subscribers

    });
    
    this.start();
  }

  //start connection
  public async start(){
    try{
      await this.connection.start();
      console.log("Connection is estalished")
    } catch (error){
      console.log(error);
      //setTimeout(()=> this.start(), 5000)
    }
  }

  async sendMessage(user : string, message : string) {
    if (!user || !message) return;
    await this.connection.invoke('SendMessage', user, message);
  }

  getMessages(): Observable<string[]>{
    return this.messagesSubject.asObservable();
  }
  
}
