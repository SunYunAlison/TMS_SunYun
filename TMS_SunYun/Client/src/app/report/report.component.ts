import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { Freezer } from "./model";
import { ReportDataService } from "../services/report-data.service";
import { PopAlertService } from "../services/pop-alert.service";
import { CsvDataService } from "../services/csv-data.service";
import { AppConfigService } from '../services/app-config.service';

declare var Plotly: any;

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})
export class ReportComponent implements OnInit, OnDestroy {
  private readonly destroyed = new Subject<void>();

  chartTitle: string = "" //write into config and convert to a variable
  selectedFreezer: Freezer = {
    EqpId: '',
    EqpName: '',
    HI: 0,
    HIHI: 0,
    LO: 0,
    LOLO: 0,
    Offset: 0,
    UpdateBy: '',
  };
  freezerInfoList: any[] = [];
  selectedDataType: string = "";
  dataTypeList: Array<string> = [];
  dataTypeFlag: boolean = false;

  dateRange!: FormGroup;
  startTime: string = '';
  endTime: string = '';
  defaultDay: number = 7; // write into config
  temperatureData!: any;

  hihi: any;
  high: any;
  lolo: any;
  low: any;
  setpoint: any;

  hours: Array<number> = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23];
  selectedStartHour: number = 0;
  selectedEndHour: number = 0;

  constructor(
    // private csvDataService: CsvDataService, 
    private reportDataService: ReportDataService,
    private popupService: PopAlertService,
    private environment: AppConfigService) {
    this.defaultDay = this.environment.config.defaultDay;
    this.chartTitle = this.environment.config.chartTitle;
    const today = new Date();
    const preDay = new Date(today.getTime() - ((this.defaultDay - 1) * 24 * 60 * 60 * 1000));
    this.dateRange = new FormGroup({
      start: new FormControl(preDay),
      end: new FormControl(today),
    });
  }

  ngOnInit() {
    this.getFreezerList();
    this.dataTypeList = [
      "by hour",
      "by minute",
    ];
    this.selectedDataType = this.dataTypeList[0];
    this.selectedStartHour = this.hours[0];
    this.selectedEndHour = this.hours[23];
    setTimeout(() => {
      this.onClickSubmit();
    }, 100);
  }

  getFreezerList() {
    this.reportDataService.GetFreezerList().pipe(takeUntil(this.destroyed)).subscribe((res: any) => {
      console.log(res);
      if (res.Result == "Success") {
        this.freezerInfoList = JSON.parse(res.Data);
        this.freezerInfoList.sort((a, b) => {
          let nameA = a.EqpName.toUpperCase();
          let nameB = b.EqpName.toUpperCase();
          if (nameA < nameB) {
            return -1;
          }
          if (nameA > nameB) {
            return 1;
          }
          return 0;
        })
        this.selectedFreezer = this.freezerInfoList[0];
      }
      else {
        console.log(res.Error);
      }
    },
      (error) => {
        console.log(error);
      });
  }

  getTempbyMin(freezerId: string, startTime: string, endTime: string) {
    this.reportDataService.GetTempbyMin(freezerId, startTime, endTime).pipe(takeUntil(this.destroyed)).subscribe((res: any) => {
      if (res.body.Result == "Success") {
        let data = JSON.parse(res.body.Data);
        this.temperatureData = this.dataTransfer(data);
        if (this.temperatureData) {
          this.drawChart(this.temperatureData);
        }
      }
      else {
        console.log(res.body.Error);
      }
    },
      (error) => {
        console.log(error);
      });
  }

  getTempbyHour(freezerId: string, startTime: string, endTime: string) {
    this.reportDataService.GetTempbyHour(freezerId, startTime, endTime).pipe(takeUntil(this.destroyed)).subscribe((res: any) => {
      if (res.body.Result == "Success") {
        let data = JSON.parse(res.body.Data);
        this.temperatureData = this.dataTransfer(data);
        if (this.temperatureData) {
          this.drawChart(this.temperatureData);
        }
      }
      else {
        console.log(res.body.Error);
      }
    },
      (error) => {
        console.log(error);
      });
  }

  updateDateRange() {
    let dateStart = this.dateRange.value.start;
    let dateEnd = this.dateRange.value.end;
    let today = new Date;
    let range = (dateEnd.getTime() - dateStart.getTime()) / (24 * 60 * 60 * 1000);
    let limit = (today.getTime() - dateStart.getTime()) / (24 * 60 * 60 * 1000);
    if (range > 365 || limit > 365) {
      this.popupService.messagePopUp("Error", "Data cannot exceed 1 year!");
    }
    else if (range > 90) {
      this.selectedDataType = this.dataTypeList[0]; // by hour
      this.dataTypeFlag = true;
      console.log(">90");
      this.popupService.messagePopUp("Success", "Please take note that selected date range is more than 90 days!");
    }
    else if (range > 7) {
      this.selectedDataType = this.dataTypeList[0]; // by hour
      this.dataTypeFlag = true;
      console.log(">7");
      this.popupService.messagePopUp("Success", "Selected data range more than 7 days, data type change to 'by hour'!");
    }
    else {
      this.selectedDataType = this.dataTypeList[1];
      this.dataTypeFlag = false;
    }
  }

  onClickSubmit() {
    let dateStart = this.dateRange.value.start;
    let dateEnd = this.dateRange.value.end;

    dateStart.setHours(this.selectedStartHour);
    dateStart.setMinutes(0);
    dateStart.setSeconds(0);

    dateEnd.setHours(this.selectedEndHour);
    dateEnd.setMinutes(59);
    dateEnd.setSeconds(59);
    this.startTime = this.dateToString(dateStart);
    this.endTime = this.dateToString(dateEnd);

    if (this.selectedDataType == "by minute") { // use bymin api
      this.getTempbyMin(this.selectedFreezer.EqpName, this.startTime, this.endTime);
    }
    else {
      this.getTempbyHour(this.selectedFreezer.EqpName, this.startTime, this.endTime);
    }
  }

  onClickExportCSV() {
    let dateStart = this.dateRange.value.start;
    let dateEnd = this.dateRange.value.end;

    dateStart.setHours(this.selectedStartHour);
    dateStart.setMinutes(0);
    dateStart.setSeconds(0);

    dateEnd.setHours(this.selectedEndHour);
    dateEnd.setMinutes(59);
    dateEnd.setSeconds(59);
    this.startTime = this.dateToString(dateStart);
    this.endTime = this.dateToString(dateEnd);

    let csvName = this.selectedFreezer.EqpName + '_' + this.selectedDataType + '_' + this.startTime.slice(0, 13) + '_' + this.endTime.slice(0, 13);
    if (this.selectedDataType == "by minute") {
      this.reportDataService.DownloadTempbyMin(this.selectedFreezer.EqpName, this.startTime, this.endTime).pipe(takeUntil(this.destroyed)).subscribe((res: any) => {
        if (res.body.Result == "Success") {
          let data = JSON.parse(res.body.Data);
          CsvDataService.exportToCsv(csvName, data);
        }
        else {
          console.log(res.body.Error);
        }
      },
        (error) => {
          console.log(error);
        });
    }
    else {
      this.reportDataService.DownloadTempbyHour(this.selectedFreezer.EqpName, this.startTime, this.endTime).pipe(takeUntil(this.destroyed)).subscribe((res: any) => {
        if (res.body.Result == "Success") {
          let data = JSON.parse(res.body.Data);
          CsvDataService.exportToCsv(csvName, data);
        }
        else {
          console.log(res.body.Error);
        }
      },
        (error) => {
          console.log(error);
        });
    }
  }

  drawChart(data: any) {
    //create freezer spec data
    console.log(data);
    this.low = data.lo;
    this.lolo = data.lolo;
    this.high = data.hi;
    this.hihi = data.hihi;
    this.setpoint = ((Number(this.low) + Number(this.high)) / 2);
    let hh = Array(2).fill(this.hihi);
    let h = Array(2).fill(this.high);
    let ll = Array(2).fill(this.lolo);
    let l = Array(2).fill(this.low);
    let setpoint = Array(2).fill(this.setpoint);

    let x = data.datetimeArray;
    x = x.filter((n: any) => n != "null");  // remove "null" value get from API
    x = x.filter((n: any) => n != "");
    let xSpec = [x[0], x.pop()];
    x.push(xSpec[1]);
    let y1 = data.dataArray;
    y1 = y1.filter((n: any) => n != "null"); // remove "null" value get from API
    y1 = y1.filter((n: any) => n != "");
    console.log("x", x);
    console.log("y1", y1);
    console.log("xSpec", xSpec);
    var trace0 = {
      x: xSpec,
      y: setpoint,
      type: 'scatter',
      mode: 'lines',
      name: "Setpoint",
      line: {
        // dash: 'dashdot',
        color: '#bdbebd'
      },
    };

    var trace1 = {
      x: x,
      y: y1,
      type: 'scatter',
      mode: 'lines',
      name: "Temperature",
      line: {
        color: '#014a6f',
      },
      text: "temperature"
    };

    var trace2 = {
      x: xSpec,
      y: hh,
      type: 'scatter',
      mode: 'lines',
      name: "High",
      line: {
        // dash: 'dashdot',
        color: '#ff5232'
      },
    };

    var trace3 = {
      x: xSpec,
      y: h,
      type: 'scatter',
      mode: 'lines',
      name: "HighHigh",
      line: {
        dash: 'dashdot',
        color: '#ff5232'
      },
    };

    var trace4 = {
      x: xSpec,
      y: ll,
      type: 'scatter',
      mode: 'lines',
      name: "LowLow",
      line: {
        // dash: 'dashdot',
        color: '#ff5232'
      },
    };

    var trace5 = {
      x: xSpec,
      y: l,
      type: 'scatter',
      mode: 'lines',
      name: "Low",
      line: {
        dash: 'dashdot',
        color: '#ff5232'
      },
    };

    var dataset = [trace1, trace2, trace3, trace4, trace5, trace0];

    var layout = {
      xaxis: {
        type: "date"
      },
      // title: "Temperature Chart",
      showlegend: true,
    };

    var plotConfig = { displayModeBar: false, responsive: true };

    Plotly.newPlot('hourPlotChart', dataset, layout, plotConfig);

  }

  dateToString(date: Date) {
    let yyyy = date.getFullYear();
    let month = date.getMonth();
    let mm = month >= 9 ? (month + 1).toString() : '0' + (month + 1).toString();
    let day = date.getDate();
    let dd = day > 9 ? day.toString() : '0' + day.toString();
    let hour = date.getHours();
    let hh = hour > 9 ? hour.toString() : '0' + hour.toString();
    let minute = date.getMinutes();
    let MM = minute > 9 ? minute.toString() : '0' + minute.toString();
    let second = date.getSeconds();
    let ss = second > 9 ? second.toString() : '0' + second.toString();
    return yyyy + '-' + mm + '-' + dd + ' ' + hh + ':' + MM + ':' + ss;
  }

  // convert backend api long string to array object
  dataTransfer(longString: any) {
    let data = {
      hihi: 0,
      hi: 0,
      lolo: 0,
      lo: 0,
      dataArray: [],
      datetimeArray: []
    };
    let temp = longString.slice(1, -2);
    let temp1 = temp.split('],[');
    console.log(temp1);
    data.hihi = Number(temp1[0].split(',')[1]);
    data.hi = Number(temp1[1].split(',')[1]);
    data.lolo = Number(temp1[2].split(',')[1]);
    data.lo = Number(temp1[3].split(',')[1]);

    if (temp1.length > 4) {
      let dateString = temp1[4];
      dateString = dateString.replaceAll('"DATE"', '');
      dateString = dateString.replaceAll(this.selectedFreezer.EqpName, '');
      dateString = dateString.slice(1);
      dateString = dateString.replaceAll('"', '');
      dateString = dateString.slice(2);
      data.datetimeArray = dateString.split(',');
      let dataString = temp1[5];
      dataString = dataString.replaceAll('"VALUE"', '');
      dataString = dataString.replaceAll(this.selectedFreezer.EqpName, '');
      dataString = dataString.slice(1);
      dataString = dataString.replaceAll('"', '');
      dataString = dataString.slice(2);
      data.dataArray = dataString.split(',');
      return data;
    }
    else {
      this.popupService.setErrorMsg("No data!");
      return false;
    }
  }

  ngOnDestroy() {
    this.destroyed.next(undefined);
    this.destroyed.complete();
  }

}
