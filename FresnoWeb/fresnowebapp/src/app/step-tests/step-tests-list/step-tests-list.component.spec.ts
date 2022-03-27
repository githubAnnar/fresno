import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StepTestsListComponent } from './step-tests-list.component';

describe('StepTestsListComponent', () => {
  let component: StepTestsListComponent;
  let fixture: ComponentFixture<StepTestsListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StepTestsListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StepTestsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
