import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/User';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
    model: any = {};
    //loggedIn = false;   
    //currentUser$: Observable<User | null> = of(null);

    constructor(public accountService: AccountService, private router: Router,
      private toastr: ToastrService){
      
    }

    ngOnInit(): void {
      //this.getCurrentUser();
      //this.currentUser$ = this.accountService.currentUser$;
    }

    // getCurrentUser() {
    //   console.log('nav ngonit getcurrentuser');
    //   this.accountService.currentUser$.subscribe({
    //     next: user => {
    //       console.log(user);
    //       this.loggedIn = !!user;
    //       console.log(this.loggedIn);
    //     },
    //     error: error => console.log(error)
    // })
    // }

    login() {
      console.log(this.model);
      this.accountService.login(this.model).subscribe({
        next: response => {
          console.log(response);
          //this.loggedIn = true;
          this.router.navigateByUrl('/members');
        },
        error: error => {
          console.log(error);
          // for(const value in error)
          //   this.toastr.error(error[value]);
          }
      });
    }
    logout() {
      this.accountService.logout();
      this.router.navigateByUrl('/');
          //this.loggedIn = false;

    }
}
