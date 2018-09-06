import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { ExperimentcontrollerService } from '../experimentcontroller.service';
import { Router } from '@angular/router';
import { Dataset } from '../model/models';

@Component({
  selector: 'app-datasetoverview',
  template: `
    <button (click)="refresh()">Refresh</button>

    <mat-table [dataSource]="datasets" matSort class="datasets-overview-table">
      <ng-container matColumnDef="Id">
        <mat-header-cell *matHeaderCellDef mat-sort-header>Id</mat-header-cell>
        <mat-cell *matCellDef="let dataset"> {{dataset.id}} </mat-cell>
      </ng-container>

      <ng-container matColumnDef="Name">
        <mat-header-cell *matHeaderCellDef mat-sort-header>Name</mat-header-cell>
        <mat-cell *matCellDef="let dataset"> {{dataset.name}} </mat-cell>
      </ng-container>

      <!--
      <ng-container matColumnDef="expandedDetail">
        <mat-cell *matCellDef="let dataset" [attr.colspan]="displayedColumns.length">
          <div class="element-detail" [@detailExpand]="dataset === expandedElement?'expanded':'collapsed'">          
          {{dataset.description}}
          </div>
        </mat-cell>
      </ng-container>-->


      <ng-container matColumnDef="Actions">
        <mat-header-cell *matHeaderCellDef></mat-header-cell>
        <mat-cell class="action-cell" *matCellDef="let dataset" (click)="$event.stopPropagation()">
            <button mat-mini-fab class="action-btn" (click)="deleteDataset(dataset)"><i class="material-icons">delete</i></button>
        </mat-cell>
      </ng-container>

      <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
      <mat-row *matRowDef="let row; columns: displayedColumns;"
        (click)="navigate(row)"></mat-row>
      <!--
      <mat-row *matRowDef="let row; columns: displayedColumns;"
        matRipple
        class="element-row"
        [class.expanded] = "expandedElement === row"
        (click)="expandedElement = row">
      </mat-row>
      <mat-row *matRowDef="let row; columns: ['expandedDetail']" class="detail-row"></mat-row>-->
    </mat-table>
    
  `,
  styles: [
    `
      th.mat-sort-header-sorted {
        color: black;
      }

      .action-btn {
        margin-left: 8px;
      }
    `
  ]
})
export class DatasetoverviewComponent implements OnInit {
  datasets: Observable<Dataset[]>;
  expandedElement: Dataset;
  selectedDataset: Dataset;
  displayedColumns: string[] = ['Id', 'Name', 'Actions'];

  constructor(
    private experimentController: ExperimentcontrollerService,
    private router: Router
  ) {}

  ngOnInit() {
    this.refresh();
  }

  refresh() {
    this.datasets = this.experimentController.getDatasets();
  }

  async deleteDataset(dataset: Dataset) {
    this.experimentController.deleteDataset(dataset);
    this.refresh();
  }

  navigate(dataset: Dataset) {
    this.selectedDataset = dataset;
    this.router.navigate(['/datasets', dataset.id]);
  }
}
