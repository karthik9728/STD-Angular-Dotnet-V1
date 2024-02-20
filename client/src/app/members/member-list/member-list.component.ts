import { Component } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { Member } from '../../_models/member';
import { Observable, take } from 'rxjs';
import { Pagination } from '../../_models/pagination';
import { UserParams } from '../../_models/userParams';
import { User } from '../../_models/user';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent {
  //members$: Observable<Member[]> | undefined;
  members: Member[] | undefined;
  pagination: Pagination | undefined;
  userParams: UserParams | undefined;
  user: User;

  constructor(
    private memberService: MemberService,
    private accountService: AccountService
  ) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: (user) => {
        if (user) {
          this.userParams = new UserParams(user);
          this.user = user;
        }
      },
    });
  }

  ngOnInit() {
    this.loadMembers();
  }

  loadMembers() {
    if (!this.userParams) return;

    this.memberService.getMembers(this.userParams).subscribe({
      next: (response) => {
        if (response.result && response.pagination) {
          this.members = response.result;
          this.pagination = response.pagination;
        }
      },
    });
  }

  pageChanged(event: any) {
    if (this.userParams && this.userParams.pageNumber != event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }
  }

  resetFilters() {
    if (this.user) {
      this.userParams = new UserParams(this.user);
      this.loadMembers();
    }
  }
}
