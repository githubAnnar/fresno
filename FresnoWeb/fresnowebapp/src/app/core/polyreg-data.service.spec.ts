import { TestBed } from '@angular/core/testing';

import { PolyregDataService } from './polyreg-data.service';

describe('PolyregDataService', () => {
  let service: PolyregDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PolyregDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
