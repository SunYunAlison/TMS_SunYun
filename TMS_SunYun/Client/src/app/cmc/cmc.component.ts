import { AfterViewInit, Component, OnInit, OnDestroy, ViewChild, ViewChildren, QueryList  } from '@angular/core';
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from '@angular/material/table';
import { Subject } from 'rxjs';
import { interval, Subscription } from "rxjs";
import { takeUntil } from 'rxjs/operators';
import { SelectionModel } from '@angular/cdk/collections';
import { FormControl } from '@angular/forms';

import { AppConfigService } from '../services/app-config.service';
import { CmcDataService } from "../services/cmc-data.service";
import { PopAlertService } from "../services/pop-alert.service";
import { Messages } from "./model";
import { Users } from "./model";

@Component({
  selector: 'app-cmc',
  templateUrl: './cmc.component.html',
  styleUrls: ['./cmc.component.scss']
})
export class CmcComponent implements AfterViewInit, OnInit, OnDestroy  {

  private readonly destroyed = new Subject<void>();
  private updateSubscription!: Subscription;

  displayedColumns: string[] = ['reportTime', 'messageCode', 'messageBody', 'chatId', 'chatGroup', 'lmUser', 'lmTime'];
  messageList: Messages[] = [];
  messsageDataSource = new MatTableDataSource<Messages>(this.messageList);

  userColumns: string[] = ['select', 'userName', 'userId', 'email', 'contactNo'];
  user: Users = new Users();
  userList: Users[] = [];
  userDataSource = new MatTableDataSource(this.userList);
  selection = new SelectionModel<Users>(false, []);
  selectItem = new Users();
  successMsg: string = "";
  errorMsg: string = "";
  userIdText: string = "";
 
  keyword: string = "";
  clickAdd: boolean = false;
  clickUpdate: boolean = false;
  clickDelete: boolean = false;
  result: boolean = false;
  userName = new FormControl('');
  userId = new FormControl('');
  email = new FormControl('');
  contactNo = new FormControl('');
  emailFormat = new RegExp(/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/);
  autoRefresh: any;

  @ViewChildren(MatPaginator) paginator = new QueryList<MatPaginator>();

  ngAfterViewInit(){
    this.userDataSource.paginator = this.paginator.toArray()[1];
    this.messsageDataSource.paginator = this.paginator.toArray()[0];
  }

  constructor(private commonDataService: CmcDataService, private environment: AppConfigService, private popupService: PopAlertService) { 
    this.autoRefresh = environment.config.autoRefresh;
  }

  ngOnInit() {
    this.getMessageData();
    if (this.autoRefresh != "N") {
      let timeInterval = Number(this.autoRefresh) * 60 * 1000;
      this.updateSubscription = interval(timeInterval).subscribe(
        (val) => {
          this.getMessageData();
        });
    }
    this.getAllUser();
    this.successMsg = "";
    this.errorMsg = "";
    this.clickAdd = false;
    this.clickDelete = false;
    this.clickUpdate = false;
  }

  getMessageData() {
    this.commonDataService.GetMessageRecords().pipe(takeUntil(this.destroyed)).subscribe((result: any) => {
      if (result.Result == "Success") {
        this.messageList = JSON.parse(result.Data);
        this.messsageDataSource = new MatTableDataSource<Messages>(this.messageList);
        this.messsageDataSource.paginator = this.paginator.toArray()[0];
      }
      else {
        console.log(result.Error);
      }
    },
      (error) => {
        console.log(error);
      })
  }

  getAllUser() {
    this.keyword = "";
    this.commonDataService.GetAllUsers().pipe(takeUntil(this.destroyed)).subscribe((result: any) => {
      if (result.Result == "Success") {
        this.userList = JSON.parse(result.Data);
        this.userDataSource = new MatTableDataSource<Users>(this.userList);
        this.userDataSource.paginator = this.paginator.toArray()[1];
      }
      else {
        console.log(result.Error);
      }
    },
      (error) => {
        console.log(error);
      })
  }

  submitUser() {
    if (this.email.value || this.contactNo.value || this.userId.value) {
      if (this.checkEmail(this.email.value) == false) {
        if (this.clickAdd == true && this.clickUpdate == false) {
          this.user.UserName = this.userName.value;
          this.user.UserId = this.userId.value || 'NA';
          this.user.Email = this.email.value;
          this.user.ContactNo = this.contactNo.value;

          this.commonDataService.NewAccount(this.user).pipe(takeUntil(this.destroyed)).subscribe(res => {
            if ((res as any).Result == "Success") {
              this.result = true;
              this.successMsg = "Successfully added!"
              this.popupService.setSuccessMsg(this.successMsg);
              setTimeout(() => {
                this.clickAdd = false;
              }, 1000);
            } else {
              this.result = true;
              this.errorMsg = (res as any).Error;
              this.popupService.setErrorMsg(this.errorMsg);
            }
          });
        }
        else if (this.clickAdd == false && this.clickUpdate == true) {
          this.user.UserName = this.userName.value;
          this.user.UserId = this.userId.value;
          this.user.Email = this.email.value;
          this.user.ContactNo = this.contactNo.value;

          this.commonDataService.UpdateAccountInfo(this.user).pipe(takeUntil(this.destroyed)).subscribe(res => {
            if ((res as any).Result == "Success") {
              this.result = true;
              this.successMsg = "Successfully updated!"
              this.popupService.setSuccessMsg(this.successMsg);
              this.clickUpdate = false;            
            } else {
              this.result = true;
              this.errorMsg = (res as any).Error;
              this.popupService.setErrorMsg(this.errorMsg);
            }
          });
        }
      }
      else {
        this.result = true;
        this.errorMsg = "please key in a valid email address!"
        this.popupService.setErrorMsg(this.errorMsg);
      }
    }
    else {
      this.result = true;
      this.errorMsg = "please fill in at least one contact infomation!"
      this.popupService.setErrorMsg(this.errorMsg);
    }

    setTimeout(() => {
      this.user = new Users();
      this.getAllUser();
    }, 1000);

    setTimeout(() => {
      this.errorMsg = "";
      this.successMsg = "";
    }, 5000);
  }

  newAccount() {
    this.result = false;
    this.user = new Users();
    this.clickUpdate = false;
    this.clickAdd = true;
    this.userName = new FormControl('');
    this.userId = new FormControl('');
    this.email = new FormControl('');
    this.contactNo = new FormControl('');
  }

  updateAccount() {
    this.result = false;
    this.clickAdd = false;
    this.clickUpdate = true;
    if (this.selection.selected.length > 0) {
      this.selectItem = this.selection.selected[0];
      this.userName = new FormControl();
      this.userId = new FormControl();
      if (this.selectItem.UserId) {
        this.userId.disable();
      }

      this.email = new FormControl();
      this.contactNo = new FormControl();
      this.userName.setValue(this.selectItem.UserName);
      this.userId.setValue(this.selectItem.UserId);
      this.email.setValue(this.selectItem.Email);
      this.contactNo.setValue(this.selectItem.ContactNo);
    }
    else {
      this.result = true;
      this.errorMsg = "Please Select an item!";
    }
  }

  clearInput() {
    this.result = false;
    this.errorMsg = "";
    this.successMsg = "";
    this.clickAdd = false;
    this.clickUpdate = false;
    this.userName = new FormControl('');
    this.userId = new FormControl('');
    this.email = new FormControl('');
    this.contactNo = new FormControl('');
  }

  deleteAccount() {
    if (this.selection.selected.length > 0) {
      this.selectItem = this.selection.selected[0];
      this.user.UserName = this.selectItem.UserName;
      this.user.UserId = this.selectItem.UserId;
      this.user.Email = this.selectItem.Email;
      this.user.ContactNo = this.selectItem.ContactNo;
      this.commonDataService.DeleteAccount(this.user).pipe(takeUntil(this.destroyed)).subscribe((result: any) => {
        if (result.body.Result == "Success") {
          this.clickDelete = true;
          this.successMsg = "Successfully deleted!"
          this.popupService.setSuccessMsg(this.successMsg);
        }
        else {
          console.log(result.body.Error);
          this.clickDelete = true;
          this.errorMsg = result.body.Error;
        }
      },
        (error) => {
          console.log(error);
        })
    }
    else {
      this.clickDelete = true;
      this.errorMsg = "Please Select an item!";
    }

    setTimeout(() => {
      this.clickDelete = false;
      this.successMsg = "";
      this.errorMsg = "";
      this.getAllUser();
    }, 1000);
  }

  checkEmail(email: string) {
    if (this.emailFormat.test(email) || email === null || email === "") {
      return false;
    }
    else {
      return true;
    }
  }

  filterData() {
    let filterValue = this.keyword.trim().toLowerCase();
    this.userDataSource.filter = filterValue;
  }



  ngOnDestroy() {
    this.destroyed.next(undefined);
    this.destroyed.complete();
  }

}
