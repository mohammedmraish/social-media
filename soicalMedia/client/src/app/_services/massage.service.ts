import { environment } from './../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { getPaginatedResult, getPaginationHeaders } from './PaginationHelper';
import { Massage } from '../_models/massage';

@Injectable({
  providedIn: 'root',
})
export class MassageService {
  baseUrl = environment.baseUrl;
  constructor(private http: HttpClient) {}

  getMassages(pageNumber, pageSize, container) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);

    return getPaginatedResult<Massage[]>(
      this.baseUrl + 'massages',
      params,
      this.http
    );
  }

  getMassageThread(username: string) {
    return this.http.get<Massage[]>(
      this.baseUrl + 'massages/thread/' + username
    );
  }

  sendMassage(username: string, content: string) {
    return this.http.post<Massage>(this.baseUrl + 'massages', {
      recipientUsername: username,
      content: content,
    });
  }

  deleteMassage(id: number) {
    return this.http.delete(this.baseUrl + 'massages/' + id);
  }
}
