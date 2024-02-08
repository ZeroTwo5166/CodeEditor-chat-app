import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { loginModel, registerModel } from '../others';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrl = "https://localhost:7235/api/User";
  constructor(private http : HttpClient) { }

  login(credentials : loginModel){
    const url = `${this.baseUrl}/login`;
    return this.http.post(url, credentials);
  }

  register(userDetails: registerModel): Observable<any> {
    const url = `${this.baseUrl}/register`; 
    return this.http.post(url, userDetails);
  }

  retrieveImage(userName: string): Observable<any> {
    const dataSent = {
      Username : userName
    }
    return this.http.post(`${this.baseUrl}/retrieve`,dataSent);
  }

  byteConverter(data : any){
    const url = `${this.baseUrl}/byteConverter`;
    return this.http.post(url, data)
  }

  sendCodeToSql(title : string, code : string){
    const url = "https://localhost:7235/api/code/save";
    return this.http.post(url, { title, code} )
  }

  getIdByUsername(username : string){
    return this.http.get(`${this.baseUrl}/getidbyusername/${username}`);
  }

  insertCodeToSql(title: string, code: string): Observable<any> {
    const url = `https://localhost:7235/api/code/insert`; // Adjust the actual insert API endpoint
    const payload = { title, code };

    return this.http.post(url, payload);
  }

  updateCodeToSql(title: string, code: string): Observable<any> {
    const url = `https://localhost:7235/api/code/update`; // Adjust the actual update API endpoint
    const payload = { title, code };

    return this.http.put(url, payload);
  }

  //DOnt need this
  getUser(email : string): Observable<any>{

    return this.http.get(`${this.baseUrl}/getuser?Email=${email}`)
  }

}
