import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiService } from 'src/app/shared/services/api.service';
declare var $:any;
@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
  categories:any[]=[]
  form:any;
  submitted:boolean=false;
  page:number=1;
  constructor(private _apiService:ApiService,private _fb:FormBuilder) { }

  ngOnInit(): void {
    this.buildForm();
    this.getCategories();
  }

  buildForm(){
    this.form = this._fb.group({
      id:[''],
      name:['',Validators.required]
    })
  }

  getCategories(){
    this._apiService.Get('product','getCategories','').subscribe((response:any)=>{
      console.log(response)
      if(response.success){
        this.categories=[];
        this.categories=response.data;
      }
    })
  }

  deleteCategory(id:any){
    this._apiService.Get('product','deleteCategory',`?id=${id}`).subscribe((response:any)=>{
      if(response.success){
        alert('Deleted');
        this.getCategories();
      }
    })
  }

  insertCategoriy(){
    this.submitted=true;
    if(!this.form.valid) return;
    console.log(this.form.value)
    let data={
      id:0,
      name:this.form.controls['name'].value
    }
    this._apiService.Post('product','insertCategory',data).subscribe((response:any)=>{
      if(response.success){
        alert('Inserted');
        $("#cat").modal('hide');
        this.form.reset();
        this.submitted=false;
        this.getCategories();
      }
    })
  }

  openModal(){
    $("#cat").modal('show');
  }



}
