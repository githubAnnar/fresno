import { Component, OnInit } from '@angular/core';
import { PersonDataService, TokenStorageService } from '../core';
import { IGetPersonsMessage, IPerson } from '../shared';

@Component({
  selector: 'app-persons',
  templateUrl: './persons.component.html',
  styleUrls: ['./persons.component.css']
})
export class PersonsComponent implements OnInit {
  title!: string;
  getPersonsMessage!: IGetPersonsMessage;
  persons!: IPerson[];
  allowedToAdd = false;

  constructor(private personDataService: PersonDataService, private tokenService: TokenStorageService) { }

  ngOnInit(): void {
    this.allowedToAdd = this.tokenService.isModeratorOrAdmin();
    this.title = 'Persons';

    const getPersonsObserver = {
      next: (m: IGetPersonsMessage) => {
        console.log(`getPersonsObserver got ${m.data.length} values: ${m.message}`);
        this.getPersonsMessage = m;
      },
      error: (err: string) => console.error(`getPersonsObserver got an error: ${err}`),
      complete: () => {
        console.log('getPersonsObserver got a complete notification');
        this.persons = this.getPersonsMessage.data;
      }
    };

    this.personDataService.getAllPersons().subscribe(getPersonsObserver);
  }
}
