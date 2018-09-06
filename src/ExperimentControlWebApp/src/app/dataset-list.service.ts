import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { ExperimentcontrollerService } from './experimentcontroller.service';
import { Dataset } from './model/models';
import { switchMap, map, tap, startWith } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DatasetListService {
  public datasets$: Observable<Dataset[]>;
  public refresh$ = new Subject<{}>();

  constructor(private experimentController: ExperimentcontrollerService) {
    this.datasets$ = this.refresh$.pipe(
      startWith({}),
      switchMap(id => experimentController.getDatasets())
    );
  }
}
