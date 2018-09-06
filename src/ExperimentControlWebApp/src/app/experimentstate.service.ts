import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ObservableLike, Observable } from 'rxjs';
import { Experiment, InstanceTask } from './model/models';

@Injectable({
  providedIn: 'root'
})
export class ExperimentstateService {
  public startExperiment(experiment: Experiment) {
    console.log('Starting experiment');
  }

  public stopExperiment(experiment: Experiment) {
    console.log('Pausing experiment');
  }

  public deleteTasks(experiment: Experiment) {
    console.log('Deleting Tasks');
  }

  public generateTasksNew(experiment: Experiment) {}

  public startTask(experiment: Experiment, task: InstanceTask) {}

  public cancelTask(experiment: Experiment, task: InstanceTask) {}
}
