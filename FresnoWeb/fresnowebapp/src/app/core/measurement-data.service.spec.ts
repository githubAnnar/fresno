import { TestBed } from '@angular/core/testing';

import { MeasurementDataService } from './measurement-data.service';

describe('MeasurementDataServiceService', () => {
  let service: MeasurementDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MeasurementDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
