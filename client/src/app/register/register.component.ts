import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  model: any = {}
  @Input() usersFromHomeComp: any;
  @Output() cancelTrigger = new EventEmitter();

  constructor(private accountService: AccountService){

  }

  register(){
    console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: (response) => {
        console.log(response);
      },
      error: (error) => console.log(error)
    });
    this.cancel();
  }

  cancel(){
    console.log("cancel");
    this.cancelTrigger.emit(false);
  }
}
