import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { OptimicationParameter } from '../../model/models';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-parameter-editor',
  template: `
    <ul class="optimication-parameters">
      <li *ngFor="let param of parameters"><!--| parametertypepipe: 'Fixed'-->
        <div class="parameter-list" (click)="changeParameter(param)">
          {{param.name}}
        </div>
      </li>
    </ul>
  `,
  styles: []
})
export class ParameterEditorComponent implements OnInit {
  @Input()
  parameters: OptimicationParameter[];
  @Output()
  parametersChange = new EventEmitter();

  constructor(private dialog: MatDialog) {}

  ngOnInit() {}

  changeParameter(parameter: OptimicationParameter) {
    /*console.log(parameter);
    const dialogRef = this.dialog.open(ParameterDetailsComponent, {
      width: '40%',
      data: parameter
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (!dialogResult) {
        return;
      }
      const index = this.parameters.indexOf(parameter);
      this.parameters[index] = dialogResult;
      this.parametersChange.emit(this.parameters);
    });*/
  }
}
