import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutModule } from "../layout/layout.module";
import { SettingComponent } from './setting.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';

@NgModule({
  declarations: [
    SettingComponent
  ],
  imports: [
    CommonModule,
    LayoutModule,
    MatFormFieldModule,
    MatInputModule
  ],
  exports: [
    SettingComponent,
  ]
})
export class SettingModule { }
