import { Injectable, Inject } from '@angular/core';
import { Headers, Http } from '@angular/http';

import { Session, Speaker } from './model';
import { HttpClient } from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';

@Injectable()
export class DataService {
  private sessionUrl = 'api/sessions';
  private speakerUrl = 'api/speakers';
  /**
   * init with Http
   */
  constructor(
    private http: HttpClient,
    @Inject('API_URL') private baseUrl: string
  ) {}

  getSessions(): Observable<Session[]> {
    return this.http
      .get<Session[]>(this.baseUrl + this.sessionUrl)
      .pipe(catchError(this.handleError));
  }

  getSession(id: number): Observable<Session> {
    return this.http
      .get<Session>(`${this.baseUrl + this.sessionUrl}/${id}`)
      .pipe(catchError(this.handleError));
  }

  getSpeaker(id: number): Observable<Speaker> {
    return this.http
      .get<Speaker>(`${this.baseUrl + this.speakerUrl}/${id}`)
      .pipe(catchError(this.handleError));
  }

  getSpeakers(): Observable<Speaker[]> {
    return this.http
      .get<Speaker[]>(this.baseUrl + this.speakerUrl)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any): Observable<any> {
    console.error('An error occurred', error); // for demo purposes only
    return throwError(error.message || error);
  }
}
