import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CartComponent } from 'src/app/views/cart/cart.component';
import { DashboardComponent } from 'src/app/views/dashboard/dashboard.component';
import { OrdersComponent } from 'src/app/views/orders/orders.component';
import { ViewProductsComponent } from 'src/app/views/view-products/view-products.component';
import { CategoriesComponent } from 'src/app/views/categories/categories.component';
import { AdminGuard } from 'src/app/shared/guards/admin_guard.service';

const routes: Routes = [
  {
    path:'',
    redirectTo:'dashboard',
    pathMatch:'full'
  },
  {path:'dashboard',component:DashboardComponent},
  {path:'view-products',component:ViewProductsComponent,canActivate:[AdminGuard]},
  {path:'cart',component:CartComponent},
  {path:'order',component:OrdersComponent,canActivate:[AdminGuard]},
  {path:'categories',component:CategoriesComponent,canActivate:[AdminGuard]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminLayoutRoutingModule { }
