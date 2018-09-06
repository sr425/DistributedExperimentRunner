import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SingleFolderComponent } from './single-folder/single-folder.component';
import { MaterialModule } from '../material.module';

@NgModule({
  imports: [CommonModule, MaterialModule],
  declarations: [SingleFolderComponent]
})
export class FolderModule {}
