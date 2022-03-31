import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';
import { PersonDataService, SorterService } from 'src/app/core';
import { IGetPersonNameMessage, IStepTest } from 'src/app/shared';

@Component({
  selector: 'app-step-tests-list',
  templateUrl: './step-tests-list.component.html',
  styleUrls: ['./step-tests-list.component.css']
})
export class StepTestsListComponent implements OnInit {
  private _stepTests: IStepTest[] = [];

  @Input() get listStepTests(): IStepTest[] {
    return this._stepTests;
  }

  set listStepTests(value: IStepTest[]) {
    if (value) {
      this.stepTests = this._stepTests = value;
    }
  }

  stepTests: any[] = [];
  currentPath!: string;

  constructor(private sorterService: SorterService, private personDataService: PersonDataService, private route: ActivatedRoute) {
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