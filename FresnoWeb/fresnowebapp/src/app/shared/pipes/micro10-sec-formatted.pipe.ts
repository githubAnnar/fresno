import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'micro10SecFormatted'
})
export class Micro10SecFormattedPipe implements PipeTransform {

  transform(value: number): string {
    const date = new Date(2000, 1, 1, 0, 0, value / (10000000));
    const timeOptions: Intl.DateTimeFormatOptions = { minute: '2-digit', second: '2-digit' };
    return date.toLocaleTimeString('nb-NO', timeOptions);
  }

}
