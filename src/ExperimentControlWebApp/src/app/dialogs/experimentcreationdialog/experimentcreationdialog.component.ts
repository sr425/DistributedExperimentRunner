import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-experimentcreationdialog',
  template: `
    <div class="create-dialog">
      <div mat-dialog-title>Create Experiment</div>
      <div mat-dialog-content>
        <mat-form-field class="dialog">
          <input matInput [(ngModel)]="name" placeholder="Name"/>
        </mat-form-field>
      </div>

      <div mat-dialog-content>
        <mat-form-field class="dialog">
          <textarea matInput [(ngModel)]="description"
          cdkTextareaAutosize
          #autosize="cdkTextareaAutosize"
          placeholder="Description"></textarea>
        </mat-form-field>
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
      width: 100%
    }
    `
  ]
})
export class ExperimentcreationdialogComponent implements OnInit {
  name;
  description;

  constructor(public dialogRef: MatDialogRef<ExperimentcreationdialogComponent>) {
  }

  ngOnInit() {
  }


  confirm() {
    this.dialogRef.close({
      'name': this.name,
      'description': this.description
    });
  }
}
