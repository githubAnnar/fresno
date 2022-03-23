import { TestBed } from '@angular/core/testing';

import { MeasurementDataServiceService } from './measurement-data-service.service';

describe('MeasurementDataServiceService', () => {
  let service: MeasurementDataServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MeasurementDataServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
