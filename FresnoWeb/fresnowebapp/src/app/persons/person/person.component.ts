import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PersonDataService, StepTestDataService } from 'src/app/core';
import { IGetPersonMessage, IGetStepTestsMessage, IPerson, IStepTest, IStepTestEx } from 'src/app/shared';

@Component({
  selector: 'app-person',
  templateUrl: './person.component.html',
  styleUrls: ['./person.component.css']
})
export class PersonComponent implements OnInit {
  title!: string;
  person: IPerson = { Id: 0, FirstName: '', LastName: '', BirthDate: '', Email: '', Height: 0, MaxHr: 0, PostCity: '', PostCode: '', Sex: '', Street: '' };
  getPersonMessage!: IGetPersonMessage;

  stepTests!: IStepTestEx[];
  getStepTestsMessage!: IGetStepTestsMessage;

  constructor(private personDataService: PersonDataService, private stepTestDataService: StepTestDataService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.title = 'Person';
    let id: string | null = this.route.snapshot.paramMap.get('id');
    let idSelected = 0;
    if (id) {
      idSelected = +id;
    }

    // Get Person
    // Create observer object
    const personGetObserver = {
      next: (m: IGetPersonMessage) => {
        console.log(`Person Observer got: ${m.message}`);
        this.getPersonMessage = m;
      },
      error: (err: string) => console.error('Person Observer got an error: ' + err),
      complete: () => {
        this.person = this.getPersonMessage.data;
        this.title = `Person: ${this.person.LastName}, ${this.person.FirstName}`;
        console.log(`Person Observer got a complete notification for ${this.title}`);
      }
    };

    this.personDataService.getPersonById(idSelected).subscribe(personGetObserver);

    // Get StepTests
    // Get StepTests observer
    const stepTestsGetObserver = {
      next: (m: IGetStepTestsMessage) => {
        console.log(`StepTests Observer got: ${m.message}`);
        this.getStepTestsMessage = m;
      },
      error: (err: string) => { console.error(`StepTest Observer got an error: ${err}`) },
      complete: () => {
        this.stepTests = this.getStepTestsMessage.data.map(s => ({ Id: s.Id, EffortUnit: s.EffortUnit, FirstName: '', Increase: s.Increase, LastName: '', LoadPreset: s.LoadPreset, PersonId: s.PersonId, StepDuration: s.StepDuration, Temperature: s.Temperature, TestDate: s.TestDate, TestType: s.TestType, Weight: s.Weight }));
        console.log(`StepTest Observer got a complete notification for ${this.stepTests.length} rows(s)`);
      }
    }

    this.stepTestDataService.getAllStepTestsByPersonId(idSelected).subscribe(stepTestsGetObserver);
  }
}