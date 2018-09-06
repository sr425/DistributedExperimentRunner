import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Dataset } from '../../model/dataset';

@Component({
  selector: 'app-input-parameter-dialog',
  template: `
    <div class="create-dialog">
      <div mat-dialog-title>Create Input Parameter</div>
      <div mat-dialog-content>
        <mat-form-field class="dialog">
          <input matInput [(ngModel)]="inputParameter.inputName" placeholder="Name"/>
        </mat-form-field>
      </div>

      <mat-list class="inut-parameter-data-list">
        <mat-list-item *ngFor="let input of inputParameter.datasetIds">
          {{input}}
        </mat-list-item>
      </mat-list>

      <mat-form-field>
        <input matInput type="number" [(ngModel)]="newValue" >
        <button mat-button *ngIf="newValue" matSuffix mat-icon-button aria-label="Add" (click)="addValue()">
          <mat-icon>add</mat-icon>
        </button>
      </mat-form-field>

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
export class InputParameterDialogComponent implements OnInit {
  inputParameter: Dataset;
  newValue: number;

  constructor(
    public dialogRef: MatDialogRef<InputParameterDialogComponent>,
    @Inject(MAT_DIALOG_DATA) data?: Dataset
  ) {
    if (data) {
      this.inputParameter = <Dataset>{ ...data };
    } else {
      this.inputParameter = <Dataset>{ name: '', description: '' };
    }
  }

  ngOnInit() {}
  /*
  addValue() {
    this.inputParameter.datasetIds.push(this.newValue);
    this.newValue = undefined;
  }

  confirm() {
    if (!this.inputParameter.inputName) {
      return;
    }
    this.dialogRef.close(<DatasetInput>this.inputParameter);
  }*/
}
