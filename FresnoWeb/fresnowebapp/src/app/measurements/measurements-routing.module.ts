import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CanOpen } from '../core';
import { MeasurementComponent } from './measurement/measurement.component';
import { MeasurementsComponent } from './measurements.component';

const routes: Routes = [
  { path: 'measurements', component: MeasurementsComponent, canActivate: [CanOpen] },
  { path: 'measurement/:id', component: MeasurementComponent, canActivate: [CanOpen] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MeasurementsRoutingModule { }
