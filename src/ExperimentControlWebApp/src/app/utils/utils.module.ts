import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DictionaryIteratorPipe } from './dictionary-iterator.pipe';

@NgModule({
  imports: [CommonModule],
  declarations: [DictionaryIteratorPipe],
  exports: [DictionaryIteratorPipe]
})
export class UtilsModule {}
