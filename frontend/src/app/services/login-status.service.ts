import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
//import { CookieService } from 'ngx-cookie-service'; //Tried using cookie but it gave error trying to store profilepic
import { userBanner } from '../others';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class LoginStatusService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(false); //BehaviourSubject for loginstatus bool
  private usernameSubject = new BehaviorSubject<string>(""); //BehaviourSubject for username string
  private profilePicSubject = new BehaviorSubject<string>(""); // Assuming profile pic is a string URL

  constructor(private apiService : ApiService) { 
  }

  //This last parameter is optional, 
  //If it is provided, use that as profile pic --> This is useful when login via google
  //If it is not provided, use the username to make an api call to backend > get the image in useable format
  setLoginStatus(isLoggedIn : boolean, username : string, isProfilePictureGiven? : string) : void {
    this.isLoggedInSubject.next(isLoggedIn);
    this.usernameSubject.next(username); 

    if(isProfilePictureGiven !== undefined){
      this.profilePicSubject.next(isProfilePictureGiven)
      this.setLocalStorage(username, isProfilePictureGiven);
    }
    else {
      this.getUserProfilePic(username);
    }

    //this.setSessionStorage(username, profilePic);
  }

  getLoginStatusObservable(): Observable<boolean> {
    return this.isLoggedInSubject.asObservable();
  }

  getUsernameObservable(): Observable<string> {
    return this.usernameSubject.asObservable();
  }

  getProfilePicObservable(): Observable<string> {
    return this.profilePicSubject.asObservable();
  }

  //This method calls an api and returns the picture URL and sends it to the behaviour subject so that observables can subscribe to it 
  getUserProfilePic(username : string){
    this.apiService.retrieveImage(username).subscribe((response : any) => {
      console.log(response)
      this.profilePicSubject.next(response.data);

      this.setLocalStorage(username, response.data);
    }, (error) => {
      console.log(error)
    })
  }

  //reset all the behaviour subjects and remove the sessionStorage
  logout(){
    this.isLoggedInSubject.next(false);
    this.usernameSubject.next("");
    this.profilePicSubject.next("");
    localStorage.removeItem('LoggedInUser');
  }

  //Storing the loggedInUser and profilepic to sessionStorage
  private setLocalStorage(username: string, profilePic: string): void {
    const loginUser : userBanner = {
      username: username,
      profilePic: profilePic
    };
    localStorage.setItem('LoggedInUser', JSON.stringify(loginUser));
  }

  //Two way check if user is loggedin
  checkLoginStatus(router : any) : void {
    this.getLoginStatusObservable().subscribe((loggedIn) => {
      if(!loggedIn && !localStorage.getItem('LoggedInUser')){
        router.navigate(["/try-code-editor"]);
      }
    })
  }

}
