import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { DocumentService } from '../document.service';
import { HubService } from '../services/hub.service';
import { ToastrService } from 'ngx-toastr';
import { ProjectApiService } from '../services/project-api.service';

@Component({
  selector: 'app-join-project',
  templateUrl: './join-project.component.html',
  styleUrl: './join-project.component.css'
})
export class JoinProjectComponent {

  room: string = '';
  username : string = "";

  constructor(private dialogRef: MatDialogRef<JoinProjectComponent>,
    private router: Router, 
    private chatService : DocumentService, 
    private hubService : HubService,
    private toastr : ToastrService,
    private projectService : ProjectApiService) {}

  async joinRoom() {
    const userObj = localStorage.getItem("LoggedInUser");
    if(userObj){
      this.username = JSON.parse(userObj).username;
    }

    if(this.room !== ""){
      try{
        const response: any = await this.hubService.checkRoomValidity(this.room).toPromise();

        if(response.containsRoom){
          this.chatService.joinRoom(this.username, this.room); //If room is valid, allow user to join the room
          this.toastr.success(`Joined room: ${this.room}`,"Room Joined Success", {
            timeOut:1000,
            progressBar:false
          })
          this.dialogRef.close();

          localStorage.setItem("AllowEditorAccess", "True");

          setTimeout(()=>{
            this.router.navigate(['code-editor'])
          }, 500)
        }
        else {
          //If room is not valid/doesnt exists, let the user know
          this.toastr.error(`Room "${this.room}" doesn't exists!`,"Room Joined Failed", {
            timeOut:1000,
            progressBar:false
          })
        }
      } catch(error){
        console.log(error)
      }
    }
    else {
      this.toastr.error(`Enter room`,"Room Joined Failed", {
        timeOut:1000,
        progressBar:false
      })
    } 
  }

  closeDialog() {
    // Clear input values and close the dialog
    this.room = '';
    this.dialogRef.close();
  }

  
}


/*

async getCodeFromDb(){
    try {
      // Get the projectId from the chatService
      //const projectId = await this.chatService.projectIdSubject.value;
      const projectId : any = "5D541AD9-A6FA-4A0F-0D06-08DC26884A34"; 
      // Fetch code from the database using projectService
      this.projectService.getCode(projectId).subscribe((response: any) => {
        // Extract code from the response (adjust this based on your actual API response structure)
        const code = response.code;
        //console.log(code)
        // Send the received code to the initialDocumentSubject in DocumentService
        this.chatService.setInitialDocument(code);
      });
    } catch (error) {
      console.error('Error fetching code from the database:', error);
    }
  }

if(this.room != ""){
      this.hubService.checkRoomValidity(this.room).subscribe((response : any)=>{
        if(response.containsRoom){
          this.chatService.joinRoom(this.username, this.room, ""); //If room is valid, allow user to join the room
          console.log(this.username, this.room)
          this.toastr.success(`Joined room: ${this.room}`,"Room Joined Success", {
            timeOut:1000,
            progressBar:false
          })
          this.dialogRef.close();
          setTimeout(()=>{
            this.router.navigate(['code-editor'])
          }, 500)
        } else {
          //If room is not valid/doesnt exists, let the user know
          this.toastr.error(`Room "${this.room}" doesn't exists!`,"Room Joined Failed", {
            timeOut:1000,
            progressBar:false
          })
        }
      })
    } else {
      this.toastr.error(`Enter room`,"Room Joined Failed", {
        timeOut:1000,
        progressBar:false
      })
    } */