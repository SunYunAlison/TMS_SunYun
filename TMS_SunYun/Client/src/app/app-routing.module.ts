import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';

import { HomeComponent } from './home/home.component';
import { ReportComponent } from './report/report.component';
import { SettingComponent } from "./setting/setting.component";
import { CmcComponent } from "./cmc/cmc.component";
import { AlarmComponent } from "./alarm/alarm.component";

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'dashboard', component: DashboardComponent, loadChildren: () => import('src/app/dashboard/dashboard.module').then(x => x.DashboardModule) },
  { path: 'report', component: ReportComponent, loadChildren: () => import('src/app/report/report.module').then(x => x.ReportModule) },
  { path: 'setting', component: SettingComponent, loadChildren: () => import('src/app/setting/setting.module').then(x => x.SettingModule) },
  { path: 'cmc', component: CmcComponent, loadChildren: () => import('src/app/cmc/cmc.module').then(x => x.CmcModule) },
  { path: 'alarm', component: AlarmComponent, loadChildren: () => import('src/app/alarm/alarm.module').then(x => x.AlarmModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
