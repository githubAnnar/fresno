import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { throwError } from 'rxjs';
import { IMeasurementEx } from '../imeasurement-ex.interface';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class HelpersModule {
  public handleError(error: any) {
    console.error('server error:', error);
    if (error.error instanceof Error) {
      const errMessage = error.error.message;
      return throwError(errMessage);
    }

    return throwError(error || 'Node.js server error');
  }

  public getLactateArray(measurements: IMeasurementEx[]): number[][] {
    let data = measurements.map(m => { return [m.Load, m.Lactate] });
    return data;
  }

  public getHeartRateArray(measurements: IMeasurementEx[]): number[][] {
    let data = measurements.map(m => { return [m.Load, m.HeartRate] });
    return data;
  }
}
