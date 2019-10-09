import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Speaker } from '../shared/model';
import { DataService } from '../shared/data.service';

@Component({
  selector: 'app-speakers',
  templateUrl: './speakers.component.html',
  styleUrls: ['./speakers.component.css']
})
export class SpeakersComponent implements OnInit {
  speakers$: Observable<Speaker[]>;

  constructor(private dataService: DataService) {}

  ngOnInit() {
    this.getSpeakers();
  }

  getSpeakers() {
    this.speakers$ = this.dataService.getSpeakers();
  }
}
