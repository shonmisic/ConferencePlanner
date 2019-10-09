import { Component, OnInit } from '@angular/core';

import { DataService } from '../shared/data.service';
import { Session } from '../shared/model';
import { Observable } from 'rxjs';

@Component({
  selector: 'conf-sessions',
  templateUrl: './sessions.component.html',
})
export class SessionsComponent implements OnInit {
  sessions$: Observable<Session[]>;

  constructor(private dataService: DataService) {}

  ngOnInit() {
    this.getSessions();
  }

  getSessions() {
    this.sessions$ = this.dataService.getSessions();
  }
}
