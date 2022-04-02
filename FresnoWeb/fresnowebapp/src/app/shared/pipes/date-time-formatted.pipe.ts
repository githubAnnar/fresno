import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateTimeFormatted'
})
export class DateTimeFormattedPipe implements PipeTransform {

  transform(value: string): string {
    // Get date
    const date = new Date(value);
    const dateOptions: Intl.DateTimeFormatOptions = { year: 'numeric', month: '2-digit', day: '2-digit', };
    const timeOptions: Intl.DateTimeFormatOptions = { hour: '2-digit', minute: '2-digit', };
    return `${date.toLocaleDateString(undefined, dateOptions)} ${date.toLocaleTimeString(undefined, timeOptions)}`
  }
}
