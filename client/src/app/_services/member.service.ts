import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';

@Injectable({ providedIn: 'root' })
export class MemberService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'user');
  }

  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + `user/${username}`);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'user', member);
  }
}
