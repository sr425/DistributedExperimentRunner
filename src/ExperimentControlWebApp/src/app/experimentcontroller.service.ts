import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ObservableLike, Observable } from 'rxjs';
import { Experiment, InstanceTask, TaskSet } from './model/models';
import { Dataset } from './model/dataset';
import { Folder } from './model/folder';

@Injectable({
  providedIn: 'root'
})
export class ExperimentcontrollerService {
  private base = 'http://localhost:5000';
  private experimentUrl = this.base + '/api/experiments';
  private datasetUrl = this.base + '/api/datasets';
  private tasksUrl = this.base + '/api/tasksets';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json'
    });
  }

  public getFolderstructure(): Observable<Folder[]> {
    const opt = this.getHeaders();
    return this.http.get<Folder[]>(`${this.base}/api/folders`, {
      headers: opt
    });
  }

  public updateFolderstructure(folders: Folder[]): Observable<any> {
    const opt = this.getHeaders();
    return this.http.post(`${this.base}/api/folders`, folders, {
      headers: opt
    });
  }

  public getExperiments(): Observable<Experiment[]> {
    const opt = this.getHeaders();
    return this.http.get<Experiment[]>(this.experimentUrl, { headers: opt });
  }

  public getExperiment(id: number): Observable<Experiment> {
    const opt = this.getHeaders();
    return this.http.get<Experiment>(`${this.experimentUrl}/${id}`, {
      headers: opt
    });
  }

  public createExperiment(experiment: Experiment): Observable<Experiment> {
    const opt = this.getHeaders();
    return this.http.post<Experiment>(this.experimentUrl, experiment, {
      headers: opt
    });
  }

  public updateExperiment(experiment: Experiment): Observable<Experiment> {
    const opt = this.getHeaders();
    return this.http.put<Experiment>(
      `${this.experimentUrl}/${experiment.id}`,
      experiment,
      {
        headers: opt
      }
    );
  }

  public deleteExperiment(experiment: Experiment): Observable<{}> {
    const opt = this.getHeaders();
    const url = `${this.experimentUrl}/${experiment.id}`;
    return this.http.delete(url, { headers: opt });
  }

  public getTasks(tasksetId: number): Observable<InstanceTask[]> {
    const opt = this.getHeaders();
    const url = `${this.tasksUrl}/${tasksetId}/tasks`;
    return this.http.get<InstanceTask[]>(url, { headers: opt });
  }

  public getDatasets(): Observable<Dataset[]> {
    const opt = this.getHeaders();
    return this.http.get<Dataset[]>(this.datasetUrl, { headers: opt });
  }

  public getDataset(id: number) {
    const opt = this.getHeaders();
    return this.http.get<Experiment>(`${this.datasetUrl}/${id}`, {
      headers: opt
    });
  }

  public async deleteDataset(dataset: Dataset): Promise<void> {
    const opt = this.getHeaders();
    const url = `${this.datasetUrl}/${dataset.id}`;
    try {
      await this.http.delete(url, { headers: opt }).toPromise();
    } catch (err) {
      if (err.status !== 404) {
        throw err;
      }
      return;
    }
  }

  public getTasksets(experimentId: number): Observable<TaskSet[]> {
    const opt = this.getHeaders();
    return this.http.get<TaskSet[]>(
      `${this.experimentUrl}/${experimentId}/tasksets`,
      { headers: opt }
    );
  }

  public generateTasksets(experimentId: number): Observable<TaskSet[]> {
    const opt = this.getHeaders();
    return this.http.post<TaskSet[]>(
      `${this.experimentUrl}/${experimentId}/generatetasksets`,
      { headers: opt }
    );
  }

  public generateTasks(
    experimentId: number,
    tasksetId: number
  ): Observable<InstanceTask[]> {
    const opt = this.getHeaders();
    return this.http.post<InstanceTask[]>(
      `${
        this.experimentUrl
      }/${experimentId}/tasksets/${tasksetId}/generatetasks`,
      { headers: opt }
    );
  }

  public startTasks(experimentId: number): Observable<any> {
    const opt = this.getHeaders();
    return this.http.post(`${this.experimentUrl}/${experimentId}/start`, {
      headers: opt
    });
  }
}
