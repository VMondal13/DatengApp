import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MembersDetailComponent } from './members/members-detail/members-detail.component';
import { ListComponent } from './list/list.component';
import { MessagesComponent } from './messages/messages.component';
import { TestComponent } from './test/test.component';
import { authGuard } from './_guards/auth.guard';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';

const routes: Routes = [
  {path:'', component: HomeComponent},
  {path:'', 
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {path:'members', component: MembersListComponent},
      {path:'members/:username', component: MembersDetailComponent},
      {path:'member/edit', component: MemberEditComponent, canDeactivate: [preventUnsavedChangesGuard]},
      {path:'lists', component: ListComponent},
      {path:'messages', component: MessagesComponent},
      {path:'test', component: TestComponent}
    ]
  },  
  {path:'errors', component: TestErrorComponent},
  {path:'not-found', component: NotFoundComponent},
  {path:'server-error', component: ServerErrorComponent},
  {path:'**', component: NotFoundComponent, pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
