import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BackgroundColorService } from '../services/background-color.service';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import { ApiService } from '../services/api.service';
import { registerModel } from '../others';
import { LoginStatusService } from '../services/login-status.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})

export class RegisterComponent implements OnInit{
  @ViewChild('colorInput1') colorInput1!: ElementRef<HTMLInputElement>;
  @ViewChild('colorInput2') colorInput2!: ElementRef<HTMLInputElement>;

  signupForm!: FormGroup;
  profilePicByte : any;

  selectedFile: File | null = null;

  profilePicPreview: string = '../../assets/default-pp.png'; //default profile pic

  color1: string = "#11088c";
  color2: string = "#b818b3";
  passwordRevealed1: boolean = false;
  passwordRevealed2: boolean = false;
  passwordMatchError: boolean = false;

  constructor(private backgroundColorService: BackgroundColorService,
    private fb: FormBuilder,
    private authService : ApiService,
    private loginService : LoginStatusService,
    private toastr: ToastrService,
    private router : Router) { }

  ngOnInit(): void {
    //just a background change service
    this.backgroundColorService.color1$.subscribe(color => this.color1 = color);
    this.backgroundColorService.color2$.subscribe(color => this.color2 = color);
    //signupform for the whole register field
    this.signupForm = this.fb.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email, this.emailValidator()]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(8)]],
    }, { validator: this.passwordMatch("password", "confirmPassword") });
  }
  
  //main register function on button click
  register() {
    const formData: FormData = new FormData(); //As our backend takes formdata to convert into byte[]
    if (this.selectedFile) {
      formData.append('file', this.selectedFile, this.selectedFile.name);
      //byteconverter
      this.authService.byteConverter(formData).subscribe(
        (response) => {
          console.log(response);
          this.profilePicByte = response;
          this.apiCallForRegister(response);
        },
        (error) => console.log(error)
      );
    }
    else {
      this.apiCallForRegister(null);
    }
  }
  
  //creating the function for api call because it is used in two different scopes, 
  //Implementing the DRY principle
  //This function sets the profilepic type which can be either byte[] or null
  apiCallForRegister(profilePic : any) {
    const credentials: registerModel = {
      username: this.signupForm.value.username,
      email: this.signupForm.value.email,
      password: this.signupForm.value.password,
      profilePic: profilePic,
    };
      
    this.authService.register(credentials).subscribe(
      (response: any) => {
        this.loginService.setLoginStatus(true, this.signupForm.value.username);
        console.log(response);
        this.toastr.success('Your account has been created..', 'Registration Successful', {
          timeOut: 1000,
          progressBar: false,
        });
        //this.router.navigate(['code-editor'])
        this.router.navigate(['dashboard'])
        setTimeout(() => {
          // this.router.navigate(['code-editor'])
        }, 1000);
      },
      (error) => {
        console.log(error);
        this.toastr.error(`${error.error}!!!`, 'Registration Failed', {
          timeOut: 1000,
          progressBar: false,
        });
      }
    );


  }

  // Method to handle profile picture changes
  handleProfilePicChange(event: any): void {
    //Get the file input
    const file = event.target.files[0];

    // Check if a file is selected
    if (file) {
      this.selectedFile = file; //if file is selected, pass it to the selectedFile variable
      // Read the file as a data URL and display for preview
      const reader = new FileReader();
      reader.onload = (e: any) => {
        this.profilePicPreview = e.target.result;
      };
      reader.readAsDataURL(file);
    } else {
      this.selectedFile = null; //if file is not selected, set it to null
    }
  }

  //Get signupform control
  getControl(name : any) : AbstractControl | null{
    return this.signupForm.get(name)
  }

  //custom email validator 
  emailValidator(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const email = control.value as string;
  
      if (!email || email.indexOf('@') === -1) {
        return { invalidEmail: true };
      }
      return null;
    };
  }

  //custom password match validator
  passwordMatch(password : string, confirm_password : string){
    return function(form:AbstractControl){
      const passwordValue = form.get(password)?.value
      const confirmPasswordValue = form.get(confirm_password)?.value
      
      if(passwordValue === confirmPasswordValue){
        return null;
      }
      return { passwordMismatchError : true}
    }
  }

  //just a simple background set function
  setColor1(color: any): void {
    this.backgroundColorService.setColor1(color.target.value);
  }
  
  //just a simple background set function
  setColor2(color: any): void {
    this.backgroundColorService.setColor2(color.target.value);
  }

  //as the name says. For first password input field
  togglePasswordVisibilityOne(passwordInput: HTMLInputElement): void {
    this.passwordRevealed1 = !this.passwordRevealed1;
    passwordInput.type = this.passwordRevealed1 ? 'text' : 'password';
  }

  //for second password input field
  togglePasswordVisibilityTwo(passwordInput: HTMLInputElement): void {
    this.passwordRevealed2 = !this.passwordRevealed2;
    passwordInput.type = this.passwordRevealed2 ? 'text' : 'password';
  }

}
