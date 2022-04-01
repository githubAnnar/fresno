import { Component, OnInit } from '@angular/core';
import { MeasurementDataService, TokenStorageService } from '../core';
import { IGetMeasurementsMessage, IMeasurement } from '../shared';

@Component({
  selector: 'app-measurements',
  templateUrl: './measurements.component.html',
  styleUrls: ['./measurements.component.css']
})
export class MeasurementsComponent implements OnInit {
  title!: string;
  getMeasurementsMessage!: IGetMeasurementsMessage;
  measurements!: IMeasurement[];
  allowedToAdd = false;

  constructor(private measurementDataService: MeasurementDataService, private tokenService: TokenStorageService) { }

  ngOnInit(): void {
    this.allowedToAdd = this.tokenService.isModeratorOrAdmin();
    this.title = 'Measurements';
    const getMeasurementsObserver = {
      next: (m: IGetMeasurementsMessage) => {
        console.log(`getMeasurementsObserver got ${m.data.length} values: ${m.message}`);
        this.getMeasurementsMessage = m;
      },
      error: (err: string) => console.error(`getMeasurementsObserver got an error: ${err}`),
      complete: () => {
        console.log('getMeasurementsObserver got a complete notification');
        this.measurements = this.getMeasurementsMessage.data;
      }
    };

    this.measurementDataService.getAllMeasurements().subscribe(getMeasurementsObserver);
  }
}
