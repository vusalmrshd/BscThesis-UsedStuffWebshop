import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


import { AuthLayoutRoutingModule } from './auth-layout-routing.module';
import { AuthLayoutComponent } from './auth-layout/auth-layout.component';

import { LoginComponent } from 'src/app/views/login/login.component';
import { RegisterComponent } from 'src/app/views/register/register.component';
import { ActivateAccountComponent } from 'src/app/views/activate-account/activate-account.component';
import { ForgetPasswordComponent } from 'src/app/views/forget-password/forget-password.component';
import { ResetPasswordComponent } from 'src/app/views/reset-password/reset-password.component';
import { FilterTablePipe } from 'src/app/shared/pipe/filter.pipe';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AuthLayoutComponent,LoginComponent,RegisterComponent,ActivateAccountComponent,ForgetPasswordComponent,ResetPasswordComponent
  ],
  imports: [
    CommonModule,
    ToastrModule.forRoot(),
    FormsModule,ReactiveFormsModule,
    AuthLayoutRoutingModule
  ]
})
export class AuthLayoutModule { }
