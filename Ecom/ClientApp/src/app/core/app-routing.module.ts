import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminLayoutComponent } from '../layouts/admin-layout/admin-layout/admin-layout.component';
import { AuthLayoutComponent } from '../layouts/auth-layout/auth-layout/auth-layout.component';
import { AdminGuard } from '../shared/guards/admin_guard.service';
import { LoginGuard } from '../shared/guards/login_guard.service';

const routes: Routes = [
  
  {
    path:'',
    component:AdminLayoutComponent,
    loadChildren:()=>import('../layouts/admin-layout/admin-layout.module').then(m=>m.AdminLayoutModule)
  },
  {
    path:'auth',
    component:AuthLayoutComponent,
    loadChildren:()=>import('../layouts/auth-layout/auth-layout.module').then(m=>m.AuthLayoutModule),
    canActivate:[LoginGuard]
  },
  {
    path:'**',
    redirectTo:'',
    pathMatch:'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
