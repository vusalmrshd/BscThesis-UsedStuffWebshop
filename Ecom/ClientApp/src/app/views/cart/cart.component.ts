import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/shared/services/api.service';
import { AuthService } from 'src/app/shared/services/auth.service';
import { CartService } from 'src/app/shared/services/cart.service';
declare var $:any;
@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  pager:number = 1;
  cartProducts:any=[];
  totalBill:number=0;

  form:any;
  submitted:boolean=false;
  user:any;

  constructor(private _router:Router,private _cartService:CartService,private _fb:FormBuilder,private _authService:AuthService,private _toastr:ToastrService,private _apiService:ApiService) {
    this.cartProducts=this._cartService.getCartProducts();
    this.totalBill=0;
    this.cartProducts.forEach((product:any)=>{
      this.totalBill = this.totalBill + product.total;
    })
    this.user = this._authService.getUser();
   }

  ngOnInit(): void {
   this.builCheckoutForm();
  }

  builCheckoutForm(){
    this.form=this._fb.group({
      name:[(this.user !== null && this.user !== undefined)?this.user.name : '',Validators.required],
      email:[(this.user !== null && this.user !== undefined) ? this.user.email : '',Validators.required],
      cnum:['',Validators.required],
      em:['',Validators.required],
      ey:['',Validators.required],
      cv:['',Validators.required],
      address:['',Validators.required]
    });
  }

  removeItemFromCart(product:any){
    this.cartProducts = this._cartService.removeProductFromCart(product)
    this.totalBill=0;
    this.cartProducts.forEach((product:any)=>{
      this.totalBill = this.totalBill + product.total;
    })
  }

  plusCart(product:any){
    this.cartProducts= this._cartService.productQtyFromCart(product,true);
    this.totalBill=0;
    this.cartProducts.forEach((product:any)=>{
      this.totalBill = this.totalBill + product.total;
    })
  }

  minusCart(product:any){
    this.cartProducts= this._cartService.productQtyFromCart(product,false);
    this.totalBill=0;
    this.cartProducts.forEach((product:any)=>{
      this.totalBill = this.totalBill + product.total;
    })
  }

  checkout(){
    this.totalBill=0;
    this.cartProducts.forEach((product:any)=>{
      this.totalBill = this.totalBill + product.total;
    });
    if(this._authService.islogin()){
      $("#checkout").modal('show');
    }
    else{
      this._router.navigate(["auth"])
    }
    
  }

  placeOrder(){
    this.submitted=true;
    if(!this.form.valid) return;

    let dataToInsert = {
      sellerId:this.cartProducts[0].sellerId,
      customerId:this.user.userId,
      customerName:this.form.controls['name'].value,
      customerEmail:this.form.controls['email'].value,
      customerAddress:this.form.controls['address'].value,
      cardNumber:this.form.controls['cnum'].value,
      expMonth:this.form.controls['em'].value,
      expYear:this.form.controls['ey'].value,
      cvc:this.form.controls['cv'].value,
      orderPrice:this.totalBill,
      orderProducts:this.cartProducts
    }
    console.log(dataToInsert)
  
    this._apiService.Post('order','placeOrder',dataToInsert).subscribe((response:any)=>{
      if(response.success){
        this.form.reset();
        $("#checkout").modal('hide');
        this._cartService.clearCart();
        this.cartProducts=[];
      }
      else{
        alert(response.message)
      }
    },err=>{
      alert('Connection problem');
    })

  }

}
