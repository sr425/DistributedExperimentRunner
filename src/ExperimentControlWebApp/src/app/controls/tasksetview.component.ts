import { Component, OnInit, Input } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { TaskSet, InstanceTask } from '../model/models';
import { ExperimentcontrollerService } from '../experimentcontroller.service';
import { ExperimentstateService } from '../experimentstate.service';

@Component({
  selector: 'app-tasksetview',
  template: `
  <ng-container *ngIf="tasksets$ | async as tasksets">
    <mat-accordion *ngFor="let taskset of tasksets">
      <mat-expansion-panel (opened)="loadTasks(taskset)">
        <mat-expansion-panel-header>
          <mat-panel-title>{{taskset.id}} - {{taskset.name}}</mat-panel-title>
          <mat-panel-description>
            <button mat-mini-fab (click)="experimentController.generateTasks(taskset.experimentId, taskset.id);$event.stopPropagation()">
              <i class="material-icons">settings</i>
            </button>
            <button mat-mini-fab (click)="experimentStateService.deleteTasks(taskset);$event.stopPropagation()">
              <i class="material-icons">delete</i>
            </button>
          </mat-panel-description>
        </mat-expansion-panel-header>

        <ng-container *ngIf="tasks$ | async as tasks">
          <mat-table [dataSource]="tasks" matSort class="tasks-overview-table">
            <ng-container matColumnDef="Id">
              <mat-header-cell *matHeaderCellDef mat-sort-header>Id</mat-header-cell>
              <mat-cell *matCellDef="let task"> {{task.id}} </mat-cell>
            </ng-container>

            <ng-container matColumnDef="Name">
              <mat-header-cell *matHeaderCellDef mat-sort-header>Name</mat-header-cell>
              <mat-cell *matCellDef="let task">
                <div>{{task.id}} - {{task.name}}</div>
              </mat-cell>
            </ng-container>

            <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
            <mat-row *matRowDef="let row; columns: displayedColumns;"></mat-row>
          </mat-table>
        </ng-container>
      </mat-expansion-panel>
    </mat-accordion>
  </ng-container>
  `,
  styles: []
})
export class TasksetviewComponent implements OnInit {
  displayedColumns = ['Id', 'Name'];
  experimentIdValue: number;
  @Input()
  set experimentId(value: number) {
    this.experimentIdValue = value;
    this.refresh();
  }
  tasksets$ = new Observable<TaskSet[]>();
  tasks$ = new Observable<InstanceTask[]>();

  constructor(
    public experimentController: ExperimentcontrollerService,
    public experimentStateService: ExperimentstateService
  ) {}

  ngOnInit() {}

  refresh() {
    this.tasksets$ = this.experimentController.getTasksets(
      this.experimentIdValue
    );
  }
}
