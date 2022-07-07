import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { HttpClientModule } from '@angular/common/http';
import { MatDialogModule } from "@angular/material/dialog";

import { HeaderComponent } from './header/header.component';
import { NavbarComponent } from './navbar/navbar.component';
import { PopAlertComponent } from './pop-alert/pop-alert.component';
import {RouterModule} from '@angular/router';


@NgModule({
  declarations: [
    HeaderComponent,
    NavbarComponent,
    PopAlertComponent
  ],
  imports: [
    RouterModule,
    CommonModule,
    MatButtonModule,
    HttpClientModule,
    MatDialogModule
  ],
  exports: [
    HeaderComponent,
    NavbarComponent,
    PopAlertComponent
  ]
})
export class LayoutModule { }
