import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class SettingDataService {

  apiBaseUrl: string = "";

  constructor(private httpClient: HttpClient, private environment: AppConfigService) {
    this.apiBaseUrl = environment.config.apiUrl;
  }

  GetRealTimeStatusAndOffset() {
    return this.httpClient.get(`${this.apiBaseUrl}Setting/GetRealTimeStatusAndOffset`);
  }

  UpdateStatusAndOffset(freezerList: any) {
    return this.httpClient.post(`${this.apiBaseUrl}Setting/UpdateStatusAndOffset`, freezerList)
  }
  
}
