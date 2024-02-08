import { Component, Inject, AfterViewInit, Renderer2, ElementRef } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ProjectApiService } from '../services/project-api.service';
import { DocumentService } from '../document.service';
import { ToastrService } from 'ngx-toastr';
import { HubService } from '../services/hub.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-project',
  templateUrl: './edit-project.component.html',
  styleUrl: './edit-project.component.css'
})
export class EditProjectComponent implements AfterViewInit{

  room : string = ""
  initialProjectName = this.data.message
  username : string = "";
  code : string = "";//Code from db

  constructor(
    public dialogRef: MatDialogRef<EditProjectComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { message: string, projectId : any },
    private projectService : ProjectApiService,
    private documentService : DocumentService,
    private toastr : ToastrService,
    private hubService : HubService,
    private router: Router

  ) {

  }

  ngAfterViewInit(): void {

  }

  async onEditClick() {
    await this.getCodeFromDb();
    if(this.initialProjectName !== this.data.message){
      this.projectService.updateTitle(this.data.projectId, this.initialProjectName).subscribe((response)=>{

      })
    }

    //this.createRoom();
  }

  onCancelClick(){
    this.dialogRef.close(null); // Close the dialog without passing any data
  }


  async createRoom() {
    const userObj = localStorage.getItem("LoggedInUser");
    if (userObj) {
      this.username = JSON.parse(userObj).username;
    }

    if (this.room !== "") {
      try {
        const response: any = await this.hubService.checkRoomValidity(this.room).toPromise();

        if (response.containsRoom) {
          // Room already exists, can't create it
          this.toastr.error(`Room "${this.room}" already exists!`, "Room Creation Failed", {
            timeOut: 1000,
            progressBar: false
          });
        } else {
          // Check if the user has entered a title for their project
          if (this.initialProjectName !== "") {
            this.documentService.editRoom(this.username, this.room, this.initialProjectName, this.data.projectId, this.code);
            this.toastr.success(`Room "${this.room}" created.`, "Room Created", {
              timeOut: 1000,
              progressBar: false
            });
            localStorage.setItem("AllowEditorAccess", "True")
            this.dialogRef.close();
            await this.router.navigate(['code-editor']);
          } else {
            this.toastr.error(`Enter a title!!`, "Room Creation Failed", {
              timeOut: 1000,
              progressBar: false
            });
          }
        }
      } catch (error) {
        console.error("Error checking room validity:", error);
      }
    } else {
      this.toastr.error(`Enter room!!!`, "Room Creation Failed", {
        timeOut: 1000,
        progressBar: false
      });
    }
  }

  async getCodeFromDb() {
    try {
      const response: any = await this.projectService.getCode(this.data.projectId).toPromise();
      this.code = response.code;
      console.log('Code from DB:', this.code); // Add this line for logging
      this.createRoom(); // Move createRoom() call here
    } catch (error) {
      console.error("Error retrieving code from DB:", error);
    }
  }
  
}
