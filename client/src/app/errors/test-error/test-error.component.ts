import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent {
  baseUrl = 'https://localhost:5001/api/';
  validationError: string[] = []
  constructor(private http: HttpClient){
  

  }
    
  get404Error(){
    this.http.get(this.baseUrl + 'buggy/not-found').subscribe({
      next: response => {
        console.log(response);
      },
      error: error => console.log(error),
      complete: () => console.log('this request is completed.')      
  });
  }

  get500Error(){
    this.http.get(this.baseUrl + 'buggy/server-error').subscribe({
      next: response => {
        console.log(response);
      },
      error: error => console.log(error),
      complete: () => console.log('this request is completed.')      
  });
  }

  get400Error(){
    this.http.get(this.baseUrl + 'buggy/bad-request').subscribe({
      next: response => {
        console.log(response);
      },
      error: error => console.log(error),
      complete: () => console.log('this request is completed.')      
  });
  }

  get401Error(){
    this.http.get(this.baseUrl + 'buggy/auth').subscribe({
      next: response => {
        console.log(response);
      },
      error: error => console.log(error),
      complete: () => console.log('this request is completed.')      
  });
  }

  get400ValidationError(){
    this.http.post(this.baseUrl + 'account/register', {}).subscribe({
      next: response => {
        console.log(response);
      }, 
      error: error => {
        console.log(error);
        this.validationError = error;
        console.log(this.validationError);
      },
      complete: () => console.log('this request is completed.')      
  });
  }
}
