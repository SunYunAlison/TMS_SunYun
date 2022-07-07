import { Component, Input, OnInit } from '@angular/core';
import { trigger,state, style,transition,animate } from "@angular/animations";
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

  constructor() { }

  ngOnInit() {
  }

  flip:string = 'inactive';
  toggleFlip(){
    this.flip = (this.flip == 'inactive')? 'active' : 'inactive';
  }

}
