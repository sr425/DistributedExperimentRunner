import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dictionaryIterator'
})
export class DictionaryIteratorPipe implements PipeTransform {
  transform(input: any, args?: any): any {
    console.log(input);
    const result = [];
    for (const key in input) {
      if (input.hasOwnProperty(key)) {
        result.push({ key: key, value: input[key] });
      }
    }
    return result;
  }
}
