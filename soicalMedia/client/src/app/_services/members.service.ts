import { PaginatedResult } from './../_models/pagination';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from './../../environments/environment';
import { Injectable } from '@angular/core';
import { Member } from '../_models/member';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';
import { PhotoEditorComponent } from '../members/photo-editor/photo-editor.component';

@Injectable({
  providedIn: 'root',
})
export class MembersService {
  baseUrl = environment.baseUrl;
  members: Member[] = [];
  paginatedResult: PaginatedResult<Member[]> = new PaginatedResult<Member[]>();

  constructor(private http: HttpClient) {}

  getMembers(page?: number, itemsPerPage?: number) {
    //save state
    // if (this.members.length > 0) return of(this.members);

    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }
    return this.http
      .get<Member[]>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(
        map((res) => {
          this.paginatedResult.result = res.body;
          if (res.headers.get('Pagination') != null) {
            this.paginatedResult.pagination = JSON.parse(
              res.headers.get('Pagination')
            );
          }
          return this.paginatedResult;
        })
      );
  }

  getMember(username: string) {
    //save state
    const member = this.members.find((x) => x.username == username);
    if (member !== undefined) return of(member);

    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = member;
      })
    );
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(photoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photoId);
  }

  addLike(username: string) {
    return this.http.post(this.baseUrl + 'likes/' + username, {});
  }

  getLikes(predicate: string) {
    return this.http.get<Partial<Member[]>>(
      this.baseUrl + 'likes?predicate=' + predicate
    );
  }
}
