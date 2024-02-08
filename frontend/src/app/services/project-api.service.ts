import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { projectModel } from '../others';

@Injectable({
  providedIn: 'root'
})
export class ProjectApiService {

  private baseUrl = "https://localhost:7235/api/Project";
  constructor(private http : HttpClient) { }

  createProject(data : projectModel){
    return this.http.post(`${this.baseUrl}/create`, data);
  }

  getCode(id : any){
    return this.http.get(`${this.baseUrl}/getCodeFromProject/${id}`)
  }

  getProjectByUserId(userId : any){
    return this.http.get(`${this.baseUrl}/getProjectsByUserId/${userId}`)
  }

  deleteProjectById(projectId : any){
    return this.http.delete(`${this.baseUrl}/deleteProjectById/${projectId}`)
  }

  updateTitle(projectId: any, newTitle : string){
    return this.http.put(`${this.baseUrl}/updateProjectTitle/${projectId}/${newTitle}`, {})
  }

}
