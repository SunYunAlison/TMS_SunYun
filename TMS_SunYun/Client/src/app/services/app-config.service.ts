import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from 'rxjs';

@Injectable()
export class AppConfigService {
  appConfig: any;
  configUrl = "./config/app-config.json";

  constructor(private http: HttpClient) {
    this.getJson().subscribe(data => {
      this.appConfig = data;
    });
  }

  public getJson(): Observable<any> {
    return this.http.get(this.configUrl);
  }

  get config() {
    let urls = this.appConfig;
    return urls;
  }

}
