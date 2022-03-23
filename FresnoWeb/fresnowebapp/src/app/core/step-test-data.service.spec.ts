import { TestBed } from '@angular/core/testing';

import { StepTestDataService } from './step-test-data.service';

describe('StepTestDataServiceService', () => {
  let service: StepTestDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StepTestDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
