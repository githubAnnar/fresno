import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { HelpersModule, IDeleteMeasurementMessage, IGetMeasurementMessage, IGetMeasurementsMessage, IMeasurement, IPatchMeasurementMessage, IPostNewMeasurementMessage, IStepTest } from '../shared';

@Injectable({
  providedIn: 'root'
})
export class MeasurementDataServiceService {

  // local server
  baseUrl: string = 'http://localhost:8000/api/measurement/'
  helpers: HelpersModule = new HelpersModule();

  constructor(private http: HttpClient) {
    this.helpers = new HelpersModule();
  }

  getAllMeasurements(): Observable<IGetMeasurementsMessage> {
    return this.http.get<IGetMeasurementsMessage>(`${this.baseUrl}getallmeasurements`)
      .pipe(catchError(this.helpers.handleError));
  }

  getAllMeasurementsByStepTest(stepTest: IStepTest): Observable<IGetMeasurementsMessage> {
    return this.http.get<IGetMeasurementsMessage>(`${this.baseUrl}getallmeasurementsbysteptestid/${stepTest.Id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getAllMeasurementsByStepTestId(id: number): Observable<IGetMeasurementsMessage> {
    return this.http.get<IGetMeasurementsMessage>(`${this.baseUrl}getallmeasurementsbysteptestid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  getMeasurementById(id: number): Observable<IGetMeasurementMessage> {
    return this.http.get<IGetMeasurementMessage>(`${this.baseUrl}getmeasurementbyid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }

  addNewMeasurement(measurement: IMeasurement): Observable<IPostNewMeasurementMessage> {
    return this.http.post<IPostNewMeasurementMessage>(`${this.baseUrl}postnewmeasurement`, measurement)
      .pipe(catchError(this.helpers.handleError));
  }

  updateMeasurement(measurement: IMeasurement): Observable<IPatchMeasurementMessage> {
    return this.http.patch<IPatchMeasurementMessage>(`${this.baseUrl}updatemeasurement`, measurement)
      .pipe(catchError(this.helpers.handleError));
  }

  deleteMeasurementById(id: number): Observable<IDeleteMeasurementMessage> {
    return this.http.delete<IDeleteMeasurementMessage>(`${this.baseUrl}deletemeasurementbyid/${id}`)
      .pipe(catchError(this.helpers.handleError));
  }
}
