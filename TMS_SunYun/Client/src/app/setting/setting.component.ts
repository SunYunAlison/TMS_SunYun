import { Component, Input, OnInit } from '@angular/core';
import { trigger,state, style,transition,animate } from "@angular/animations";
import { PopAlertService } from "../services/pop-alert.service";
import { FreezerData } from "./model";
import { FREEZERDATA } from "./mock-data";

@Component({
  selector: 'app-setting',
  templateUrl: './setting.component.html',
  styleUrls: ['./setting.component.scss'],
  animations: [
    trigger('flipState',[
      state('active',style({
        transform: 'rotateY(179deg)'
      })),
      state('inactive',style({
        transform: 'rotateY(0deg)'
      })),
      transition('active => inactive',animate('500ms ease-out')),
      transition('inactive => active',animate('500ms ease-in')),
    ])
  ]
})

export class SettingComponent implements OnInit {

  @Input()boxData: FreezerData = FREEZERDATA;

  editable:boolean = true;
  editableBack:boolean = true;

  constructor(private popupService: PopAlertService) { }

  ngOnInit() {
  }

  flip:string = 'inactive';
  toggleFlip(){
    this.flip = (this.flip == 'inactive')? 'active' : 'inactive';
  }

  boxFrontSubmit( freezerName: string){
    let msg = "Freezer " + freezerName + " specification submitted successfully!"
    this.popupService.setSuccessMsg(msg);
  }

}
