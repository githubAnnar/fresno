import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementsListComponent } from './measurements-list.component';

describe('MeasurementsListComponent', () => {
  let component: MeasurementsListComponent;
  let fixture: ComponentFixture<MeasurementsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MeasurementsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MeasurementsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
