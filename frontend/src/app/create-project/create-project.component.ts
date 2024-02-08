import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { DocumentService } from '../document.service';
import { HubService } from '../services/hub.service';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from '../services/api.service';
import { ProjectApiService } from '../services/project-api.service';
import { projectModel } from '../others';

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrl: './create-project.component.css'
})
export class CreateProjectComponent {
  projectTitle: string = '';
  room: string = '';
  username : string = "";
  //private localStorageSetPromise: Promise<void> | null = null; // Add this line

  constructor(private dialogRef: MatDialogRef<CreateProjectComponent>,
    private router: Router, 
    private hubService : HubService,
    private toastr : ToastrService,
    private apiService:ApiService,
    private projectService : ProjectApiService,
    private documentService : DocumentService) {}

  closeDialog() {
    // Clear input values and close the dialog
    this.projectTitle = '';
    this.room = '';
    this.dialogRef.close();
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
          if (this.projectTitle !== "") {
            await this.createProjectInDb();
            //this.chatService.createRoom(this.username, this.room, this.projectTitle);
            //this.documentService.initializeRoomAndJoin(this.username, this.room, this.projectTitle);
            this.documentService.createRoom(this.username, this.room, this.projectTitle);
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

  //Checks the user, gets the userId and creates the project in the database
  async createProjectInDb() {
    const userObj: any = localStorage.getItem("LoggedInUser");
    if (userObj) {
      const userName = JSON.parse(userObj).username;
      try {
        const response: any = await this.apiService.getIdByUsername(userName).toPromise();
        const userId = response.userId;
        //console.log(userId);
        // Now you can use userId or perform further actions with it.
        const dataForProjectCreation : projectModel = {
          projectname : this.projectTitle,
          userid : userId,
          code : ""
        }

        const projectResponse: any = await this.projectService.createProject(dataForProjectCreation).toPromise();
        this.documentService.setProjectId(projectResponse.projectId); // Assuming projectResponse contains the projectId
  
        //localStorage.setItem("ProjectId", projectResponse.projectId)
        // Now you can use projectResponse.projectId or perform further actions with it.
        } catch (error) {
        console.error("Error getting user ID:", error);
      }
    }
  }


}
