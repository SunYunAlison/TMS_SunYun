import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfigService } from './app-config.service';

@Injectable({
  providedIn: 'root'
})
export class CmcDataService {

  apiBaseUrl: string = "";

  constructor(private httpClient: HttpClient, private environment: AppConfigService) {
    this.apiBaseUrl = environment.config.apiUrl;
  }

  GetSMSRecords() {
    return this.httpClient.get(`${this.apiBaseUrl}message/GetSMSRecords`);
  }

  GetMessageRecords() {
    return this.httpClient.get(`${this.apiBaseUrl}message/GetMessageRecords`);
  }

  GetCMCCurrentVersion() {
    return this.httpClient.get(`${this.apiBaseUrl}message/GetCMCCurrentVersion`);
  }

  GetAllUsers() {
    return this.httpClient.get(`${this.apiBaseUrl}Authentication/GetAllUsers`);
  }

  NewAccount(newUser: any) {
    return this.httpClient.post(this.apiBaseUrl + "Authentication/NewAccount", newUser);
  }

  UpdateAccountInfo(updateUserInfo: any) {
    return this.httpClient.post(this.apiBaseUrl + "Authentication/UpdateAccountInfo", updateUserInfo);
  }

  DeleteAccount(user: any) {
    return this.httpClient.post(this.apiBaseUrl + "Authentication/DeleteAccount", user, { observe: 'response' });
  }
}
