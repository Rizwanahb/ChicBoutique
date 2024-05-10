import { Pipe, PipeTransform } from '@angular/core';
import { Product } from '../_models/product';

@Pipe({
  name: 'sort',
})
export class SortPipe implements PipeTransform {
  transform(value: Product[], criteria: string, direction: string): Product[] {
    let multiplier = 1;

    if (direction === 'desc') {
      multiplier = -1;
    }

    value.sort((a: Product, b: Product) => {
      if (a[criteria] < b[criteria]) {
        return -1 * multiplier;
      } else if (a[criteria] > b[criteria]) {
        return 1 * multiplier;
      } else {
        return 0;
      }
    });

    return value;
  }
}
