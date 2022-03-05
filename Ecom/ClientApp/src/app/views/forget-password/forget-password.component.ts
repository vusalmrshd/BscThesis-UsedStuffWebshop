import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/shared/services/api.service';

declare const $: any;

@Component({
  selector: 'app-forget-password',
  templateUrl: './forget-password.component.html',
  styleUrls: ['./forget-password.component.css']
})
export class ForgetPasswordComponent implements OnInit {

  forgotPasswordForm: any;
  submitted = false;

  loginRoute:string='/auth/login';

  showAlert:boolean=false;
  errorAlert:boolean=false;
  successAlert:boolean=false;
  alertMessage:string=''

  constructor(
    private apiCallingService: ApiService,
    private _toastr:ToastrService,
    private formBuilder: FormBuilder
  ) {
    this.forgotPasswordForm = FormGroup;
  }

  ngOnInit(): void {
    this.forgotPasswordForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }
  get submittedForgotPasswordForm() { return this.forgotPasswordForm.controls; }


  onSubmit() {
    this.hideAlert();
    this.submitted = true;
    if (this.forgotPasswordForm.invalid) {return;}

    this.apiCallingService.Post('auth', 'forgetPassword', {
        "email": this.submittedForgotPasswordForm.email.value,
        "link": document.getElementsByTagName('base')[0].href
      }).subscribe((response:any) => {
          if (response.success) {
            this._toastr.success('Email is sent. Please follow the instruction mentioned on email!');
            this.forgotPasswordForm.reset();
            this.submitted = false;
          } else {
            this._toastr.error(response.message);
          }
       
        },error => {
          this._toastr.error('Error occured while processing your request');
        });
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
