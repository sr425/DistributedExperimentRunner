import { Component, OnInit } from '@angular/core';
import { Observable, Subject, merge } from 'rxjs';
import {
  Dataset,
  Experiment,
  ExperimentPart,
  FixedParameter
} from '../../model/models';
import { ExperimentcontrollerService } from '../../experimentcontroller.service';
import { ActivatedRoute } from '@angular/router';
import { ExperimentstateService } from '../../experimentstate.service';
import { map, switchMap, startWith, combineLatest, tap } from 'rxjs/operators';
import { ExperimentApiService } from '../experiment-api.service';
import { MatDialog } from '@angular/material';
import { StringInputDialogComponent } from '../../dialogs/string-input-dialog/string-input-dialog.component';

@Component({
  selector: 'app-experiment-view',
  template: `
  <ng-container *ngIf="experiment$ | async as experiment">

  <h1 class="headline">Experiment {{experiment.id}}: {{experiment?.name}} </h1>

  <mat-card>
    <div class="name-save-div">
      <mat-form-field class="name-field">
        <mat-label>Name</mat-label>
        <input matInput [(ngModel)]="experiment.name" />
      </mat-form-field>

      <button mat-mini-fab (click)="saveChanges(experiment)"><i class="material-icons">save</i></button>
    </div>
    <div>
      <mat-form-field class="description-field">
        <mat-label>Description</mat-label>
        <textarea matInput [(ngModel)]="experiment.description"
            cdkTextareaAutosize
            #autosize="cdkTextareaAutosize"></textarea>
      </mat-form-field>
    </div>
  </mat-card>

  <mat-card>
    <h2 class="title"> Payload</h2>

    <div *ngIf="experiment.payloadFilename" class="body-1">{{experiment.payloadFilename}}</div>
    <div *ngIf="experiment.payloadHash" class="body-1">{{experiment.payloadHash}}</div>

    <td-file-upload #fileUpload [(ngModel)]="payloadFile" defaultColor="accent" activeColor="warn" cancelColor="primary"
      (upload)="uploadPayload($event, experiment)"  accept=".zip" required>
      <mat-icon>file_upload</mat-icon><span>{{ payloadFile?.name }}</span>
      <ng-template td-file-input-label>
        <mat-icon>attach_file</mat-icon>
        <span>
          Choose a file...
        </span>
      </ng-template>
    </td-file-upload>
  </mat-card>

  <mat-card>
    <h2>Shared Parameters</h2>
    <div *ngIf="experiment">
      <app-fixed-parameter-editor [fixedParameters]="experiment.sharedFixedParameter"
                                  (parameterChanged)="parametersUpdate($event, experiment)">
      </app-fixed-parameter-editor>
    </div>
  </mat-card>

  <mat-card>
    <div class="add-part-div">
      <button type="button" mat-mini-fab (click)="addPart(experiment);$event.stopPropagation()">
        <i class="material-icons">add</i>
      </button>
    </div>
    <ng-container *ngIf="experimentParts$ | async as parts" style="justify-content: space-between;align-items: end;">
      <div *ngFor="let part of parts">
        <mat-expansion-panel>
          <mat-expansion-panel-header class="space-between-expansion-panel-header">
            <mat-panel-title>
              <h2>{{part.name}}</h2>
            </mat-panel-title>
            <div></div>
            <mat-panel-description class="right-align-panel-description">
              <button *ngIf="!part.running" type="button" mat-mini-fab (click)="startExperimentPart(part);$event.stopPropagation()">
                <i class="material-icons">play_arrow</i>
              </button>
              <button *ngIf="part.running" type="button" mat-mini-fab (click)="stopExperimentPart(part);$event.stopPropagation()">
                <i class="material-icons">stop</i>
              </button>
            </mat-panel-description>
          </mat-expansion-panel-header>
          <app-experiment-part-view [experimentPart]="part"></app-experiment-part-view>
        </mat-expansion-panel>
      </div>
    </ng-container>
  </mat-card>
</ng-container>
`,
  styles: [
    `
      .add-part-div {
        display: flex;
        justify-content: center;
      }

      .description-field {
        width: 75%;
      }
      .name-field {
        width: 25%;
      }

      .space-between-expansion-panel-header {
        justify-content: space-between;
      }

      .right-align-panel-description {
        flex: 0 0 auto;
      }

      .name-save-div {
        justify-content: space-between;
        align-content: space-between;
        align: center;
      }

      .benchmark-list-item {
        float: right;
      }
    `
  ]
})
export class ExperimentViewComponent {
  experimentId$: Observable<number>;
  experiment$: Observable<Experiment>;
  experimentParts$: Observable<ExperimentPart[]>;
  datasets$: Observable<Dataset[]>;

  refreshParts$ = new Subject<{}>();
  refreshExperiment$ = new Subject<{}>();

  displayedColumns: string[] = ['Id', 'Name', 'Actions'];

  payloadFile: File;

  constructor(
    private experimentController: ExperimentcontrollerService,
    private experimentApi: ExperimentApiService,
    private dialog: MatDialog,
    route: ActivatedRoute,
    public experimentStateService: ExperimentstateService
  ) {
    this.experimentId$ = route.paramMap.pipe(
      map(params => Number(params.get('id')))
    );

    this.experimentParts$ = this.experimentId$.pipe(
      combineLatest(this.refreshParts$.pipe(startWith({})), id => id),
      switchMap(id => experimentApi.getExperimentParts(id)),
      tap(d => console.log(d))
    );

    this.experiment$ = this.experimentId$.pipe(
      combineLatest(this.refreshExperiment$.pipe(startWith({})), id => id),
      switchMap(id => experimentController.getExperiment(id)),
      tap(console.log)
    );
  }

  async saveChanges(experiment) {
    await this.experimentController.updateExperiment(experiment);
  }

  addPart(experiment: Experiment) {
    const dialogRef = this.dialog.open(StringInputDialogComponent, {
      width: '40%',
      data: 'Name of the new experiment part'
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (!dialogResult) {
        return;
      }

      this.experimentApi
        .addExperimentPart(experiment.id, <ExperimentPart>{
          name: dialogResult
        })
        .subscribe(this.refreshParts$);
    });
  }

  startExperimentPart(part: ExperimentPart) {
    this.experimentApi
      .startExperimentPart(part.experimentId, part.id)
      .subscribe(() => this.refreshParts$.next({}));
  }

  stopExperimentPart(part: ExperimentPart) {
    this.experimentApi
      .stopExperimentPart(part.experimentId, part.id)
      .subscribe(() => this.refreshParts$.next({}));
  }

  uploadPayload(file: File, experiment: Experiment): void {
    this.experimentApi.uploadFile(file).subscribe(id => {
      experiment.payloadHash = id;
      experiment.payloadFilename = file.name;
      this.experimentController
        .updateExperiment(experiment)
        .subscribe(() => {});
      this.payloadFile = undefined;
    });
  }

  parametersUpdate(params: FixedParameter[], experiment: Experiment) {
    experiment.sharedFixedParameter = params;
    this.experimentController.updateExperiment(experiment).subscribe(() => {});
  }
}
