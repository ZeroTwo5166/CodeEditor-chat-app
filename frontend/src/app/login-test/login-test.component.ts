import { Component } from '@angular/core';
import { CookieLoginService } from '../services/cookie-login.service';

@Component({
  selector: 'app-login-test',
  templateUrl: './login-test.component.html',
  styleUrl: './login-test.component.css'
})
export class LoginTestComponent {

  username !: string;
  password !:string;
  isLoggedIn : boolean = false;

  constructor(private loginService : CookieLoginService){}

  loginBtn(){
    const credentials = { email : this.username, password : this.password}
    this.loginService.login(credentials).subscribe((response:any) => {
      console.log(response);
      localStorage.setItem("loggedInUser", response.username);
      this.isLoggedIn = true;
    })
  }


  logOut(){
    this.loginService.logout();
    this.isLoggedIn = false;

  }
}
