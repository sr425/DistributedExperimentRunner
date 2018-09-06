import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-single-folder',
  template: `
    <mat-accordion>
      <mat-expansion-panel>
        <mat-expansion-panel-header>
          <mat-panel-title>
          </mat-panel-title>
        </mat-expansion-panel-header>
      </mat-expansion-panel>
    </mat-accordion>
  `,
  styles: []
})
export class SingleFolderComponent implements OnInit {
  constructor() {}

  ngOnInit() {}
}
