import { Component, OnInit, OnDestroy } from '@angular/core';
import { AppConfigService } from "../../services/app-config.service";

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  title:string =""; 
  isFullScreen!:boolean;
  today:any = Date.now();
  timer:any;

  constructor(private environment: AppConfigService) { 
    this.title = environment.appConfig.title;
  }

  ngOnInit() {
    this.isFullScreen = !!document.fullscreenElement; 
    this.timer = setInterval(()=> {
      this.today = Date.now();
    },1000);
  }
  onScreenChange() {
    this.isFullScreen = !!document.fullscreenElement;
    console.log(this.isFullScreen);
    if (this.isFullScreen) {
      document.exitFullscreen();
      this.isFullScreen = false;
    }
    else {
      document.documentElement.requestFullscreen();
      this.isFullScreen = true
    }
  }

  ngOnDestroy() {
    clearInterval(this.timer);
  }

}
