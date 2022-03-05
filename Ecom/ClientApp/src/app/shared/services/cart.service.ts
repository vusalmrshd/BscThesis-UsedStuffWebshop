import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  constructor(private router:Router) { }

  addProductToCart(product:any,quantity:number){
    let prods=[];
    prods = JSON.parse(localStorage.getItem('cart') || '[]');
    let isExists = false;
    if(prods.length > 0){
        prods.forEach((prod:any) => {
            if(prod.productId === product.productId) {
                prod.quantity = prod.quantity + quantity;
                isExists=true;
            }
        });
        if(!isExists){
            let productToAdd = {
                productId:product.productId,
                name:product.name,
                price:product.price,
                quantity:quantity,
                image:product.image,
                sellerId:product.sellerId
            };
            prods.push(productToAdd);
        }
    
        localStorage.setItem('cart',JSON.stringify(prods));
        return prods;
    }
    else{
      let productToAdd = {
          productId:product.productId,
          name:product.name,
          price:product.price,
          quantity:quantity,
          image:product.image,
          sellerId:product.sellerId
      };
      prods.push(productToAdd);

      localStorage.setItem('cart',JSON.stringify(prods));
    }
    prods.forEach((prod:any)=>{
      prod.total = prod.quantity * prod.price;
    });
    return prods;
  }

  addDiscountProductToCart(product:any,quantity:number){
    let prods=[];
    prods = JSON.parse(localStorage.getItem('cart') || '[]');
    let isExists = false;
    if(prods.length > 0){
        prods.forEach((prod:any) => {
            if(prod.productId === product.productId) {
                prod.quantity = prod.quantity + quantity;
                isExists=true;
            }
        });
        if(!isExists){
            let productToAdd = {
                productId:product.productId,
                name:product.name,
                price:product.discountPrice,
                quantity:quantity,
                image:product.image,
                sellerId:product.sellerId
            };
            prods.push(productToAdd);
        }
    
        localStorage.setItem('cart',JSON.stringify(prods));
        return prods;
    }
    else{
      let productToAdd = {
          productId:product.productId,
          name:product.name,
          price:product.discountPrice,
          quantity:quantity,
          image:product.image,
          sellerId:product.sellerId
      };
      prods.push(productToAdd);

      localStorage.setItem('cart',JSON.stringify(prods));
    }
    prods.forEach((prod:any)=>{
      prod.total = prod.quantity * prod.price;
    });
    return prods;
  }

  getCartProducts(){
    var prods =  JSON.parse(localStorage.getItem('cart') || '[]');
    prods.forEach((prod:any)=>{
      prod.total = prod.quantity * prod.price;
    });
    return prods;
  }

  removeProductFromCart(product:any){
    var prods = JSON.parse(localStorage.getItem('cart') || '[]');
    let index=0;
    prods.forEach((prod:any)=>{
        if(prod.productId === product.productId){
            prods.splice(index,1);
        }
        index++
    })
    localStorage.setItem('cart',JSON.stringify(prods));
    prods.forEach((prod:any)=>{
      prod.total = prod.quantity * prod.price;
    });
    return prods;
  }

  productQtyFromCart(product:any,isAdd:boolean){
    var prods = JSON.parse(localStorage.getItem('cart') || '[]');
    prods.forEach((prod:any)=>{
        if(prod.productId === product.productId){
            if(isAdd) prod.quantity +=1;
            else{
                if(prod.quantity - 1 > 0) prod.quantity -=1;
            }
        }
    })
    localStorage.setItem('cart',JSON.stringify(prods));
    prods.forEach((prod:any)=>{
      prod.total = prod.quantity * prod.price;
    });
    return prods;

  }

  clearCart(){
    localStorage.removeItem('cart');
  }

}
