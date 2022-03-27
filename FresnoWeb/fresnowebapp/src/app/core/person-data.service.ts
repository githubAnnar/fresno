import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { HelpersModule, IDeletePersonMessage as IDeletePersonMessage, IGetPersonMessage as IGetPersonMessage, IGetPersonsMessage as IGetPersonsMessage, IPatchUserMessage as IPatchPersonMessage, IPostNewPersonMessage as IPostNewPersonMessage, IStepTest, IPerson as IPerson, IGetPersonNameMessage } from '../shared';

@Injectable({
  providedIn: 'root'
})
export class PersonDataService {

  // local server
  baseUrl: string = 'http://localhost:8000/api/person/'
  helpers: HelpersModule = new HelpersModule();

  constructor(private http: HttpClient) {
    this.helpers = new HelpersModule();
  }

  getAllPersons(): Observable<IGetPersonsMessage> {
    return this.http.get<IGetPersonsMessage>(`${this.baseUrl}getallpersons`)
      .pipe(catchError(this.helpers.handleError))
  }

  getPersonById(id: number): Observable<IGetPersonMessage> {
    return this.http.get<IGetPersonMessage>(`${this.baseUrl}getpersonbyid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getPersonByStepTestId(id: number): Observable<IGetPersonMessage> {
    return this.http.get<IGetPersonMessage>(`${this.baseUrl}getpersonbysteptestid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getPersonByStepTest(stepTest: IStepTest): Observable<IGetPersonMessage> {
    return this.http.get<IGetPersonMessage>(`${this.baseUrl}getpersonbysteptestid/${stepTest.Id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getPersonNameById(id: number): Observable<IGetPersonNameMessage> {
    return this.http.get<IGetPersonNameMessage>(`${this.baseUrl}getpersonnamebyid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  addNewPerson(user: IPerson): Observable<IPostNewPersonMessage> {
    return this.http.post<IPostNewPersonMessage>(`${this.baseUrl}postnewperson`, user)
      .pipe(catchError(this.helpers.handleError));
  }

  updatePerson(user: IPerson): Observable<IPatchPersonMessage> {
    return this.http.patch<IPatchPersonMessage>(`${this.baseUrl}updateperson`, user)
      .pipe(catchError(this.helpers.handleError));
  }

  deletePerson(user: IPerson): Observable<IDeletePersonMessage> {
    return this.http.delete<IDeletePersonMessage>(`${this.baseUrl}deletepersonbyid/${user.Id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  deletePersonById(id: number): Observable<IDeletePersonMessage> {
    return this.http.delete<IDeletePersonMessage>(`${this.baseUrl}deletepersonbyid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }
}
