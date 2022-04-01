import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MeasurementsRoutingModule } from './measurements-routing.module';
import { MeasurementsComponent } from '../measurements/measurements.component';
import { MeasurementsListComponent } from './measurements-list/measurements-list.component';
import { MeasurementComponent } from './measurement/measurement.component';

@NgModule({
  declarations: [
    MeasurementsComponent,
    MeasurementsListComponent,
    MeasurementComponent
  ],
  imports: [
    CommonModule,
    MeasurementsRoutingModule
  ],
  exports: [
    MeasurementsListComponent
  ]
})
export class MeasurementsModule { }
