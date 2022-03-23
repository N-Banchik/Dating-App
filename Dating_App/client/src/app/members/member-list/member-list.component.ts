import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/models/Members';
import { PaginatedResult, Pagination } from 'src/app/models/pagination';
import { User } from 'src/app/models/user';
import { UserParams } from 'src/app/models/user-params';
import { AccountService } from 'src/app/services/account.service';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: Member[];
  filterForm: FormGroup;
  pagination: Pagination;
  userParams: UserParams;
  genderList = [
    {
      value: 'male',
      display: 'Males',
    },
    {
      value: 'female',
      display: 'Females',
    },
  ];

  constructor(
    private memberService: MembersService,
    private formBuilder: FormBuilder
  ) {
    this.userParams = this.memberService.UserParams;
  }

  ngOnInit(): void {
    this.initializeForm();
    this.loadMembers();
  }

  initializeForm() {
    this.filterForm = this.formBuilder.group({
      gender: [this.userParams.gender],
      minAge: [this.userParams.minAge],
      maxAge: [this.userParams.maxAge],
      itemsPerPage: [this.userParams.pageSize],
      orderBy: ['last Active'],
    });
  }
  setitems() {
    this.userParams.gender = this.filterForm.value.gender;
    this.userParams.minAge = this.filterForm.value.minAge;
    this.userParams.maxAge = this.filterForm.value.maxAge;
    this.userParams.orderBy = this.filterForm.value.orderBy;
    this.pagination.currentPage = 1;
  }
  loadMembers() {
    this.memberService.UserParams = this.userParams;
    this.memberService.getMembers(this.userParams).subscribe((res) => {
      this.members = res.result;
      this.pagination = res.pagination;
    });
  }

  pageChanged({ page }: any) {
    this.userParams.pageNumber = page;
    this.memberService.UserParams = this.userParams;
    this.loadMembers();
  }

  resetFilters() {
    this.userParams = this.memberService.resetUserParams();
    this.initializeForm();
    this.loadMembers();
  }
}
