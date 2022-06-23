import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable } from 'rxjs';
import { HelpersModule, IMeasurementEx } from '../shared';

@Injectable({
  providedIn: 'root'
})
export class PolyregDataService {
  // local server
  baseUrl: string = 'http://localhost:8000/api/polyreg/'
  helpers: HelpersModule = new HelpersModule();
  constructor(private http: HttpClient) {
    this.helpers = new HelpersModule();
  }

  getLactateFit(measurements: IMeasurementEx[]): Observable<string> {
    let arr = this.helpers.getLactateArray(measurements);
    return this.http.put<string>(`${this.baseUrl}fitlactate`, arr)
      .pipe(catchError(this.helpers.handleError));
  }

  getHeartRateFit(measurements: IMeasurementEx[]): Observable<string> {
    let arr = this.helpers.getHeartRateArray(measurements);
    return this.http.put<string>(`${this.baseUrl}fitheart`, arr)
      .pipe(catchError(this.helpers.handleError));
  }
}