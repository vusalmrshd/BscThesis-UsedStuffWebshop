import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute, Params } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from 'src/app/shared/services/api.service';
@Component({
  selector: 'app-activate-account',
  templateUrl: './activate-account.component.html',
  styleUrls: ['./activate-account.component.css']
})
export class ActivateAccountComponent implements OnInit {

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
        this.apiCallingService.
          Post('auth', 'validatePasswordLink', {
            "token": params.token,
          }).
          subscribe(
            (response: any) => {
              if (response.success) {
                this.apiCallingService.Get('auth','activateAccount',`?email=${params.email}`).subscribe((res:any)=>{
                  if(res.success) {
                    this._toastr.success('Account Activated');
                    _Router.navigateByUrl('/auth/login')
                  }
                })
              } else {
                this._toastr.error('Link may expired or having some internal server error. Please generate request again!');
                this._Router.navigate(['/forgetPassword']);
              }
            },
            error => {
              this._toastr.error('Error occured while processing your request');
            });
      }
    });
  }

  ngOnInit(): void {
 
  }
  
}
