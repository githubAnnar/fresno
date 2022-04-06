import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { SorterService } from 'src/app/core';
import { IStepTestEx } from 'src/app/shared';

@Component({
  selector: 'app-step-tests-list',
  templateUrl: './step-tests-list.component.html',
  styleUrls: ['./step-tests-list.component.css']
})
export class StepTestsListComponent implements OnInit {
  private _stepTests: IStepTestEx[] = [];

  @Input() get listStepTests(): IStepTestEx[] {
    return this._stepTests;
  }

  set listStepTests(value: IStepTestEx[]) {
    if (value) {
      this.stepTests = this._stepTests = value;
    }
  }

  stepTests: IStepTestEx[] = [];
  currentPath!: string;

  constructor(private sorterService: SorterService, private route: ActivatedRoute) {
  }

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

  isOnPerson(): boolean {
    return this.currentPath === 'person';
  }

  sort(prop: string) {
    this.sorterService.sort(this.stepTests, prop);
  }
}