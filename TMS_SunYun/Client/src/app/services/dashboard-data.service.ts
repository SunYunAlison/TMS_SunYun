import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardDataService {

  apiBaseUrl: string = "";

  constructor(private httpClient: HttpClient, private environment: AppConfigService) {
    this.apiBaseUrl = environment.config.apiUrl;
  }

  GetFreezerDataList() {
    return this.httpClient.get(`${this.apiBaseUrl}Dashboard/GetRealTimeTemp`);
  }

  UpdateTempbyLimit(freezerList: any) {
    return this.httpClient.post(`${this.apiBaseUrl}Dashboard/UpdateTempbyLimit`, freezerList)
  }
}
