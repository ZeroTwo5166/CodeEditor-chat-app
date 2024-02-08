import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginStatusService } from '../services/login-status.service';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CreateProjectComponent } from '../create-project/create-project.component';
import { JoinProjectComponent } from '../join-project/join-project.component';
import { GetRouteService } from '../services/get-route.service';
import { ProjectApiService } from '../services/project-api.service';
import { ApiService } from '../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { DeleteConfirmationComponent } from '../delete-confirmation/delete-confirmation.component';
import { EditProjectComponent } from '../edit-project/edit-project.component';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent{

  projects: any[] = [];
  userId !: any;
  searchQuery : string = "";

  constructor(private router: Router, 
    private apiService : ApiService,
    private loginStatus : LoginStatusService,
    private dialog : MatDialog,
    private routeService : GetRouteService,
    private toastrService : ToastrService,
    private projectService : ProjectApiService)
  {
      this.loginStatus.checkLoginStatus(router); //Check login status
      //if routed from code-editor, reload the component and services
      
      if(this.routeService.previousRoutePath.value == "/code-editor"){
        window.location.reload();
      } 

  }

  ngOnInit(): void {
    const userObj = localStorage.getItem("LoggedInUser");
    if(userObj){
      const userIdFromLS = JSON.parse(userObj).username;
      this.apiService.getIdByUsername(userIdFromLS).subscribe((response:any) => {
        this.userId = response.userId;
        //console.log(this.userId)
        this.projectService.getProjectByUserId(this.userId).subscribe((resp:any) => {
          this.projects = resp;
          console.log(this.projects)
        })
      })

    }
  }

  //Open popup for creating room
  createOptions(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = "60%";
    this.dialog.open(CreateProjectComponent, dialogConfig);
  }

  //Open popup for joining room
  joinOptions(){
    const dialogConfig = new MatDialogConfig();
    dialogConfig.disableClose = true;
    dialogConfig.autoFocus = true;
    dialogConfig.width = "60%";
    this.dialog.open(JoinProjectComponent, dialogConfig);
  }


  edit(project : any){
    const dialogRef = this.dialog.open(EditProjectComponent, {
      width: '350px',
      data: { 
        message: `${project.projectName}`,
        projectId : `${project.id}`
     }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== null && result !== undefined) { // Check if the user provided a new title
        console.log("Updated project name:", result);
        // Now you can proceed with updating the project title using the new value (result)
      }
    });
  }

  delete(project: any) {
    const dialogRef = this.dialog.open(DeleteConfirmationComponent, {
      width: '350px',
      data: { message: `Are you sure you want to delete project "${project.projectName}"?` }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) { // If user confirms deletion
        this.projectService.deleteProjectById(project.id).subscribe((response) => {
          this.toastrService.warning(`Project deleted`, '', {
            timeOut: 1000,
            progressBar: false
          });
          this.projects = this.projects.filter(p => p.id !== project.id);
        }, error => {
          console.error('Failed to delete project', error);
          // Handle error, if needed
        });
      }
    });
  }


}
