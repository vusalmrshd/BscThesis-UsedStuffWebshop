import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/shared/services/api.service';
import { CartService } from 'src/app/shared/services/cart.service';
import { FilterTablePipe } from '../../shared/pipe/filter.pipe';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  products:any[]=[];
  categories:any[]=[];
  discountedProducts:any[]=[];
  page:number = 1;
  searchName:string='';
  discountPage:number=1;
  discountSearchName:string='';
  

  constructor(private _cartService:CartService,private _toastr:ToastrService,private _apiService:ApiService) { }

  ngOnInit(): void {
      this.getProducts();
      this.getCategories();
      this.getDiscountedProducts();
  }

  applyFilter(){}

  addToCart(product:any){
    this._cartService.addProductToCart(product,1);
    alert(`${product.name} added to cart`);
  }

  addToCartDiscount(product:any){
    this._cartService.addDiscountProductToCart(product,1);
    alert(`${product.name} added to cart`);
  }

  getProducts(){
    this._apiService.Get('product','getAllProducts',``).subscribe((result:any)=>{
      if(result.success){
        this.products= [];
        this.products = result.data;

      }
    })
  }

  getProductByCat(event:any){
    if(`${event.target.value}` !== '0'){
      this._apiService.Get('product','getProductByCat',`?catId=${event.target.value}`).subscribe((result:any)=>{
        if(result.success){
          console.log(result.data)
          this.products= [];
          this.products = result.data;
  
        }
      })
    }
    else this.getProducts();
   
  }

  getDisountProductByCat(event:any){
    if(`${event.target.value}` !== '0'){
      this._apiService.Get('product','getDiscountedProductByCat',`?catId=${event.target.value}`).subscribe((result:any)=>{
        if(result.success){
          console.log(result.data)
          this.discountedProducts= [];
          this.discountedProducts = result.data;
  
        }
      })
    }
    else this.getDiscountedProducts();
    
  }

  getCategories(){
    this._apiService.Get('product','getCategories',``).subscribe((result:any)=>{
      if(result.success){
        this.categories= [];
        this.categories = result.data;

      }
    })
  }

  getDiscountedProducts(){
    this._apiService.Get('product','getDiscountedProducts',``).subscribe((result:any)=>{
      if(result.success){
        this.discountedProducts= [];
        this.discountedProducts = result.data;
      }
    })
  }
  

}
