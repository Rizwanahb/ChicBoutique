// filterByUserId.pipe.ts
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterByUserId'
})
export class FilterByUserIdPipe implements PipeTransform {
  transform(orders: any[], userId: number): any[] {
    return orders.filter(order => order.userId === userId);
  }
}
