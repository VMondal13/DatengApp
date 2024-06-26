import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  registerMode = false;
  users: any;

  constructor(private http: HttpClient){

  }

  ngOnInit(): void {   
    this.getUsers();
  }

  
  getUsers(){
    this.http.get('https://localhost:5001/api/users').subscribe({
      next: response => {
        this.users = response;
        console.log(this.users);
      },
      error: error => console.log(error),
      complete: () => console.log('this request is completed.')      
  });
  }

  registerTogle(){
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(trigger: boolean){
    this.registerMode = trigger;
  }
}
