import { Component, OnInit } from '@angular/core';
import { Member } from '../models/Members';
import { Pagination } from '../models/pagination';
import { MembersService } from '../services/members.service';
@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {

  members: Partial<Member>[] = [];
  predicate= 'liked';
  pageNumber: number = 1;
  pageSize: number = 10;
  pagination:Pagination;
  
  constructor(
    private MembersService: MembersService,
  ) { }

  ngOnInit() {
this.loadLikes();
  }

  loadLikes() {
    this.MembersService.getLikes(this.predicate,this.pageNumber,this.pageSize).subscribe(data => {
      this.members = data.result;
      this.pagination = data.pagination;
    });
  }

  pageChanged(event: any): void {
    this.pageNumber = event.page;
    this.loadLikes();
  }
}
