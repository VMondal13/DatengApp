import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MembersDetailComponent } from './members/members-detail/members-detail.component';
import { ListComponent } from './list/list.component';
import { MessagesComponent } from './messages/messages.component';
import { TestComponent } from './test/test.component';
import { authGuard } from './_guards/auth.guard';

const routes: Routes = [
  {path:'', component: HomeComponent},
  {path:'', 
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {path:'members', component: MembersListComponent},
      {path:'members/:id', component: MembersDetailComponent},
      {path:'lists', component: ListComponent},
      {path:'messages', component: MessagesComponent},
      {path:'test', component: TestComponent}
    ]
  },  
  {path:'**', component: HomeComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
