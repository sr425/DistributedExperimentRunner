import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ObservableLike, Observable } from 'rxjs';
import { ExperimentPart } from '../model/models';

@Injectable({
  providedIn: 'root'
})
export class ExperimentApiService {
  private base = 'http://localhost:5000';
  private experimentUrl = this.base + '/api/experiments';

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    return new HttpHeaders({
      'Content-Type': 'application/json'
    });
  }

  public getExperimentParts(
    experimentId: number
  ): Observable<ExperimentPart[]> {
    return this.http.get<ExperimentPart[]>(
      `${this.experimentUrl}/${experimentId}/experimentparts`
    );
  }

  public getExperimentPart(
    experimentId: number,
    partId: number
  ): Observable<ExperimentPart> {
    return this.http.get<ExperimentPart>(
      `${this.experimentUrl}/${experimentId}/experimentparts/${partId}`
    );
  }

  public addExperimentPart(
    experimentId: number,
    part: ExperimentPart
  ): Observable<any> {
    return this.http.post(
      `${this.experimentUrl}/${experimentId}/experimentparts`,
      part
    );
  }

  public updateExperimentPart(
    experimentId: number,
    part: ExperimentPart
  ): Observable<any> {
    return this.http.put(
      `${this.experimentUrl}/${experimentId}/experimentparts/${part.id}`,
      part
    );
  }

  public deleteExperimentPart(
    experimentId: number,
    partId: number
  ): Observable<any> {
    return this.http.delete(
      `${this.experimentUrl}/${experimentId}/experimentparts/${partId}`
    );
  }

  public startExperimentPart(
    experimentId: number,
    partId: number
  ): Observable<any> {
    return this.http.post(
      `${this.experimentUrl}/${experimentId}/experimentparts/${partId}/start`,
      {}
    );
  }

  public stopExperimentPart(
    experimentId: number,
    partId: number
  ): Observable<any> {
    return this.http.post(
      `${this.experimentUrl}/${experimentId}/experimentparts/${partId}/stop`,
      {}
    );
  }

  public uploadFile(file: File): Observable<string> {
    const formData = new FormData();
    formData.set('file', file, file.name);
    return this.http.post(`${this.base}/api/payload`, formData, {
      responseType: 'text'
    });
  }
}
