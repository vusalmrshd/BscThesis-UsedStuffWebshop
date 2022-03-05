import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ActivateAccountComponent } from 'src/app/views/activate-account/activate-account.component';
import { ForgetPasswordComponent } from 'src/app/views/forget-password/forget-password.component';
import { LoginComponent } from 'src/app/views/login/login.component';
import { RegisterComponent } from 'src/app/views/register/register.component';
import { ResetPasswordComponent } from 'src/app/views/reset-password/reset-password.component';

const routes: Routes = [
  { path:'',redirectTo:'login',pathMatch:'full'},
  { path:'login',component:LoginComponent},
  { path:'register',component:RegisterComponent},
  { path:'forgetPassword', component: ForgetPasswordComponent},
  { path:'resetPassword', component: ResetPasswordComponent},
  { path:'activateAccount', component: ActivateAccountComponent}, 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthLayoutRoutingModule { }
