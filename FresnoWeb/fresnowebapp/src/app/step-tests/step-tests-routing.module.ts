import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CanOpen } from '../core';
import { StepTestComponent } from './step-test/step-test.component';
import { StepTestsComponent } from './step-tests.component';

const routes: Routes = [
  { path: 'steptests', component: StepTestsComponent, canActivate: [CanOpen] },
  { path: 'steptest/:id', component: StepTestComponent, canActivate: [CanOpen] }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StepTestsRoutingModule { }
