import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { MeasurementDataService, PersonDataService, StepTestDataService } from 'src/app/core';
import { IGetMeasurementsMessage, IGetStepTestMessage, IMeasurementEx, IStepTest } from 'src/app/shared';

@Component({
  selector: 'app-step-test',
  templateUrl: './step-test.component.html',
  styleUrls: ['./step-test.component.css']
})
export class StepTestComponent implements OnInit {
  title!: string;
  stepTest: IStepTest = { EffortUnit: '', Id: 0, Increase: 0, LoadPreset: 0, PersonId: 0, StepDuration: 0, Temperature: 0, TestDate: '', TestType: '', Weight: 0 };
  getStepTestMessage!: IGetStepTestMessage;

  measurements!: IMeasurementEx[];
  getMeasurementsMessage!: IGetMeasurementsMessage;

  testName!: Observable<string>;

  constructor(private stepTestDataService: StepTestDataService, private measurementDataService: MeasurementDataService, private personDataService: PersonDataService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.title = 'Step Test';
    let id: string | null = this.route.snapshot.paramMap.get('id');
    let idSelected = 0;
    if (id) {
      idSelected = +id;
    }

    // Get StepTest
    // Create observer object
    const stepTestGetObserver = {
      next: (m: IGetStepTestMessage) => {
        console.log(`StepTest Observer got: ${m.message}`);
        this.getStepTestMessage = m;
      },
      error: (err: string) => console.error('StepTest Observer got an error: ' + err),
      complete: () => {
        this.stepTest = this.getStepTestMessage.data;
        this.title = `Step Test: ${this.stepTest.TestDate}`;
        console.log(`StepTest Observer got a complete notification for ${this.title}`);
        this.testName = this.personDataService.getPersonNameByIdEx(this.stepTest.PersonId);
      }
    };

    this.stepTestDataService.getStepTestById(idSelected).subscribe(stepTestGetObserver);

    // Get Measurements
    // Get Measurements observer
    const measurementsGetObserver = {
      next: (m: IGetMeasurementsMessage) => {
        console.log(`Measurements Observer got: ${m.message}`);
        this.getMeasurementsMessage = m;
      },
      error: (err: string) => { console.error(`Measurements Observer got an error: ${err}`) },
      complete: () => {
        this.measurements = this.getMeasurementsMessage.data.map(m => ({ FirstName: '', HeartRate: m.HeartRate, Id: m.Id, InCalculation: m.InCalculation, Lactate: m.Lactate, LastName: '', Load: m.Load, Sequence: m.Sequence, StepTestId: m.StepTestId, TestDate: '' }));
        console.log(`Measurements Observer got a complete notification for ${this.measurements.length} rows(s)`);
      }
    }

    this.measurementDataService.getAllMeasurementsByStepTestId(idSelected).subscribe(measurementsGetObserver);
  }
}
