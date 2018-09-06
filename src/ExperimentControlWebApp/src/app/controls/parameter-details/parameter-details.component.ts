/*import { Component, OnInit, Inject, Optional } from '@angular/core';
import { OptimicationParameter, FixedParameter } from '../../model/models';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-parameter-details',
  template: `
    <div class="create-dialog">
      <div mat-dialog-title>Create Parameter</div>
      <div mat-dialog-content>
        <mat-form-field class="dialog">
          <input matInput [(ngModel)]="parameter.name" placeholder="Name"/>
        </mat-form-field>
      </div>

      <div mat-dialog-content>
        <mat-form-field>
          <mat-select placeholder="Opt Type of optimication parameter" [(ngModel)]="parameter.optimicationParameterType">
            <mat-option *ngFor="let t of typesList" [value]="t">
              {{t}}
            </mat-option>
          </mat-select>
        </mat-form-field>

        <mat-form-field>
          <mat-select placeholder="Type of parameter value" [(ngModel)]="parameter.parameterValueType">
            <mat-option *ngFor="let t of supportedTypes" [value]="t">
              {{t}}
            </mat-option>
          </mat-select>
        </mat-form-field>
      </div>

      <div mat-dialog-content *ngIf="parameter.optimicationParameterType === 'Set'">
        <ng-container *ngIf="parameter.parameterValueType === 'double'">
          <mat-form-field *ngFor="let value of parameter.values;let i = index">
            <input matInput type="number" [ngModel]="value.doubleValue" (ngModelChange)="setValue(i, $event)" placeholder="Value"/>
          </mat-form-field>
        </ng-container>
        <ng-container *ngIf="parameter.parameterValueType === 'int'">
          <mat-form-field *ngFor="let value of parameter.values;let i = index">
            <input matInput type="number" [ngModel]="value.intValue" (ngModelChange)="setValue(i, $event)" placeholder="Value"/>
          </mat-form-field>
          </ng-container>
        <ng-container *ngIf="parameter.parameterValueType === 'bool'">
          <mat-form-field *ngFor="let value of parameter.values;let i = index">
            <input matInput [ngModel]="value.boolValue" (ngModelChange)="setValue(i, $event)" placeholder="Value"/>
          </mat-form-field>
        </ng-container>
        <ng-container *ngIf="parameter.parameterValueType === 'string'">
          <mat-form-field *ngFor="let value of parameter.values;let i = index">
            <input matInput [ngModel]="value.stringValue" (ngModelChange)="setValue(i, $event)" placeholder="Value"/>
          </mat-form-field>
        </ng-container>
      </div>
      
      <button mat-mini-fab (click)="addValue();$event.stopPropagation()">
        <i class="material-icons">add</i>
      </button>

      <div mat-dialog-content *ngIf="parameter.optimicationParameterType === 'Dynamic'">
        Not yet implemented
      </div>

      <div mat-dialog-actions>
        <button mat-button (click)="confirm()" type="submit">
          OK
        </button>
        <button mat-button (click)="dialogRef.close()" type="button">
          Cancel
        </button>
      </div>
    </div>
`,
  styles: [
    `
      .create-dialog {
        display: flex;
        flex-direction: column;
      }

      .dialog {
        width: 100%;
      }
    `
  ]
})
export class ParameterDetailsComponent {
  parameter: ParameterDetailsViewModel;

  supportedTypes = ['double', 'int', 'string', 'bool'];

  constructor(
    public dialogRef: MatDialogRef<ParameterDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) data?: OptimicationParameter
  ) {
    if (data) {
      this.parameter = <ParameterDetailsViewModel>{ ...data };
    } else {
      this.parameter = new ParameterDetailsViewModel();
      this.parameter.values = [];
    }
  }

  addValue() {
    if (!this.parameter.values) {
      this.parameter.values = [];
    }
    switch (this.parameter.parameterValueType) {
      case 'double':
        this.parameter.values.push({ doubleValue: 0.0 });
        break;
      case 'int':
        this.parameter.values.push({ intValue: 0 });
        break;
      case 'bool':
        this.parameter.values.push({ boolValue: false });
        break;
      default:
        this.parameter.values.push({ stringValue: '' });
        break;
    }
  }

  confirm() {
    if (!this.parameter.name) {
      return;
    }

    /*if (this.parameter.optimicationParameterType === ParameterType.Set) {
      const values = this.parameter.values.map(v => {
        switch (this.parameter.parameterValueType) {
          case 'double':
            return {
              name: this.parameter.name,
              doubleValue: v.doubleValue,
              parameterValueType: this.parameter.parameterValueType
            };
          case 'int':
            return {
              name: this.parameter.name,
              intValue: v.intValue,
              parameterValueType: this.parameter.parameterValueType
            };
          case 'bool':
            return {
              name: this.parameter.name,
              boolValue: v.boolValue,
              parameterValueType: this.parameter.parameterValueType
            };
          default:
            return {
              name: this.parameter.name,
              stringValue: v.stringValue,
              parameterValueType: this.parameter.parameterValueType
            };
        }
      });

      this.dialogRef.close(<SetOptimicationParameter>{
        name: this.parameter.name,
        values: values,
        optimicationParameterType: this.parameter.optimicationParameterType,
        parameterValueType: this.parameter.parameterValueType
      });
  }

  setValue(index: number, value) {
    switch (this.parameter.parameterValueType) {
      case 'double':
        this.parameter.values[index].doubleValue = value;
        break;
      case 'int':
        this.parameter.values[index].intValue = value;
        break;
      case 'bool':
        this.parameter.values[index].boolValue = value;
        break;
      default:
        this.parameter.values[index].stringValue = value;
        break;
    }
  }
}
*/
