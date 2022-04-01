import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StepTestsRoutingModule } from './step-tests-routing.module';
import { StepTestsComponent } from './step-tests.component';
import { StepTestsListComponent } from './step-tests-list/step-tests-list.component';
import { StepTestComponent } from './step-test/step-test.component';
import { MeasurementsModule } from '../measurements/measurements.module';


@NgModule({
  declarations: [
    StepTestsComponent,
    StepTestsListComponent,
    StepTestComponent
  ],
  imports: [
    CommonModule,
    MeasurementsModule,
    StepTestsRoutingModule
  ],
  exports: [
    StepTestsListComponent
  ]
})
export class StepTestsModule { }
