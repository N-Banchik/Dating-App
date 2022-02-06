import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberDetailsComponent } from '../members/member-details/member-details.component';
import { MemberListComponent } from '../members/member-list/member-list.component';
import { Routes, RouterModule } from '@angular/router';
import { MemberCardComponent } from '../members/member-card/member-card.component';
import { SharedModule } from './shared.module';


const routes: Routes = [
  { path: '', component: MemberListComponent, pathMatch: 'full' },
  { path: ':username', component: MemberDetailsComponent },
];

@NgModule({
  declarations: [
    MemberListComponent,
    MemberDetailsComponent,
    MemberCardComponent,
  ],
  imports: [CommonModule, RouterModule.forChild(routes) ,SharedModule],
  exports: [
    RouterModule,
    
    MemberListComponent,
    MemberDetailsComponent,
    MemberCardComponent,
  ],
})
export class MembersModule {}
