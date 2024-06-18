import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/User';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'Dating App';
  //users: any;
  
  constructor(private http: HttpClient, private accountService: AccountService){}

  ngOnInit(): void {   
  //this.getUsers();
  this.setCurrentUser();
  }


  setCurrentUser(){
    console.log('app ngonit setcurrentuser');
    const strUser = localStorage.getItem('user');
    if(!strUser) return;
    else{
      const user: User = JSON.parse(strUser);
      this.accountService.setCurrentUser(user);
    }

  }
}
