import { HttpClient, HttpHeaders, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, take, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../models/Members';
import { PaginatedResult } from '../models/pagination';
import { User } from '../models/user';
import { UserParams } from '../models/user-params';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.apiUrl;
  members: Member[] = [];
  user:User
  memberCache = new Map<string,PaginatedResult<Member[]>>();
  userParams: UserParams;


  constructor(private http: HttpClient,
    accountService:AccountService) {
      accountService.currentUser$.pipe(take(1)).subscribe((user: any) => {
        this.user = user as User;
        this.userParams = new UserParams(user);
      });
    }

    public get UserParams():UserParams{
      return this.userParams;
    }
    public set  UserParams(userParams: UserParams){
      this.userParams = userParams;
    }

   resetUserParams(){
     this.UserParams = new UserParams(this.user);
     return this.UserParams;
   }
  getMembers(userParams: UserParams): Observable<PaginatedResult<Member[]>> {

    let key = Object.values(userParams).join("-")
    const response = this.memberCache.get(key);

    if(response)
    {
      return of(response);
    }

    let params = this.getPaginationParams(userParams.pageNumber,userParams.pageSize);
    params = params.append('minAge', userParams.minAge.toString());
    params = params.append('maxAge', userParams.maxAge.toString());
    params = params.append('gender', userParams.gender);
    params = params.append('orderBy', userParams.orderBy);

    return this.getPaginatedResult<Member[]>(`${this.baseUrl}users`, params).pipe(tap(response=>this.memberCache.set(key,response)));
  }

  private getPaginatedResult<T>(url:string, params: HttpParams): Observable<PaginatedResult<T>> {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();

    return this.http.get<T>(url,
      {
        observe: 'response',
        params
      }).pipe(
        map((res: HttpResponse<T>) => {
          paginatedResult.result = res.body as T;
          if (res.headers.get('Pagination') !== null) {
            paginatedResult.pagination = JSON.parse(res.headers.get('Pagination') || '');
          }
          return paginatedResult;
        })
      );
  }


  getMember(username: string): Observable<Member> {
    const members = [...this.memberCache.values()];
    const allMembers = members.reduce((array:Member[], current:PaginatedResult<Member[]>) => array.concat(current.result),[]);
    const member = allMembers.find(m => m.username === username);
    if(member) return of(member);
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

  deletePhoto(id:number){
    return this.http.delete(`${this.baseUrl}users/delete-photo/${id}`,{});
  }

  setMainPhoto(id: number) {
    return this.http.put(`${this.baseUrl}users/set-main-photo/${ id }`, {});
  }

  private getPaginationParams(pageNumber:number, pageSize:number){

    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    return params;
  }

  getLikes(predicate:string,pageNumber:number,pageSize:number)
  {
    let params = this.getPaginationParams(pageNumber,pageSize);
    params = params.append('predicate', predicate);
    return this.getPaginatedResult<Partial<Member>[]>(`${this.baseUrl}likes`, params)
    // return this.http.get<Partial<Member>[]>(`${this.baseUrl}likes?predicate=${predicate}`);
  }

  addLike(username:string){
    return this.http.post(`${this.baseUrl}likes/${username}`,{});
  }

}
