import { Component, OnInit, ViewChild } from '@angular/core';
import { ExperimentcontrollerService } from '../experimentcontroller.service';
import { Experiment } from '../model/experiment';
import { MatTableDataSource, MatSort, MatDialog } from '@angular/material';
import { ExperimentcreationdialogComponent } from '../dialogs/experimentcreationdialog/experimentcreationdialog.component';
import { DeleteconfirmationdialogComponent } from '../dialogs/deleteconfirmationdialog/deleteconfirmationdialog.component';
import { Router } from '@angular/router';
import { ExperimentstateService } from '../experimentstate.service';

@Component({
  selector: 'app-experimentoverview',
  template: `
    <button (click)="refreshExperimentList()">Click me</button>

    <mat-form-field>
      <input matInput (keyup)="applyFilter($event.target.value)" placeholder="Filter">
    </mat-form-field>

    <button mat-mini-fab (click)="createNewExperiment()"><i class="material-icons">add</i></button>

    <mat-table [dataSource]="experiments" matSort class="experiment-overview-table">
      <ng-container matColumnDef="Id">
        <mat-header-cell *matHeaderCellDef mat-sort-header>Id</mat-header-cell>
        <mat-cell *matCellDef="let exp"> {{exp.id}} </mat-cell>
      </ng-container>

      <ng-container matColumnDef="Name">
        <mat-header-cell *matHeaderCellDef mat-sort-header>Name</mat-header-cell>
        <mat-cell *matCellDef="let exp"> {{exp.name}} </mat-cell>
      </ng-container>

     <ng-container matColumnDef="Actions">
        <mat-header-cell *matHeaderCellDef></mat-header-cell>
        <mat-cell class="action-cell" *matCellDef="let row" (click)="$event.stopPropagation()">
            <button mat-mini-fab class="action-btn" (click)="deleteExperiment(row)"><i class="material-icons">delete</i></button>
            <button mat-mini-fab class="action-btn" (click)="experimentStateService.startExperiment(row)"><i class="material-icons">play_arrow</i></button>
            <button mat-mini-fab class="action-btn" (click)="experimentStateService.stopExperiment(row)"><i class="material-icons">pause</i></button>
        </mat-cell>
      </ng-container>

      <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
      <mat-row *matRowDef="let row; columns: displayedColumns;"
        (click)="navigate(row)"
        [class.selected]="row === selectedExperiment"></mat-row>
    </mat-table>
  `,
  styles: [
    `
      th.mat-sort-header-sorted {
        color: black;
      }
      .selected {
        background-color: #cfd8dc !important;
        color: white;
      }

      .action-btn {
        margin-left: 8px;
      }
    `
  ]
})
export class ExperimentoverviewComponent implements OnInit {
  selectedExperiment: Experiment;
  experiments = new MatTableDataSource<Experiment>();
  displayedColumns: string[] = ['Id', 'Name', 'Actions'];
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private experimentController: ExperimentcontrollerService,
    private dialog: MatDialog,
    private router: Router,
    public experimentStateService: ExperimentstateService
  ) {}

  ngOnInit() {
    this.experiments.sort = this.sort;
    this.refreshExperimentList();
  }

  async refreshExperimentList(): Promise<void> {
    this.experimentController.getExperiments().subscribe(exp => {
      this.experiments.data = exp;
    });
  }

  navigate(experiment: Experiment): void {
    this.selectedExperiment = experiment;
    this.router.navigate(['/experiments', experiment.id]);
  }

  applyFilter(filterValue: string) {
    this.experiments.filter = filterValue.trim().toLowerCase();
  }

  createNewExperiment() {
    const dialogRef = this.dialog.open(ExperimentcreationdialogComponent, {
      width: '40%'
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      if (!dialogResult) {
        return;
      }

      this.experimentController
        .createExperiment(<Experiment>dialogResult)
        .subscribe(() => {
          this.refreshExperimentList();
        });
    });
  }

  deleteExperiment(exp: Experiment) {
    const dialogRef = this.dialog.open(DeleteconfirmationdialogComponent, {
      width: '40%',
      data: `${exp.name}+${exp.id}`
    });
    dialogRef.afterClosed().subscribe(async dialogResult => {
      if (!dialogResult || dialogResult.text !== `${exp.name}+${exp.id}`) {
        return;
      }

      this.experimentController.deleteExperiment(exp).subscribe(() => {
        this.refreshExperimentList();
      });
    });
  }
}
