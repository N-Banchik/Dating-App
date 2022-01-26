import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MembersRoutingModule } from './members-routing.module';
import { MembersComponent } from './members.component';
import { MemberListComponent } from 'src/app/members/member-list/member-list.component';
import { MemberDetailsComponent } from 'src/app/members/member-details/member-details.component';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', component: MemberListComponent, pathMatch: 'full' },
  { path: ':id', component: MemberDetailsComponent },
];
@NgModule({
  declarations: [MembersComponent, 
    MemberListComponent,
     MemberDetailsComponent],

  imports: [CommonModule,
     MembersRoutingModule,
     RouterModule.forChild(routes)],

  exports: [MembersComponent,
     MemberListComponent,
    MemberDetailsComponent],
})
export class MembersModule {}
