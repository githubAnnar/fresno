import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { UserDataService } from './user-data.service';
import { StepTestDataService } from './step-test-data.service';
import { MeasurementDataService } from './measurement-data.service';
import { SorterService } from './sorter.service';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [
    UserDataService,
    StepTestDataService,
    MeasurementDataService,
    SorterService
  ]
})
export class CoreModule { }
