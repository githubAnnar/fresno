import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OnlyDatePipe } from './only-date.pipe';
import { DateTimeFormattedPipe } from './date-time-formatted.pipe';
import { Micro10SecFormattedPipe } from './micro10-sec-formatted.pipe';



@NgModule({
  declarations: [
    OnlyDatePipe,
    DateTimeFormattedPipe,
    Micro10SecFormattedPipe
  ],
  imports: [
    CommonModule
  ],
  exports: [
    OnlyDatePipe,
    DateTimeFormattedPipe,
    Micro10SecFormattedPipe
  ]
})
export class PipesModule { }
