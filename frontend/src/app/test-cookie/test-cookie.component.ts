import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoginStatusService } from '../services/login-status.service';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-test-cookie',
  templateUrl: './test-cookie.component.html',
  styleUrl: './test-cookie.component.css'
})
export class TestCookieComponent {
  
  byteArrayForHtml : any;
  userName: string = '';

  selectedFile: File | null = null;
  imageDataURL!: string;
  imageUrl = "../../assets/default-pp.png";

  constructor(private apiService : ApiService){}

  

  getUserBtn(){
    this.apiService.getUser("zerotwo51666@gmail.com").subscribe((resp) => {
      console.log(resp)
    }, err => console.log(err))
  }

  ngOnInit(){

  }
  onFileChanged(event : any) {
    const fileInput = event.target;
    if (fileInput.files.length > 0) {
      this.selectedFile = fileInput.files[0];
      // Update the image preview
      this.imageUrl = URL.createObjectURL(this.selectedFile as Blob);
    } else {
      // User didn't choose a file, reset to default image
      this.selectedFile = null;
      this.imageUrl = this.imageUrl;
    }
  }


  retrieve(){

    console.log(this.userName)

    this.apiService.retrieveImage("subarna").subscribe((response : any) => {
      console.log(response)
      this.imageDataURL = response.data;
    }, (error) => {
      console.log(error)
    })
  }

  /*
  handleFileInput(event : any){
    const file: File = event.target.files[0];
    const reader = new FileReader();
    reader.onloadend = () => {
      // Convert the data URL to a byte array
      const byteArray = this.dataURLToByteArray(reader.result as string);
      
      // Now, `byteArray` contains the image data as a byte array
      console.log(byteArray);
      this.byteArrayForHtml = byteArray;
    };
    reader.readAsDataURL(file);
  } 
  onFileSelected(event: any) {
    const file : File = event.target.files[0];

    const formData = new FormData();

    if(file){
      formData.append('file', file, file.name)
      this.apiService.byteConverter(formData).subscribe(
        resp => {
          console.log("Success", resp)
        },
        error => {
          console.log(error)
        }
      )
    }
  }
  
  
  */


}
