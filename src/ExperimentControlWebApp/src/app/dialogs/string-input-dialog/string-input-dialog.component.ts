import { Component, OnInit, Optional, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-string-input-dialog',
  template: `
  <div class="create-dialog">
  <div mat-dialog-title>{{heading}}</div>

  <mat-form-field class="dialog">
    <mat-label>Text</mat-label>
    <input matInput [(ngModel)]="text" placeholder="Value to type"/>
  </mat-form-field>

  <div mat-dialog-actions>
    <button mat-button (click)="confirm()" type="button">
      OK
    </button>
    <button mat-button (click)="dialogRef.close()" type="submit">
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
export class StringInputDialogComponent implements OnInit {
  text;
  heading;

  constructor(
    public dialogRef: MatDialogRef<StringInputDialogComponent>,
    @Optional()
    @Inject(MAT_DIALOG_DATA)
    data?: string
  ) {
    this.heading = data;
  }

  ngOnInit() {}

  confirm() {
    this.dialogRef.close(this.text);
  }
}
