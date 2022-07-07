import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardComponent } from './dashboard.component';
import { LayoutModule } from "../layout/layout.module";
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { SettingModule } from "../setting/setting.module";


@NgModule({
  declarations: [
    DashboardComponent
  ],
  imports: [
    CommonModule,
    LayoutModule,
    MatFormFieldModule,
    MatInputModule,
    SettingModule
  ]
})
export class DashboardModule { }
