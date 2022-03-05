import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit {
  user:any={};
  viewProucts:string = '/view-products'
  viewCart:string='/cart';
  order:string = '/order';
  cats:string = '/categories';
  login:string='/auth';

  role:string='noone';
  userName:string='Guest';
  constructor(private _authService:AuthService,private _router:Router) {
    this.user= _authService.getUser() || null;
    if(this.user !== undefined && this.user != null){
      this.role = (this.user == null || this.user === undefined) ? 'noone' : this.user.role;
      this.userName = (this.user == null || this.user === undefined) ? 'Guest' : this.user.userName;
    }
   }

  ngOnInit(): void {
  }

  checkAndLogin(){
    if(this.userName.toLowerCase() === 'guest') this._router.navigate(['auth']);
  }

  logout(){
    this._authService.logout();
  }
}
