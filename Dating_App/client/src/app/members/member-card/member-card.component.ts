import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/models/Members';
import { MembersService } from 'src/app/services/members.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member!: Member;

  constructor(
    private membersService: MembersService,
    private toastr: ToastrService
    ) { }
  


  ngOnInit(): void {

  }

  addLike(Member: Member) {
    this.membersService.addLike(Member.username).subscribe(() => {
      this.toastr.success('You have liked: ' + Member.knownAs);
    });
  }
}