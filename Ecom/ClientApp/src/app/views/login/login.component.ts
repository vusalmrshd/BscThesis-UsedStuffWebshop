import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/shared/services/api.service';
import { AuthService } from 'src/app/shared/services/auth.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loginRoute:string = '/auth/login';
  registerRoute:string = '/auth/register';
  forgetPasswordRoute:string='/auth/forgetPassword';
  
  authForm:any;
  submitted:boolean=false;
  disableButton:boolean=false;
  
  showAlert:boolean=false;
  errorAlert:boolean=false;
  successAlert:boolean=false;
  alertMessage:string=''

  constructor(
    private _fb:FormBuilder,private _router:Router,private _apiService:ApiService,private _authService:AuthService,private _toastr:ToastrService
  ) { }

  ngOnInit(): void {
    this.buildForm();
  }

  buildForm(){
    this.authForm = this._fb.group({      
      userNameOrEmail:['',Validators.required],
      password:['',Validators.required]
    })
  }

 
  register(){
    this.hideAlert();
    this.submitted=true;
    if(!this.authForm.valid){
      return;
    }
    else{
      this.hideAlert();
      
      this.disableButton=true;
      this._apiService.Post('auth','login',this.authForm.value).subscribe((result:any)=>{
        if(result.success){
            this._authService.saveUser(result.data,result.data.token);
            this._router.navigateByUrl('')
        }
        else {
          this.disableButton=false;
          alert(result.message);
        }
      },err =>{
        alert('Connection Problem');
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
