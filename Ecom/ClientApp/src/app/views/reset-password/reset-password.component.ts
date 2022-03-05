import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/shared/services/api.service';
declare const $: any;

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  loginRoute:string='/auth/login';

  showAlert:boolean=false;
  errorAlert:boolean=false;
  successAlert:boolean=false;
  alertMessage:string=''


  resetPasswordForm: any;
  submitted = false;
  showForm = false;
  token:any;

  constructor(
    private apiCallingService: ApiService,
    private formBuilder: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private _toastr:ToastrService,
    private _Router: Router
  ) {
    this.resetPasswordForm = FormGroup;
    this.showForm = false;
    this.activatedRoute.queryParams.subscribe((params: Params) => {
      this.token = "";
      if (params.token !== "" && params.token !== undefined && params.token !== null) {
        this.token = params.token;
        this.apiCallingService.Post('auth', 'validatePasswordLink', {"token": params.token,}).subscribe((response:any)=> {
              if (response.success) {
                this.showForm = true;
              } else {
                this._toastr.error('Link may expired or having some internal server error. Please generate request again!');
                this._Router.navigateByUrl('/auth/forgetPassword');
              }
            },
            error => {
              this._toastr.error('Error occured while processing your request');
            });
      }
    });
  }

  ngOnInit(): void {
    this.resetPasswordForm  = this.formBuilder.group({
      newPassword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
      confirmPassword: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
    }, { validator: this.MustMatch('newPassword', 'confirmPassword') });
  }
  get submittedResetPasswordForm() { return this.resetPasswordForm.controls; }
  MustMatch(controlName: string, matchingControlName: string) {
    return (formGroup: FormGroup) => {
      const control = formGroup.controls[controlName];
      const matchingControl = formGroup.controls[matchingControlName];
      if (matchingControl.errors && !matchingControl.errors.mustMatch) {
        return;
      }
      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mustMatch: true });
      } else {
        matchingControl.setErrors(null);
      }
    }
  }
  onSubmit() {
    this.hideAlert();
    this.submitted = true;
    if (this.resetPasswordForm .invalid) {return;}

    this.apiCallingService.Post('auth', 'updatePassword', {
        "password": this.submittedResetPasswordForm.newPassword.value,
        "code": this.token,
      }).subscribe((response:any) => {
          if (response.success) {
            this._toastr.success('Password is udpated successfully!');
            this.resetPasswordForm .reset();
            this.submitted = false;
            this._Router.navigateByUrl('/auth/login');
          } else {
            this._toastr.error(response.message);
          }
        },
        error => {
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
