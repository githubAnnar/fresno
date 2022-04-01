import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MeasurementDataService } from 'src/app/core';
import { IGetMeasurementMessage, IMeasurement } from 'src/app/shared';

@Component({
  selector: 'app-measurement',
  templateUrl: './measurement.component.html',
  styleUrls: ['./measurement.component.css']
})
export class MeasurementComponent implements OnInit {
  title!: string;
  measurement: IMeasurement = { HeartRate: 0, Id: 0, InCalculation: 0, Lactate: 0, Load: 0, Sequence: 0, StepTestId: 0 };
  getMeasurementMessage!: IGetMeasurementMessage;

  constructor(private measurementDataService: MeasurementDataService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.title = 'Measurement';
    let id: string | null = this.route.snapshot.paramMap.get('id');
    let idSelected = 0;
    if (id) {
      idSelected = +id;
    }

    // Get Measurement
    // Create observer object
    const measurementGetObserver = {
      next: (m: IGetMeasurementMessage) => {
        console.log(`measurementGetObserver got: ${m.message}`);
        this.getMeasurementMessage = m;
      },
      error: (err: string) => console.error('measurementGetObserver got an error: ' + err),
      complete: () => {
        this.measurement = this.getMeasurementMessage.data;
        this.title = `Measurement: ${this.measurement.Id}`;
        console.log(`    const measurementGetObserver = {
          got a complete notification for ${this.title}`);
      }
    };

    this.measurementDataService.getMeasurementById(idSelected).subscribe(measurementGetObserver);
  }
}
