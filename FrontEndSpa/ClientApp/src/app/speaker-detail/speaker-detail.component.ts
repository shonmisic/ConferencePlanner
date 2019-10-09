import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Location } from '@angular/common';

import { DataService } from '../shared/data.service';
import { Speaker } from '../shared/model';
import { switchMap, filter } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'conf-speakerdetail',
  templateUrl: './speaker-detail.component.html',
})
export class SpeakerDetailComponent implements OnInit {
  speaker$: Observable<Speaker>;

  constructor(
    private dataService: DataService,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit() {
    this.speaker$ = this.route.paramMap.pipe(
      filter((params: ParamMap) => !!params.get('id')),
      switchMap((params: ParamMap) =>
        this.dataService.getSpeaker(+params.get('id'))
      )
    );
  }

  goBack() {
    this.location.back();
  }
}
