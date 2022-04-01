import { Component, OnInit } from '@angular/core';
import { StepTestDataService, TokenStorageService } from '../core';
import { IGetStepTestsMessage, IStepTest } from '../shared';

@Component({
  selector: 'app-step-tests',
  templateUrl: './step-tests.component.html',
  styleUrls: ['./step-tests.component.css']
})
export class StepTestsComponent implements OnInit {
  title!: string;
  getStepTestsMessage!: IGetStepTestsMessage;
  stepTests!: IStepTest[];
  allowedToAdd = false;

  constructor(private stepTestDataService: StepTestDataService, private tokenService: TokenStorageService) { }

  ngOnInit(): void {
    this.allowedToAdd = this.tokenService.isModeratorOrAdmin();
    this.title = 'Step Tests';
    const getStepTestObserver = {
      next: (m: IGetStepTestsMessage) => {
        console.log(`getStepTestObserver got ${m.data.length} values: ${m.message}`);
        this.getStepTestsMessage = m;
      },
      error: (err: string) => console.error(`getStepTestObserver got an error: ${err}`),
      complete: () => {
        console.log('getStepTestObserver got a complete notification');
        this.stepTests = this.getStepTestsMessage.data;
      }
    };

    this.stepTestDataService.getAllStepTests().subscribe(getStepTestObserver);
  }
}
