import { Component, OnInit, Inject, Optional } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-deleteconfirmationdialog',
  template: `
    <div class="create-dialog">
      <div mat-dialog-title>Do you really want to delete this?</div>

      <div *ngIf="expectedText">
        <mat-form-field class="dialog">
          <mat-label>Please write the name {{expectedText}} to delete</mat-label>
          <input matInput [(ngModel)]="text" placeholder="Value to type"/>
        </mat-form-field>
      </div>

      <div mat-dialog-actions>
        <button mat-button (click)="confirm()" type="button" [disabled]="expectedText && text !== expectedText">
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
export class DeleteconfirmationdialogComponent implements OnInit {
  text;
  expectedText;

  constructor(
    public dialogRef: MatDialogRef<DeleteconfirmationdialogComponent>,
    @Optional()
    @Inject(MAT_DIALOG_DATA)
    data?: string
  ) {
    this.expectedText = data;
  }

  ngOnInit() {}

  confirm() {
    if (!this.expectedText || this.expectedText === this.text) {
      this.dialogRef.close({
        text: this.text
      });
    }
  }
}
