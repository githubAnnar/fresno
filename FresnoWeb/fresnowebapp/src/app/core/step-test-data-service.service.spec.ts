import { TestBed } from '@angular/core/testing';

import { StepTestDataServiceService } from './step-test-data-service.service';

describe('StepTestDataServiceService', () => {
  let service: StepTestDataServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StepTestDataServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
