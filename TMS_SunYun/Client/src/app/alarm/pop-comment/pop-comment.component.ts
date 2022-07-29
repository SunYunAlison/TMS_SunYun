import { Component, OnInit, Inject, OnDestroy } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialog } from '@angular/material/dialog';
import { FormBuilder, FormControl } from '@angular/forms';
import { Subject } from 'rxjs';

import { PopAlertService } from "../../services/pop-alert.service";

@Component({
  selector: 'app-pop-comment',
  templateUrl: './pop-comment.component.html',
  styleUrls: ['./pop-comment.component.scss']
})
export class PopCommentComponent implements OnInit, OnDestroy {
  private readonly destroyed = new Subject<void>();

  constructor(public dialogRef: MatDialogRef<PopCommentComponent>,
    private dialog: MatDialog, private formBuilder: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private popAlertService: PopAlertService) { }

  ngOnInit() {
  }

  submitComment(){
    this.popAlertService.setErrorMsg("");
    
    //Validation check
    if (this.data.Comment == '') {
      this.popAlertService.setErrorMsg("Comment cannot be empty!");
      return;
    }
    if (this.data.User == '') {
      this.popAlertService.setErrorMsg("User cannot be empty!");
      return;
    }

    this.dialogRef.close(this.data);
  }

  onCancelClick(){
    this.dialogRef.close();
  }

  ngOnDestroy() {
    this.destroyed.next(undefined);
    this.destroyed.complete();
  }

}
