import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/Member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.css']
})
export class MembersListComponent implements OnInit{
  //members: Member[] = []
  members$: Observable<Member[]> | undefined

  constructor(private membersService: MembersService){

  }

  ngOnInit(): void {
    //this.loadMembers();
    this.members$ = this.membersService.getMembers();
  }

  // loadMembers(){
  //   this.membersService.getMembers().subscribe({
  //     next: (members) => {
  //       this.members = members;
  //       console.log(members);
        
  //     }
  //   })
  // }
}
