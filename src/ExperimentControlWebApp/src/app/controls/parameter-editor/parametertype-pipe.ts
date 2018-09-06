import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'parametertypepipe',
  pure: false
})
export class ParameterTypePipe implements PipeTransform {
  transform(items: any[], filter: Object): any {
    if (!items || !filter) {
      return items;
    }
    // filter items array, items which match and return true will be
    // kept, false will be filtered out
    return (
      items && items.filter(item => item.optimicationParameterType === filter)
    );
  }
}
