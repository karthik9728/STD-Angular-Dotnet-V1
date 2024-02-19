import { Component } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { Member } from '../../_models/member';
import { Observable } from 'rxjs';
import { Pagination } from '../../_models/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent {
  //members$: Observable<Member[]> | undefined;
  members: Member[] | undefined;
  pagination: Pagination | undefined;
  pageNumber = 1;
  pageSize = 5;

  constructor(private memberService: MemberService) {}

  ngOnInit() {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        if (response.result && response.pagination) {
          this.members = response.result;
          this.pagination = response.pagination;
        }
      },
    });
  }

  pageChanged(event: any) {
    if (this.pageNumber != event.page) {
      this.pageNumber = event.page;
      this.loadMembers();
    }
  }
}
