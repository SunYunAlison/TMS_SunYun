import { AfterViewInit, Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

import { AlarmData, AlarmAcion } from "./model";
import { AlarmDataService } from "../services/alarm-data.service";
import { PopAlertService } from "../services/pop-alert.service";
import { PopCommentComponent } from "./pop-comment/pop-comment.component";

@Component({
  selector: 'app-alarm',
  templateUrl: './alarm.component.html',
  styleUrls: ['./alarm.component.scss']
})
export class AlarmComponent implements AfterViewInit, OnInit, OnDestroy {
  private readonly destroyed = new Subject<void>();

  alarmList: AlarmData[] = [];
  alarmDataSource = new MatTableDataSource(this.alarmList);
  displayedColumns: string[] = ['EventId', 'Freezer', 'Status', 'Temperature', 'AlarmTime', 'Action'];
  @ViewChild('paginator') paginator!: MatPaginator;
  ngAfterViewInit() {
    this.alarmDataSource.paginator = this.paginator;
  }
  constructor(private popAlertService: PopAlertService,
    private dialog: MatDialog,
    private alarmDataService: AlarmDataService) { }

  ngOnInit() {
    this.alarmDataSource.paginator = this.paginator;
    this.getAlarmList();
  }

  getAlarmList() {
    this.alarmDataService.GetAlarmList().pipe(takeUntil(this.destroyed)).subscribe((res: any) => {
      if (res.Result == "Success") {
        this.alarmList = JSON.parse(res.Data);
        this.alarmDataSource = new MatTableDataSource(this.alarmList);
        this.alarmDataSource.paginator = this.paginator;
      }
      else {
        console.log(res.Error);
      }
    },
      (error) => {
        console.log(error);
      });
  }

  updateAlarmAction(alarmAction: any) {
    this.alarmDataService.UpdateAlarmAction(alarmAction).pipe(takeUntil(this.destroyed)).subscribe((res: any) => {
      if (res.Result == "Success") {
        this.popAlertService.setSuccessMsg("Successfully comment!")
        this.getAlarmList();
      }
      else {
        this.popAlertService.setErrorMsg(res.Error);
      }
    },
      (error) => {
        console.log(error);
      });
  }

  popComment(alarmRecord: any) {
    let alarmAction: AlarmAcion = {
      Id: (alarmRecord.ID).toString(),
      EqpId: alarmRecord.EQP_ID,
      AlarmStatus: alarmRecord.ALARM_STATUS,
      AlarmTime: alarmRecord.ALARM_TIME,
      AlarmValue: alarmRecord.ALARM_VALUE,
      RecoverTime: alarmRecord.RECOVER_TIME,
      Comment: "",
      CommentBy: "",
      CommentTime: "",
      IsSendAlarm: alarmRecord.IS_SEND_ALARM,
      IsSendRecover: alarmRecord.IS_SEND_RECOVER,
      UpdateBy: alarmRecord.UPDATE_BY,
      UpdateTime: alarmRecord.UPDATE_TIME,
    };

    const dialogRef = this.dialog.open(PopCommentComponent, {
      data: {
        ID: alarmRecord.ID,
        Freezer: alarmRecord.EQP_NAME,
        AlarmTime: alarmRecord.ALARM_TIME,
        Temp: alarmRecord.ALARM_VALUE,
        Status: alarmRecord.ALARM_STATUS,
        Comment: alarmRecord.COMMENT,
        User: alarmRecord.COMMENT_BY
      },
      panelClass: 'mat-dialog-container-custom',
      disableClose: true
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result && result.ID == alarmRecord.ID) {
        alarmAction.Comment = result.Comment;
        alarmAction.CommentBy = result.User;
        this.updateAlarmAction(alarmAction);
      }
      else {
        this.popAlertService.setErrorMsg("Event ID mismatch!");
      }
    });
  }

  ngOnDestroy() {
    this.destroyed.next(undefined);
    this.destroyed.complete();
  }

}
