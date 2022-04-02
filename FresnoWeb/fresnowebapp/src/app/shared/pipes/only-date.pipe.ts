import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'onlyDate'
})
export class OnlyDatePipe implements PipeTransform {

  transform(value: string): string {
    // Convert string to date
    const date = new Date(value);
    const dateOptions: Intl.DateTimeFormatOptions = { year: 'numeric', month: '2-digit', day: '2-digit' };
    return date.toLocaleDateString(undefined, dateOptions);
  }
}
