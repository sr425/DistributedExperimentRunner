import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExperimentPartViewComponent } from './experiment-part-view/experiment-part-view.component';
import { TaskViewComponent } from './task-view/task-view.component';
import { TaskSetViewComponent } from './task-set-view/task-set-view.component';
import { ExperimentViewComponent } from './experiment-view/experiment-view.component';
import { MaterialModule } from '../material.module';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { FixedParameterEditorComponent } from './fixed-parameter-editor/fixed-parameter-editor.component';
import { ParameterValueInputComponent } from './parameter-value-input/parameter-value-input.component';
import { UtilsModule } from '../utils/utils.module';
import { NgxDatatableModule } from '@swimlane/ngx-datatable';
import { CovalentFileModule } from '@covalent/core/file';

@NgModule({
  imports: [
    CommonModule,
    MaterialModule,
    HttpClientModule,
    FormsModule,
    UtilsModule,
    NgxDatatableModule,
    CovalentFileModule
  ],
  declarations: [
    ExperimentPartViewComponent,
    TaskViewComponent,
    TaskSetViewComponent,
    ExperimentViewComponent,
    FixedParameterEditorComponent,
    ParameterValueInputComponent
  ],
  exports: [
    ExperimentPartViewComponent,
    TaskSetViewComponent,
    TaskViewComponent,
    ExperimentViewComponent
  ]
})
export class ExperimentModule {}
