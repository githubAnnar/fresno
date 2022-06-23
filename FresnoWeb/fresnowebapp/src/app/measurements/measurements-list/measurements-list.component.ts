import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { SorterService } from 'src/app/core';
import { HelpersModule, IMeasurementEx } from 'src/app/shared';

@Component({
  selector: 'app-measurements-list',
  templateUrl: './measurements-list.component.html',
  styleUrls: ['./measurements-list.component.css']
})
export class MeasurementsListComponent implements OnInit {
  private _measurements: IMeasurementEx[] = [];

  @Input() get listMeasurements(): IMeasurementEx[] {
    return this._measurements;
  }

  set listMeasurements(value: IMeasurementEx[]) {
    if (value) {
      this.measurements = this._measurements = value;
      console.log(this.measurements);
      console.log('ok', JSON.stringify(this.helpers.getLactateArray(this.measurements)));
    }
  }

  measurements: IMeasurementEx[] = [];
  currentPath!: string;
  helpers: HelpersModule = new HelpersModule();

  constructor(private sorterService: SorterService, private route: ActivatedRoute) {
    this.helpers = new HelpersModule();
  }

  ngOnInit(): void {
    const urlObserver = {
      next: (data: UrlSegment[]) => {
        this.currentPath = data[0].path;
      },
      error: (err: string) => console.error('Url Observer got an error: ' + err),
      complete: () => { }

    };

    this.route.url.subscribe(urlObserver);
  }

  isOnStepTest(): boolean {
    return this.currentPath === 'steptest';
  }

  sort(prop: string) {
    this.sorterService.sort(this.measurements, prop);
  }
}
