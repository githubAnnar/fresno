import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';
import { HelpersModule, IDeleteStepTestMessage, IGetStepTestMessage, IGetStepTestsMessage, IMeasurement, IPatchStepTestMessage, IPostNewStepTestMessage, IStepTest, IPerson, IGetStepTestsExMessage, IGetStepTestTextMessage } from '../shared';
import { DateTimeFormattedPipe } from '../shared/pipes/date-time-formatted.pipe';

@Injectable({
  providedIn: 'root'
})
export class StepTestDataService {

  // local server
  baseUrl: string = 'http://localhost:8000/api/steptest/'
  helpers: HelpersModule = new HelpersModule();

  constructor(private http: HttpClient) {
    this.helpers = new HelpersModule();
  }

  getAllStepTests(): Observable<IGetStepTestsMessage> {
    return this.http.get<IGetStepTestsMessage>(`${this.baseUrl}getallsteptests`)
      .pipe(catchError(this.helpers.handleError));
  }

  getAllStepTestsEx(): Observable<IGetStepTestsExMessage> {
    return this.http.get<IGetStepTestsExMessage>(`${this.baseUrl}getallsteptestsex`)
      .pipe(catchError(this.helpers.handleError));
  }

  getAllStepTestsByPerson(person: IPerson): Observable<IGetStepTestsMessage> {
    return this.http.get<IGetStepTestsMessage>(`${this.baseUrl}getsteptestsbypersonid/${person.Id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getAllStepTestsByPersonId(id: number): Observable<IGetStepTestsMessage> {
    return this.http.get<IGetStepTestsMessage>(`${this.baseUrl}getsteptestsbypersonid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getStepTestById(id: number): Observable<IGetStepTestMessage> {
    return this.http.get<IGetStepTestMessage>(`${this.baseUrl}getsteptestbyid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getStepTestTextById(id: number): Observable<string> {
    return this.http.get<IGetStepTestTextMessage>(`${this.baseUrl}getsteptesttextbyid/${id}`)
      .pipe(map((res: IGetStepTestTextMessage) => `${res.data.LastName}, ${res.data.FirstName} (${new DateTimeFormattedPipe().transform(res.data.TestDate)})`), catchError(this.helpers.handleError));
  }

  getStepTestByMeasurementId(id: number): Observable<IGetStepTestMessage> {
    return this.http.get<IGetStepTestMessage>(`${this.baseUrl}getsteptestbymeasureid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getStepTestByMeasurement(measurement: IMeasurement): Observable<IGetStepTestMessage> {
    return this.http.get<IGetStepTestMessage>(`${this.baseUrl}getsteptestbymeasureid/${measurement.StepTestId}`)
      .pipe(catchError(this.helpers.handleError));
  }

  addNewStepTest(stepTest: IStepTest): Observable<IPostNewStepTestMessage> {
    return this.http.post<IPostNewStepTestMessage>(`${this.baseUrl}postnewsteptest`, stepTest)
      .pipe(catchError(this.helpers.handleError));
  }

  updateStepTest(stepTest: IStepTest): Observable<IPatchStepTestMessage> {
    return this.http.patch<IPatchStepTestMessage>(`${this.baseUrl}updatesteptest`, stepTest)
      .pipe(catchError(this.helpers.handleError));
  }

  deleteStepTestById(id: number): Observable<IDeleteStepTestMessage> {
    return this.http.delete<IDeleteStepTestMessage>(`${this.baseUrl}deletesteptestbyid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }
}
