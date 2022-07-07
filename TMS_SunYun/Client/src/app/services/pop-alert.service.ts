import { Injectable } from '@angular/core';
import { MatDialog } from "@angular/material/dialog";
import { PopAlertComponent } from "../layout/pop-alert/pop-alert.component";

@Injectable(
  {providedIn: 'root'}
)
export class PopAlertService {

  constructor(private dialog: MatDialog) { }

  setErrorMsg(errorMsg: string) {
    if (errorMsg != "") {
      this.messagePopUp("Error", errorMsg);
    }
  }

  setSuccessMsg(successMsg: string) {
    this.messagePopUp("Success", successMsg);
  }

  public messagePopUp(type: any, message: any) {
    let dialogRef = this.dialog.open(PopAlertComponent, {
      panelClass: 'mat-dialog-container-custom',
      data: {
        type: type,
        message: message
      },
      disableClose: false
    });
  }

}
