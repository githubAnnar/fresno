import { Component, Input, OnInit } from '@angular/core';
import { SorterService } from 'src/app/core';
import { IPerson } from 'src/app/shared';

@Component({
  selector: 'app-persons-list',
  templateUrl: './persons-list.component.html',
  styleUrls: ['./persons-list.component.css']
})
export class PersonsListComponent implements OnInit {
  private _persons: IPerson[] = [];

  @Input() get listPersons(): IPerson[] {
    return this._persons;
  }

  set listPersons(value: IPerson[]) {
    if (value) {
      this.persons = this._persons = value;
    }
  }

  persons: any[] = [];

  constructor(private sorterService: SorterService) { }

  ngOnInit(): void {
  }

  sort(prop: string) {
    this.sorterService.sort(this.persons, prop);
  }
}
