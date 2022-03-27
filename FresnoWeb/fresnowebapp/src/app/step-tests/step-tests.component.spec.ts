import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepTestsComponent } from './step-tests.component';

describe('StepTestsComponent', () => {
  let component: StepTestsComponent;
  let fixture: ComponentFixture<StepTestsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StepTestsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StepTestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
