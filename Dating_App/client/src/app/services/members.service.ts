import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../models/Members';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];

  constructor(private http: HttpClient) {}

  getMembers(): Observable<Member[]> {
    if (this.members.length) {
      return of(this.members);
    }

    return this.http.get<Member[]>(`${this.baseUrl}users`).pipe(
      tap((members) => {
        this.members = members;
      })
    );
  }

  getMember(username: string): Observable<Member> {
    const member = this.members.find((m) => m.username === username);
    if (member) {
      return of(member);
    }
    return this.http.get<Member>(`${this.baseUrl}users/${username}`);
  }

  updateMember(member: Member): Observable<Member> {
    return this.http.put<Member>(`${this.baseUrl}users`, member).pipe(
      tap((m) => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }
}
