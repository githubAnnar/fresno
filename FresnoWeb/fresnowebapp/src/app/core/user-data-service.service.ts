import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { HelpersModule, IDeleteUserMessage, IGetUserMessage, IGetUsersMessage, IPatchUserMessage, IPostNewUserMessage, IStepTest, IUser } from '../shared';

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

  getUserById(id: number): Observable<IGetUserMessage> {
    return this.http.get<IGetUserMessage>(`${this.baseUrl}getuserbyid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getUserByStepTestId(id: number): Observable<IGetUserMessage> {
    return this.http.get<IGetUserMessage>(`${this.baseUrl}getuserbysteptestid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getUserByStepTest(stepTest: IStepTest): Observable<IGetUserMessage> {
    return this.http.get<IGetUserMessage>(`${this.baseUrl}getuserbysteptestid/${stepTest.Id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  addNewUser(user: IUser): Observable<IPostNewUserMessage> {
    return this.http.post<IPostNewUserMessage>(`${this.baseUrl}postnewuser`, user)
      .pipe(catchError(this.helpers.handleError));
  }

  updateUser(user: IUser): Observable<IPatchUserMessage> {
    return this.http.patch<IPatchUserMessage>(`${this.baseUrl}updateuser`, user)
      .pipe(catchError(this.helpers.handleError));
  }

  deleteUser(user: IUser): Observable<IDeleteUserMessage> {
    return this.http.delete(`${this.baseUrl}deleteuserbyid/${user.Id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  deleteUserById(id: number): Observable<IDeleteUserMessage> {
    return this.http.delete(`${this.baseUrl}deleteuserbyid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }
}
