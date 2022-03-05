import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/shared/services/api.service';
import { AuthService } from 'src/app/shared/services/auth.service';
declare var $:any;
@Component({
  selector: 'app-view-products',
  templateUrl: './view-products.component.html',
  styleUrls: ['./view-products.component.scss']
})
export class ViewProductsComponent implements OnInit {

  user:any;
  page:number = 1;

  productToDelete:any={};
  productToEdit:any={};
  categories:any[]=[];

  editFormOpen:boolean=false;
  form:any;
  submitted:boolean=false;

  addFormOpen:boolean=false;
  addForm:any;
  addSubmitted:boolean=false;

  isDiscount:boolean=false;


  products:any[]=[
    {
      productId:1,
      rowId:1,
      name:'Burger',
      price:55.6,
      image:'https://images.unsplash.com/photo-1571091718767-18b5b1457add?ixid=MnwxMjA3fDB8MHxzZWFyY2h8Nnx8YnVyZ2VyfGVufDB8fDB8fA%3D%3D&ixlib=rb-1.2.1&w=1000&q=80',
    },
    {
      productId:2,
      rowId:2,
      name:'Cheese Burger',
      price:50.6,
      image:'https://images.unsplash.com/photo-1571091718767-18b5b1457add?ixid=MnwxMjA3fDB8MHxzZWFyY2h8Nnx8YnVyZ2VyfGVufDB8fDB8fA%3D%3D&ixlib=rb-1.2.1&w=1000&q=80',
    },
    {
      productId:3,
      rowId:3,
      name:'Peace Burger',
      price:59.6,
      image:'https://images.unsplash.com/photo-1571091718767-18b5b1457add?ixid=MnwxMjA3fDB8MHxzZWFyY2h8Nnx8YnVyZ2VyfGVufDB8fDB8fA%3D%3D&ixlib=rb-1.2.1&w=1000&q=80',
    },
    {
      productId:4,
      rowId:4,
      name:'Simple Burger',
      price:55.6,
      image:'https://images.unsplash.com/photo-1571091718767-18b5b1457add?ixid=MnwxMjA3fDB8MHxzZWFyY2h8Nnx8YnVyZ2VyfGVufDB8fDB8fA%3D%3D&ixlib=rb-1.2.1&w=1000&q=80',
    },
    {
      productId:5,
      rowId:5,
      name:'Spicy Burger',
      price:55.6,
      image:'https://images.unsplash.com/photo-1571091718767-18b5b1457add?ixid=MnwxMjA3fDB8MHxzZWFyY2h8Nnx8YnVyZ2VyfGVufDB8fDB8fA%3D%3D&ixlib=rb-1.2.1&w=1000&q=80',
    },
    {
      productId:6,
      rowId:6,
      name:'Tasty Burger',
      price:105.6,
      image:'https://images.unsplash.com/photo-1571091718767-18b5b1457add?ixid=MnwxMjA3fDB8MHxzZWFyY2h8Nnx8YnVyZ2VyfGVufDB8fDB8fA%3D%3D&ixlib=rb-1.2.1&w=1000&q=80',
    },
    {
      productId:7,
      rowId:7,
      name:'Pizza',
      price:100.6,
      image:'https://images.unsplash.com/photo-1571091718767-18b5b1457add?ixid=MnwxMjA3fDB8MHxzZWFyY2h8Nnx8YnVyZ2VyfGVufDB8fDB8fA%3D%3D&ixlib=rb-1.2.1&w=1000&q=80',
    }
  ]
  constructor(private _fb:FormBuilder,private _apiService:ApiService,private _authService:AuthService,private _toastr:ToastrService) {
    this.user = _authService.getUser();
   }

  ngOnInit(): void {
    this.getCategories();
    this.getProducts();
  }

  getProducts(){
    this._apiService.Get('product','getSellerProducts',`?sellerId=${this.user.userId}`).subscribe((result:any)=>{
      if(result.success){
        this.products= [];
        this.products = result.data;
      }
    })
  }

  deleteProduct(product:any){
    this.productToDelete=product;
    $("#confirmModal").modal('show');
  }

  confirmDelete(){
    $("#confirmModal").modal('hide');
    console.log(this.productToDelete)
    this._apiService.Get('product',`deleteProduct?productId=${this.productToDelete.productId}`,'').subscribe((response:any)=>{
      if(response.success){
        this._toastr.success(response.message || 'Product Deleted');
        this.getProducts();
      }
      else{
        this._toastr.error(response.message || 'Error in deleting Product');
      }
    },err =>{
      this._toastr.error('Connection problem');
    })
  }

  editProduct(product:any){
    this.editFormOpen=true;
    this.buildEditForm(product);
    this.productToEdit=product;
    $("#editModal").modal('show');
  }

  buildEditForm(product:any){
    this.form=this._fb.group({
      productId:[product.productId,Validators.required],
      name:[product.name,Validators.required],
      price:[product.price,Validators.required],
      discountPrice:[product.discountPrice],
      isDiscount:[false],
      comment:[product.comment],
      condition:[product.condition],
      location:[product.location],
      image:[''],
      catId:[product.categoryId,Validators.required],
      imageSource:['']
    });
  }

  onFileChange(event:any){
    if(event.target.files && event.target.files.length > 0){
      this.form.patchValue({imageSource:event.target.files[0]});
    }
  }

  update(){
    this.submitted=true;
    if(!this.form.valid)return;
    const formData = new FormData();
    formData.append('name',this.form.controls['name'].value);
    formData.append('price',this.form.controls['price'].value);
    formData.append('image',this.form.controls['imageSource'].value);
    formData.append('productId',this.form.controls['productId'].value);
    formData.append('isDiscount',this.form.controls['isDiscount'].value);
    formData.append('comment',this.form.controls['comment'].value);
    formData.append('location',this.form.controls['location'].value);
    formData.append('condition',this.form.controls['condition'].value);
    formData.append('discountPrice',this.form.controls['discountPrice'].value);
    formData.append('catId',this.form.controls['catId'].value);
    formData.append('sellerId',this.user.userId);

    this._apiService.Put('product','updateProduct',formData).subscribe((result:any)=>{
      if(result.success){
        $("#editModal").modal('hide');
        this._toastr.success('Product updated successfully');
        this.products=[];
        this.getProducts();
        this.form.reset();
        this.addFormOpen=false;
        this.submitted=false;
      }
      else{
        this._toastr.error(result.message);
      }
    },err =>{
      this._toastr.error('Connection problem');
    })
     
  }

  buildAddForm(){
    this.addForm=this._fb.group({
      productId:[''],
      name:['',Validators.required],
      price:[0,Validators.required],
      comment:['',Validators.required],
      condition:['',Validators.required],
      location:['',Validators.required],
      image:['',Validators.required],
      catId:['',Validators.required],
      imageSource:['']
    });
  }

  onAddProductFileChange(event:any){
    if(event.target.files && event.target.files.length > 0){
      this.addForm.patchValue({imageSource:event.target.files[0]});
    }
  }

  addProduct(){
    $("#addModal").modal('show');
    this.addFormOpen=true;
    this.buildAddForm();
  }

  discount(event:any){
    this.isDiscount = event.currentTarget.checked;

  }

  getCategories(){
    this._apiService.Get('product','getCategories','').subscribe((response:any)=>{
      if(response.success){
        this.categories=[];
        this.categories=response.data;
      }
    })
  }
  

  insertProduct(){
    
    this.addSubmitted=true;
    if(!this.addForm.valid) return;
    else{
      $("#addModal").modal('hide');
      const formData = new FormData();
      formData.append('name',this.addForm.controls['name'].value);
      formData.append('price',this.addForm.controls['price'].value);
      formData.append('image',this.addForm.controls['imageSource'].value);
      formData.append('location',this.addForm.controls['location'].value);
      formData.append('condition',this.addForm.controls['condition'].value);
      formData.append('catId',this.addForm.controls['catId'].value);
      formData.append('comment',this.addForm.controls['comment'].value);
      formData.append('sellerId',this.user.userId);
      this._apiService.Post('product','addNewProduct',formData).subscribe((result:any)=>{
        if(result.success){
          this._toastr.success('Product inserted successfully');
          this.products=[];
          this.getProducts();
          this.addForm.reset();
          this.addFormOpen=false;
          this.addSubmitted=false;
        }
        else{
          this._toastr.error(result.message);
        }
      },err =>{
        this._toastr.error('Connection problem');
      })
    }
  }
}
