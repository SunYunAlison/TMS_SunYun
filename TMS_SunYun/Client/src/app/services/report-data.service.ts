import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class ReportDataService {
  apiBaseUrl: string = "";

  constructor(private httpClient: HttpClient, private environment: AppConfigService) {
    this.apiBaseUrl = environment.config.apiUrl;
  }

  GetFreezerList() {
    return this.httpClient.get(`${this.apiBaseUrl}Report/GetFridgeList`);
  }

  GetTempbyMin(fridgeId:string,startTime:string,endTime:string) {
    return this.httpClient.get(`${this.apiBaseUrl}Report/GetTempbyMin?fridgeId=${fridgeId}&startTime=${startTime}&endTime=${endTime}`, { observe: 'response' });
  }

  GetTempbyHour(fridgeId:string,startTime:string,endTime:string) {
    return this.httpClient.get(`${this.apiBaseUrl}Report/GetTempbyHour?fridgeId=${fridgeId}&startTime=${startTime}&endTime=${endTime}`, { observe: 'response' });
  }

  DownloadTempbyMin(fridgeId:string,startTime:string,endTime:string) {
    return this.httpClient.get(`${this.apiBaseUrl}Report/DownloadTempbyMin?fridgeId=${fridgeId}&startTime=${startTime}&endTime=${endTime}`, { observe: 'response' });
  }

  DownloadTempbyHour(fridgeId:string,startTime:string,endTime:string) {
    return this.httpClient.get(`${this.apiBaseUrl}Report/DownloadTempbyHour?fridgeId=${fridgeId}&startTime=${startTime}&endTime=${endTime}`, { observe: 'response' });
  }
  
}
