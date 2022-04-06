import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { SorterService } from 'src/app/core';
import { IMeasurementEx } from 'src/app/shared';

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
    }
  }

  measurements: IMeasurementEx[] = [];
  currentPath!: string;

  constructor(private sorterService: SorterService, private route: ActivatedRoute) { }

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
