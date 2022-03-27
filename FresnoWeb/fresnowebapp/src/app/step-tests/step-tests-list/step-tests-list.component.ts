import { Component, Input, OnInit } from '@angular/core';
import { PersonDataService, SorterService } from 'src/app/core';
import { IGetPersonNameMessage, IStepTest } from 'src/app/shared';

@Component({
  selector: 'app-step-tests-list',
  templateUrl: './step-tests-list.component.html',
  styleUrls: ['./step-tests-list.component.css']
})
export class StepTestsListComponent implements OnInit {
  private _stepTests: IStepTest[] = [];

  @Input() get listStepTests(): IStepTest[] {
    return this._stepTests;
  }

  set listStepTests(value: IStepTest[]) {
    if (value) {
      this.stepTests = this._stepTests = value;
    }
  }

  stepTests: any[] = [];

  constructor(private sorterService: SorterService, private personDataService: PersonDataService) { }

  ngOnInit(): void {
  }

  sort(prop: string) {
    this.sorterService.sort(this.stepTests, prop);
  }

  getPersonName(id: number) {
    return new Promise(resolve => {

      //let temp: IGetPersonNameMessage;

      // Get Person
      // Create observer object
      // const personNameGetObserver = {
      //   next: (m: IGetPersonNameMessage) => {
      //     console.log(`Person Name Observer got: ${m.message}`);
      //     temp = m;
      //   },
      //   error: (err: string) => console.error('Person Observer got an error: ' + err),
      //   complete: () => {
      //   }
      // };

      //this.personDataService.getPersonById(id).subscribe(personNameGetObserver);
      this.personDataService.getPersonNameById(id).subscribe((resp: IGetPersonNameMessage) => {
        resolve(`${resp.data.LastName}, ${resp.data.FirstName}`);
      });
    });
  }
}//resolve(`${temp.data.LastName}, ${temp.data.FirstName}`);