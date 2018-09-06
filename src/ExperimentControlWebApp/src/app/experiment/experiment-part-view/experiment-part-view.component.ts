import { Component, OnInit, Input } from '@angular/core';
import { ExperimentPart, Dataset, FixedParameter } from '../../model/models';
import { MatDialog } from '@angular/material';
import { StringInputDialogComponent } from '../../dialogs/string-input-dialog/string-input-dialog.component';
import { ExperimentApiService } from '../experiment-api.service';
import { DatasetListService } from '../../dataset-list.service';

@Component({
  selector: 'app-experiment-part-view',
  template: `
    <mat-card class="editable-element-div">
      <p>
        {{experimentPart.name}}
      </p>
      <button type="button" mat-mini-fab (click)="editName(experimentPart);$event.stopPropagation()">
        <i class="material-icons">edit</i>
      </button>
      <button type="button" mat-mini-fab (click)="deletePart(experimentPart);$event.stopPropagation()">
      <i class="material-icons">delete</i>
    </button>
    </mat-card>

    <mat-card>
      <mat-list>
        <div *ngFor="let input of experimentPart.inputDatasets">
          <h4>{{input.id}} - {{input.name}}</h4>
        </div>
      </mat-list>

      <mat-form-field>
        <mat-select  placeholder="Datasets to add"
          [ngModel]="selectedDataset"
          (ngModelChange)="addCurrentSelection($event, experimentPart)">
          <mat-option mat-option-empty value="{{null}}">None</mat-option>
          <mat-option *ngFor="let dataset of (datasetList.datasets$ | async) as datasets" [value]="dataset">
            {{dataset.id}} - {{dataset.name}}
          </mat-option>
        </mat-select>
      </mat-form-field>
    </mat-card>

    <div *ngIf="experimentPart">
      <app-fixed-parameter-editor [fixedParameters]="experimentPart.fixedParameters" (parameterChanged)="parametersUpdate($event)"></app-fixed-parameter-editor>
    </div>

    <h3 *ngIf="experimentPart.failed">
      Part failed
    </h3>

    <mat-card *ngIf="experimentPart.aggregatedValues">
      <h3>Results<h3>
      <ngx-datatable class="material" 
        	[rows]="experimentPart.aggregatedValues|dictionaryIterator"
          [columns] = "columns"
          [rowHeight]="'auto'" 
          [columnMode]="'force'">
      </ngx-datatable>
    <mat-card>
  `,
  styles: [
    `
      .editable-element-div {
        display: flex;
        justify-content: space-between;
      }
    `
  ]
})
export class ExperimentPartViewComponent {
  @Input()
  experimentPart: ExperimentPart;

  columns = [
    { prop: 'key', name: 'Metric' },
    { prop: 'value', name: 'Result' }
  ];

  public selectedDataset: Dataset;

  constructor(
    private dialog: MatDialog,
    private experimentApi: ExperimentApiService,
    public datasetList: DatasetListService
  ) {}

  editName(experimentPart: ExperimentPart) {
    const dialogRef = this.dialog.open(StringInputDialogComponent, {
      width: '40%',
      data: 'New name of the experiment part'
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (!dialogResult) {
        return;
      }
      experimentPart.name = dialogResult;
      this.experimentApi
        .updateExperimentPart(experimentPart.experimentId, experimentPart)
        .subscribe(() => {});
    });
  }

  addCurrentSelection(dataset: Dataset, part: ExperimentPart) {
    if (!dataset) {
      return;
    }
    if (!part.inputDatasets) {
      part.inputDatasets = [];
    }

    if (part.inputDatasets.map(d => d.id).indexOf(dataset.id) === -1) {
      part.inputDatasets.push(dataset);
    }
    this.selectedDataset = null;
    this.experimentApi
      .updateExperimentPart(part.experimentId, part)
      .subscribe(() => {});
  }

  deletePart(part: ExperimentPart) {
    this.experimentApi
      .deleteExperimentPart(part.experimentId, part.id)
      .subscribe(() => {});
  }

  parametersUpdate(params: FixedParameter[]) {
    if (!params) {
      return;
    }
    this.experimentPart.fixedParameters = params;
    this.experimentApi
      .updateExperimentPart(
        this.experimentPart.experimentId,
        this.experimentPart
      )
      .subscribe(() => {});
  }
}
