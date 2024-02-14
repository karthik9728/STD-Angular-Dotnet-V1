import { Component } from '@angular/core';
import { MemberService } from '../../_services/member.service';
import { Member } from '../../_models/member';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.css',
})
export class MemberListComponent {
  members$: Observable<Member[]> | undefined;

  constructor(private memberService: MemberService) {}

  ngOnInit() {
    this.members$ = this.memberService.getMembers();
  }
}
