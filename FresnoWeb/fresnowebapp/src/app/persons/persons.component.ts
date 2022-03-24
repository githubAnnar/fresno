import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-persons',
  templateUrl: './persons.component.html',
  styleUrls: ['./persons.component.css']
})
export class PersonsComponent implements OnInit {
  title!: string;

  constructor() { }

  ngOnInit(): void {
    this.title = 'Persons';
  }

}
