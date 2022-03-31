import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StepTestsRoutingModule } from './step-tests-routing.module';
import { StepTestsComponent } from './step-tests.component';
import { StepTestsListComponent } from './step-tests-list/step-tests-list.component';
import { StepTestComponent } from './step-test/step-test.component';


@NgModule({
  declarations: [
    StepTestsComponent,
    StepTestsListComponent,
    StepTestComponent
  ],
  imports: [
    CommonModule,
    StepTestsRoutingModule
  ],
  exports: [
    StepTestsListComponent
  ]
})
export class StepTestsModule { }
