declare var google : any;

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { loginModel, userBanner } from '../others';
import { LoginStatusService } from '../services/login-status.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit{

  loginForm !: FormGroup;
  passwordRevealed: boolean = false;

  //necessary dependencies 
  constructor(private authService : ApiService,
    private loginService : LoginStatusService,
    private router : Router,
    private toastr: ToastrService, //for popoup alert/notification messages 
    private fb: FormBuilder){
      this.loginForm = this.fb.group({
        email:['', [Validators.required, Validators.email]],
        password:['', [Validators.required]]
      })
    }


  ngOnInit() : void{
    google.accounts.id.initialize({
      client_id: '196948830255-g4e93cpbsnb5fqj36gap97pbrri2uoo1.apps.googleusercontent.com',
      callback: (resp: any) => {
        this.handleLogin(resp)
      }
    });

    google.accounts.id.renderButton(document.getElementById("google-btn"), {
      theme: 'filled_blue',
      size:'large',
      shape:'rectangle',
      width: 350
    })
  }

  //login function called from api
  login() {
    const credentials: loginModel = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    };

    // Login call
    this.authService.login(credentials).subscribe(
      (response: any) => {
        //console.log(response.profilePic)
        this.loginService.setLoginStatus(true, response.username); //if loggedin via normal login
        
        this.toastr.success("Login Successful", "", {
          timeOut: 1000,
          progressBar: false
        });

        setTimeout(()=> {
          //this.router.navigate(['code-editor']);
          this.router.navigate(['dashboard']);
        },500)
      },
      error => {
        console.log(error);
        this.toastr.error(error.error, "Login failed!", {
          timeOut: 2000,
          progressBar: false
        });
      }
    );
  }
  
  //function to decode the token(google)
  private decodeToken(token : string){
    return JSON.parse(atob(token.split(".")[1]));
  }

  //if valid, logs in user and navigates to code-editor
  handleLogin(response : any){
    if(response){
      //decode token
      const payload : any = this.decodeToken(response.credential);

      //Create an object of interface userBanner for validity
      const userInfo : userBanner = {
        username : payload.name,
        profilePic : payload.picture
      }

      //setLoginstatus with goole image if loggedin via google
      this.loginService.setLoginStatus(true, userInfo.username, userInfo.profilePic);

      // display success message and navigate to code-editor inside Angular's zone
      this.toastr.success("Login Succeful","", {
        timeOut:1000,
        progressBar:false
      })

      setTimeout(()=> {
        //this.router.navigate(['code-editor']);
        this.router.navigate(['dashboard']);
      },500)
 
    }
  }

  //show or hide password
  togglePasswordVisibility(passwordInput: HTMLInputElement): void {
    this.passwordRevealed = !this.passwordRevealed;
    passwordInput.type = this.passwordRevealed ? 'text' : 'password';
  }

}
