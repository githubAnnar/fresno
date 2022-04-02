import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonsComponent } from './persons.component';
import { PersonsRoutingModule } from './persons-routing.module';
import { PersonsListComponent } from './persons-list/persons-list.component';
import { PersonComponent } from './person/person.component';
import { StepTestsModule } from '../step-tests/step-tests.module';
import { PipesModule } from '../shared/pipes/pipes.module';

@NgModule({
  declarations: [PersonsComponent, PersonsListComponent, PersonComponent],
  imports: [
    CommonModule,
    StepTestsModule,
    PipesModule,
    PersonsRoutingModule
  ],
  exports: [
    PersonsComponent
  ]
})

export class PersonsModule { }
