import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/shared/services/api.service';
import { AuthService } from 'src/app/shared/services/auth.service';
declare var $:any;
@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  user:any;
  orders:any[]=[];
  page:number=1;
  selectedOrder:any={};
  constructor(private _apiService:ApiService,private _authService:AuthService) { 
    this.user = _authService.getUser();
  }

  ngOnInit(): void {
    this.getOrders();
  }

  getOrders(){
    let api = this.user.role.toLowerCase() === 'seller' ? `getSellerOrders` : `getCustomerOrders`;
    this._apiService.Get('order',api,`?userId=${this.user.userId}`).subscribe((response:any)=>{
      if(response.success){
        console.log(response.data);
        this.orders = response.data;
      }
    })
  }

  deleteOrder(orderId: any) {
    this._apiService.Get('order', `deleteOrder`, `?orderId=${orderId}`).subscribe((response: any) => {
      if (response.success) {
        console.log(response.data);
        this.getOrders();
      }
    })
  }

  selectOrder(order:any){
    this.selectedOrder=order;
    $("#checkout").modal('show');
  }

}
