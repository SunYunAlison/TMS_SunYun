import { Component, OnInit,Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-pop-alert',
  templateUrl: './pop-alert.component.html',
  styleUrls: ['./pop-alert.component.scss']
})
export class PopAlertComponent implements OnInit {

  title: string='';
  message: string='';
  isError: boolean = false;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.message = this.data.message;
    if (this.data.type == 'Error') {
      this.title = 'Error';
      this.isError = true;
    }
    else {
      this.title = 'Information';
      this.isError = false;
    }
  }

}
