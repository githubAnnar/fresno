import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StepTestsRoutingModule } from './step-tests-routing.module';
import { StepTestsComponent } from './step-tests.component';
import { StepTestsListComponent } from './step-tests-list/step-tests-list.component';


@NgModule({
  declarations: [
    StepTestsComponent,
    StepTestsListComponent
  ],
  imports: [
    CommonModule,
    StepTestsRoutingModule
  ],
  exports:[
    StepTestsListComponent
  ]
})
export class StepTestsModule { }
