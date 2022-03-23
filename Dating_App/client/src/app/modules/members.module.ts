import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberDetailsComponent } from '../members/member-details/member-details.component';
import { MemberListComponent } from '../members/member-list/member-list.component';
import { Routes, RouterModule } from '@angular/router';

import { SharedModule } from './shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


const routes: Routes = [
  { path: '', component: MemberListComponent, pathMatch: 'full' },
  { path: ':username', component: MemberDetailsComponent },
];

@NgModule({
  declarations: [
    MemberListComponent,
    MemberDetailsComponent,
    
  ],
  imports: [CommonModule, RouterModule.forChild(routes) ,SharedModule,FormsModule,ReactiveFormsModule,],
  exports: [
    RouterModule,
    
    MemberListComponent,
    MemberDetailsComponent,
   
  ],
})
export class MembersModule {}
