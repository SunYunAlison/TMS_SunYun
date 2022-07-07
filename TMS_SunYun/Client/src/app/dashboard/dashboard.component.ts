import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  freezerSpec = [
    "freezer01",
    "freezer02",
    "freezer04",
    "freezer05",
    "freezer06",
    "freezer07",
    "freezer08",
    "freezer09",

  ]


  constructor() { }

  ngOnInit(): void {
  }

}
