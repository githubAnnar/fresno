import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonsComponent } from './persons.component';
import { PersonsRoutingModule } from './persons-routing.module';

@NgModule({
  declarations: [PersonsComponent],
  imports: [
    CommonModule,
    PersonsRoutingModule
  ],
  exports: [
    PersonsComponent
  ]
})

export class PersonsModule { }
