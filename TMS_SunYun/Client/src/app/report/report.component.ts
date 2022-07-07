import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { HOURDATA } from "./mock-data";
import { HourlyData } from "./model";

declare var Plotly: any;

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})
export class ReportComponent implements OnInit {

  chartTitle:string = "Freezer Temperature Chart" //write into config and convert to a variable
  selectedFreezer: string = "";
  freezerNameList: Array<string> = [];
  selectedDataType: string = "";
  dataTypeList: Array<string> = [];

  dateRange!: FormGroup;
  defaultDay: number = 7; // write into config
  hourData: HourlyData = HOURDATA; //mockdata, pending API
  hihi: number = -35; // pending API
  high: number = -40; // pending API
  lolo: number = -55; // pending API
  low: number = -50; // pending API

  hours: Array<number> = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23];
  selectedStartHour:number = 0;
  selectedEndHour:number = 0;

  constructor() {
    const today = new Date();
    const preDay = new Date(today.getTime() - (this.defaultDay * 24 * 60 * 60 * 1000));
    this.dateRange = new FormGroup({
      start: new FormControl(preDay),
      end: new FormControl(today),
    });
  }

  ngOnInit() {
    this.freezerNameList = [
      "freezer-01",
      "freezer-02",
      "freezer-03",
      "freezer-04",
      "freezer-05",
      "freezer-06",
    ];
    this.dataTypeList = [
      "by hour",
      "by minute",
    ];
    this.selectedFreezer = this.freezerNameList[0];
    this.selectedDataType = this.dataTypeList[0];
    this.selectedStartHour = this.hours[0];
    this.selectedEndHour = this.hours[23];
    this.drawChart();
  }

  updateDateRange() {

    let dateStart = this.dateRange.value.start;
    let dateEnd = this.dateRange.value.end;
    console.log(dateStart.toString());
    this.selectedDataType = dateStart.toString() == dateEnd.toString() ? this.dataTypeList[1] : this.dataTypeList[0];
  }

  onClickSubmit() {
    
  }

  onClickExportCSV() {}

  drawChart() {
    let x = this.hourData.DateTime;
    let y1 = this.hourData.Data;
    let hh = Array(y1.length).fill(this.hihi);
    let h = Array(y1.length).fill(this.high);
    let ll = Array(y1.length).fill(this.lolo);
    let l = Array(y1.length).fill(this.low);

    var trace1 = {
      x: x,
      y: y1,
      type: 'scatter',
      mode: 'lines',
      name: "Temperature",
      marker: {
        color: 'blue',
        size: 12
      },
      text: "temperature"
    };

    var trace2 = {
      x: x,
      y: hh,
      type: 'scatter',
      mode: 'lines',
      name: "High",
      line: {
        // dash: 'dashdot',
        color: 'red'
      },
    };

    var trace3 = {
      x: x,
      y: h,
      type: 'scatter',
      mode: 'lines',
      name: "HighHigh",
      line: {
        dash: 'dashdot',
        color: 'red'
      },
    };

    var trace4 = {
      x: x,
      y: ll,
      type: 'scatter',
      mode: 'lines',
      name: "LowLow",
      line: {
        // dash: 'dashdot',
        color: 'red'
      },
    };

    var trace5 = {
      x: x,
      y: l,
      type: 'scatter',
      mode: 'lines',
      name: "Low",
      line: {
        dash: 'dashdot',
        color: 'red'
      },
    };

    var data = [trace1, trace2, trace3, trace4, trace5];

    var layout = {
      xaxis: {
        type: "date"
      },
      // title: "Temperature Chart",
      showlegend: true,
    };

    var plotConfig = { displayModeBar: false, responsive: true };

    Plotly.newPlot('hourPlotChart', data, layout, plotConfig);


  }

}
