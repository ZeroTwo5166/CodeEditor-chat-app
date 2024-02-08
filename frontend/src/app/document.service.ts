import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { projectDTO, userDTO } from './others';

@Injectable({
  providedIn: 'root'
})

export class DocumentService {

  private hubConnection!: HubConnection;

  private initialDocumentSubject = new BehaviorSubject<string>('console.log()');
  intialDoc$ = this.initialDocumentSubject.asObservable();

  private roomStatusSubject = new Subject<string>();
  roomStausSubject$ = this.roomStatusSubject.asObservable();

  // BehaviorSubject to hold the last received document and notify new subscribers
  private documentUpdateSubject = new BehaviorSubject<string>('');
  // Observable for document updates
  documentUpdate$ = this.documentUpdateSubject.asObservable();

  projectIdSubject = new BehaviorSubject<any>(null); // Add this line
  projectId$ = this.projectIdSubject.asObservable(); // Add this line


  constructor() {
    this.initializeHubConnection();

  }

  private initializeHubConnection(){
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7235/chat')
      .build();

    this.hubConnection
      .start()
      .then(() => {
      console.log('Connected to SignalR hub')
    })
      .catch((error) => console.error('Failed to connect to SignalR hub', error));

    this.hubConnection.on("ReceiveMessage", (user:string, message:string, messageTime:string, userCode : string) => {
      console.log("User: ", user);
      console.log("Message: ", message);
      console.log("messageTime: ", messageTime);
      console.log("UserCode: ", userCode);
      this.initialDocumentSubject.next(userCode);
    })

    this.hubConnection.on("LeaveMessage", (user:string, message: string, messagetime:string) => {
      console.log("User: ", user);
      console.log("Message: ", message);
      console.log("messageTime: ", messagetime);
    })

    // Register a method to receive document updates from the server
    this.hubConnection.on('ReceiveDocumentUpdate', (updatedDocument: string) => {
      // Notify subscribers about the document update
      this.documentUpdateSubject.next(updatedDocument);
    });
  }

  public async joinRoom(_user: string, _room: string) {
    const userDTO : userDTO = {
      user: _user,
      room : _room,
      code : ""
    }

    const projectDTO : projectDTO = {
      projectId : null,
      projectName : "",
      room : _room,
      code : ""
    }

    try {
      await this.hubConnection.invoke("JoinRoom", userDTO, projectDTO);
    } catch (error) {
      console.error("Error joining room:", error);
    }
  }

  public async createRoom(_user: string, _room: string, projectTitle:string) {
    const projectId = await this.projectIdSubject.value;
    //console.log("projectId from createRoom Doc", projectId)
    const userDTO : userDTO = {
      user:_user,
      room : _room,
      code: ""
    }

    const projectDto : projectDTO = {
      projectId : projectId,
      projectName : projectTitle,
      room: _room,
      code : ""
    }
    try {
      await this.hubConnection.invoke("JoinRoom", userDTO, projectDto);
    } catch (error) {
      console.error("Error creating room:", error);
    }
  }

  public async editRoom(_user:string, _room:string, _projectTitle:string,  _projectId: any, _code: string){
    const userDTO : userDTO = {
      user:_user,
      room : _room,
      code: _code
    }

    const projectDto : projectDTO = {
      projectId : _projectId,
      projectName : _projectTitle,
      room: _room,
      code : ""
    }
    try {
      await this.hubConnection.invoke("JoinRoom", userDTO, projectDto);
    } catch (error) {
      console.error("Error creating room:", error);
    }

  }

  // Add a method to broadcast document updates to all clients 
  public async broadcastDocumentUpdate(updatedDocument: string) {
    this.hubConnection.invoke('BroadcastDocumentUpdate', updatedDocument).catch((error) => {
      console.error('Error invoking BroadcastDocumentUpdate:', error);
    }); 
  }

  public async leaveChat(){
    localStorage.removeItem("ProjectId");
    return this.hubConnection.stop();
  }

  public async RoomValidator(): Promise<boolean> {
      return new Promise<boolean>((resolve, reject) => {
          this.hubConnection.on("RoomErrorMessage", (user: string, message: string, messageTime: string, userCode: string) => {
              console.log("Room error Message: ", message);
              resolve(message !== "Room does not exist!");
          });
      });
  }

  public async restartService() {
    await this.leaveChat();
    this.initializeHubConnection();
  }

  // Method to set projectId to projectIdSubject
  public setProjectId(projectId: string) {
    this.projectIdSubject.next(projectId);
  }

  // Method to set the initial document from outside the class
  public setInitialDocument(code: string) {
    this.initialDocumentSubject.next(code);
  }
  

}
