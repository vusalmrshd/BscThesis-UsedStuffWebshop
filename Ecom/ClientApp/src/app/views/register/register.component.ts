import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/shared/services/api.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  loginRoute:string = '/auth/login';
  registerRoute:string = '/auth/register';
  
  authForm:any;
  submitted:boolean=false;
  disableButton:boolean=false;
  
  showAlert:boolean=false;
  errorAlert:boolean=false;
  successAlert:boolean=false;
  alertMessage:string=''

  constructor(
    private _fb:FormBuilder,private _router:Router,private _apiService:ApiService,
    private _toastr:ToastrService
    ) { }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm(){
    this.authForm = this._fb.group({      
      name:['',Validators.required],
      email:['',Validators.required],
      password:['',Validators.required],
      status:['1',Validators.required],
      userName:['',Validators.required],
      image:['',Validators.required],
      imageSource:['']
      
    })
  }

  onFileChange(event:any){
    
    if(event.target.files && event.target.files.length){
      this.authForm.patchValue({imageSource:event.target.files[0]});
    }
  }

  register(){
    this.hideAlert();
    this.submitted=true;
    if(!this.authForm.valid){
      return;
    }
    else{
      this.hideAlert();
      const formData = new FormData();
      formData.append('name',this.authForm.controls['name'].value);
      formData.append('email',this.authForm.controls['email'].value);
      formData.append('password',this.authForm.controls['password'].value);
      formData.append('userName',this.authForm.controls['userName'].value);
      formData.append('status',this.authForm.controls['status'].value);
      formData.append('image',this.authForm.controls['imageSource'].value);
      formData.append('link',document.getElementsByTagName('base')[0].href);

      this.disableButton=true;
      this._apiService.Post('auth','register',formData).subscribe((result:any)=>{
        if(result.success){
          alert('Registered successfully'); 
            this._router.navigateByUrl('/auth/login')
        }
        else {
          this.disableButton=false;
          this._toastr.error(result.message);
        }
      },err =>{
        this._toastr.error('Connection Problem');
        this.disableButton=false;
      })
    }
    
  }

  showSuccessAlert(message:string){
    this.showAlert=this.successAlert=true;
    this.alertMessage=message;
  }

  showErrorAlert(message:string){
    this.showAlert=this.errorAlert=true;
    this.alertMessage=message;
  }

  hideAlert(){
    this.showAlert=this.successAlert=this.errorAlert=false;
  }
  

}


