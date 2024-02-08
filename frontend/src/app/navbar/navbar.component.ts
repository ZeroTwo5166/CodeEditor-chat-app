import { Component, OnInit, Renderer2 } from '@angular/core';
import { LoginStatusService } from '../services/login-status.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})

export class NavbarComponent{
  showMenu: boolean = false;
  isUserSignedIn : boolean = false; // Set this to true when the user is signed in
  userName : string = ""; // Replace with the actual username
  hoverText : string = ""; //for displayibg onhover
  userProfilePic : string = ""; //for displaying profile pic

  constructor(private userLoggedinState : LoginStatusService){
  }

  //When component is initialized, make sure you are observing all the values from the service
  ngOnInit(){
    this.userLoggedinState.getLoginStatusObservable().subscribe((status: boolean) => {
      this.isUserSignedIn = status;
    });
    this.userLoggedinState.getUsernameObservable().subscribe((username : string) => {
      this.userName = username;
    })
    this.userLoggedinState.getProfilePicObservable().subscribe((profile : string)=> {
      this.userProfilePic = profile;
    })

    this.isUserSignedIn = localStorage.getItem('LoggedInUser') ? true : false;
    const storedObjectString  = localStorage.getItem('LoggedInUser');
    
    if(storedObjectString  !== null){
      const storedObject = JSON.parse(storedObjectString);
      this.userProfilePic = storedObject.profilePic;
    }

  }

  toggleMenu() {
    this.showMenu = !this.showMenu;
  }

  closeMenu() {
    this.showMenu = false;
  }

  logout() {
    // Set isUserSignedIn to false and perform any other necessary actions
    this.isUserSignedIn = false;
    this.userLoggedinState.logout();
    location.reload(); //reload the page --> to route to trycodeEditor
  }

  showHoverText(): void {
    // Set the hover text when the mouse hovers over the image
    this.hoverText = `${this.userName}`
  }

  hideHoverText(): void {
    // Clear the hover text when the mouse moves out of the image
    this.hoverText = '';
  }
}
