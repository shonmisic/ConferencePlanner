import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Location } from '@angular/common';
import { switchMap, filter } from 'rxjs/operators';

import { Session } from '../shared/model';
import { DataService } from '../shared/data.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'session-detail',
  templateUrl: './session-detail.component.html',
})
export class SessionDetailComponent implements OnInit {
  session$: Observable<Session>;

  constructor(
    private sessionService: DataService,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit() {
    this.session$ = this.route.paramMap
      .pipe(
        filter((params: ParamMap) => !!params.get('id')),
        switchMap((params: ParamMap) =>
          this.sessionService.getSession(+params.get('id'))
        )
      );
  }

  goBack() {
    this.location.back();
  }
}
