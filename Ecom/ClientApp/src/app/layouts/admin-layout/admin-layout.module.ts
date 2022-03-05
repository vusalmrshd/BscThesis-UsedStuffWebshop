import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule,ReactiveFormsModule } from '@angular/forms';

import { AdminLayoutRoutingModule } from './admin-layout-routing.module';
import { AdminLayoutComponent } from './admin-layout/admin-layout.component';
import { DashboardComponent } from 'src/app/views/dashboard/dashboard.component';
import { ViewProductsComponent } from 'src/app/views/view-products/view-products.component';

import {NgxPaginationModule} from 'ngx-pagination'; 
import { FilterTablePipe } from 'src/app/shared/pipe/filter.pipe';
import { CartComponent } from 'src/app/views/cart/cart.component';
import { ToastrModule } from 'ngx-toastr';
import { OrdersComponent } from 'src/app/views/orders/orders.component';
import { CategoriesComponent } from 'src/app/views/categories/categories.component';



@NgModule({
  declarations: [
    FilterTablePipe,
    AdminLayoutComponent,
    DashboardComponent,
    ViewProductsComponent,
    OrdersComponent,
    CartComponent,
    CategoriesComponent
    
  ],
  imports: [
    CommonModule,
    ToastrModule.forRoot(),
    AdminLayoutRoutingModule,
    NgxPaginationModule,
    ReactiveFormsModule,
    FormsModule,
    
  ]
})
export class AdminLayoutModule { }
