import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ExperimentPart, FixedParameter } from '../../model/models';
import { ExperimentApiService } from '../experiment-api.service';
import { MatTableDataSource, MatTable } from '@angular/material';

type ParameterTypes = 'double' | 'int' | 'string' | 'bool';

@Component({
  selector: 'app-fixed-parameter-editor',
  template: `
    <mat-card>
      <mat-table [dataSource]="parameters">
        <ng-container matColumnDef="name">
          <mat-header-cell *matHeaderCellDef> Name </mat-header-cell>
          <mat-cell *matCellDef="let parameter">
            <input matInput [(ngModel)]="parameter.name" placeholder="Name of the parameter"/>
          </mat-cell>
          <mat-footer-cell *matFooterCellDef>
            <mat-form-field>
              <mat-label>
                Parameter Name
              </mat-label>
              <input matInput [(ngModel)]="fixedParameterName" placeholder="Name of the parameter"/>
            </mat-form-field>
          </mat-footer-cell>
        </ng-container>

        <ng-container matColumnDef="type">
          <mat-header-cell *matHeaderCellDef> Type </mat-header-cell>
          <mat-cell *matCellDef="let parameter">
            <mat-select placeholder="Type of parameter" [(ngModel)]="parameter.parameterValueType">
              <mat-option *ngFor="let type of parameterTypes" [value]="type">
                {{type}}
              </mat-option>
            </mat-select>
          </mat-cell>
          <mat-footer-cell *matFooterCellDef>
            <mat-form-field>
              <mat-select placeholder="Type of parameter" [(ngModel)]="parameterType">
                <mat-option *ngFor="let type of parameterTypes" [value]="type">
                  {{type}}
                </mat-option>
              </mat-select>
            </mat-form-field>
          </mat-footer-cell>
        </ng-container>

        <ng-container matColumnDef="value">
          <mat-header-cell *matHeaderCellDef> Value </mat-header-cell>
          <mat-cell *matCellDef="let parameter">
            <mat-checkbox *ngIf="parameter.parameterValueType == 'bool'" [(ngModel)]="parameter.boolValue">Value</mat-checkbox>
            <input *ngIf="parameter.parameterValueType == 'double'" type="number" matInput [(ngModel)]="parameter.doubleValue"/>
            <input *ngIf="parameter.parameterValueType == 'int'" type="number" matInput [(ngModel)]="parameter.intValue"/>
            <input *ngIf="parameter.parameterValueType == 'string'" type="text" matInput [(ngModel)]="parameter.stringValue"/>
          </mat-cell>
          <mat-footer-cell *matFooterCellDef>
            <mat-form-field *ngIf="parameterType != 'bool'">
              <mat-label>
                Parameter Value
              </mat-label>
              <input  [type]="inputFieldType()"
                matInput [(ngModel)]="fixedParameterValue" placeholder="Parameter value"/>
            </mat-form-field>
            <mat-checkbox *ngIf="parameterType == 'bool'" [(ngModel)]="fixedParameterChecked">Value</mat-checkbox>
          </mat-footer-cell>
        </ng-container>

        <ng-container matColumnDef="actions">
          <mat-header-cell *matHeaderCellDef class="action-table-column">
            <button  type="button" mat-mini-fab (click)="saveUpdate();$event.stopPropagation()">
              <i class="material-icons">save</i>
            </button>
          </mat-header-cell>
          <mat-cell *matCellDef="let parameter" class="action-table-column">
            <button  type="button" mat-mini-fab (click)="deleteFixedParameter(parameter);$event.stopPropagation()">
              <i class="material-icons">delete</i>
            </button>
          </mat-cell>
          <mat-footer-cell *matFooterCellDef class="action-table-column">
            <button type="button" mat-mini-fab (click)="addParameter();$event.stopPropagation()">
              <i class="material-icons">add</i>
            </button>
          </mat-footer-cell>
        </ng-container>

        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
        <mat-footer-row *matFooterRowDef="displayedColumns"></mat-footer-row>
      </mat-table>
    </mat-card>
  `,
  styles: [
    `
      .action-table-column {
        display: flex;
        justify-content: flex-end;
      }

      .add-parameter {
        display: flex;
        justify-content: space-between;
      }

      .fixed-parameter-list {
        display: flex;
        justify-content: space-between;
      }

      .fixed-parameter-table {
        width: 100%;
      }
    `
  ]
})
export class FixedParameterEditorComponent {
  params: FixedParameter[];
  parameters = new MatTableDataSource();

  @Input()
  set fixedParameters(params: FixedParameter[]) {
    this.params = params;
    this.parameters.data = params;
  }

  @Output()
  parameterChanged = new EventEmitter<FixedParameter[]>();

  displayedColumns: string[] = ['name', 'type', 'value', 'actions'];

  public parameterTypes = ['double', 'int', 'string', 'bool'];

  parameterType: ParameterTypes = 'double';

  fixedParameterName;
  fixedParameterValue;
  fixedParameterChecked;

  constructor(private experimentApi: ExperimentApiService) {}

  addParameter() {
    if (!this.fixedParameterName || !this.parameterType) {
      return;
    }
    if (this.parameterType === 'bool' ? false : !this.fixedParameterValue) {
      return;
    }

    const newFixedParam: any = {
      name: this.fixedParameterName,
      parameterValueType: this.parameterType
    };

    if (this.parameterType === 'double') {
      newFixedParam.doubleValue = <number>this.fixedParameterValue;
    } else if (this.parameterType === 'int') {
      if (
        Number(Math.round(this.fixedParameterValue)) !==
        Number(this.fixedParameterValue)
      ) {
        return;
      }
      newFixedParam.intValue = <number>this.fixedParameterValue;
    } else if (this.parameterType === 'bool') {
      if (!this.fixedParameterChecked) {
        newFixedParam.boolValue = false;
      } else {
        newFixedParam.boolValue = true;
      }
    } else {
      newFixedParam.stringValue = <string>this.fixedParameterValue;
    }

    if (!this.params) {
      this.params = [];
    }

    this.params.push(<FixedParameter>newFixedParam);

    this.fixedParameterName = undefined;
    this.fixedParameterValue = undefined;
    this.parameterType = 'double';
    this.fixedParameterChecked = false;
    this.parameterChanged.emit(this.params);
    this.parameters.data = this.params;
  }

  deleteFixedParameter(parameter: FixedParameter) {
    const index: number = this.params.indexOf(parameter, 0);
    if (index > -1) {
      this.params.splice(index, 1);
    }
    this.parameterChanged.emit(this.params);
    this.parameters.data = this.params;
  }

  getInvalidProperties(type: string): string[] {
    if (type === 'bool') {
      return ['stringValue', 'intValue', 'doubleValue'];
    } else if (type === 'string') {
      return ['boolValue', 'intValue', 'doubleValue'];
    } else if (type === 'int') {
      return ['stringValue', 'boolValue', 'doubleValue'];
    } else {
      return ['stringValue', 'intValue', 'boolValue'];
    }
  }

  saveUpdate() {
    this.parameterChanged.emit(this.params);
  }

  inputFieldType() {
    return this.parameterType === 'string' ? 'text' : 'number';
  }
}
