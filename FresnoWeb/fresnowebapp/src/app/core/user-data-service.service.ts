import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { HelpersModule, IGetUsersMessage } from '../shared';

@Injectable({
  providedIn: 'root'
})
export class UserDataServiceService {

  // local server
  baseUrl: string = 'http://localhost:8000/api/user/'
  helpers: HelpersModule = new HelpersModule();

  constructor(private http: HttpClient) {
    this.helpers = new HelpersModule();
  }

  getAllUsers(): Observable<IGetUsersMessage> {
    return this.http.get<IGetUsersMessage>(`${this.baseUrl}getallusers`)
      .pipe(catchError(this.helpers.handleError))
  }
}
