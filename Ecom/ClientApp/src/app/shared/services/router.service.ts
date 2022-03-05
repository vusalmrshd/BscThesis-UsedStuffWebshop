import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RouterService {
  //This class if for routing navigateion
  constructor(private router:Router){}

  goToLoginPage(){
      this.router.navigateByUrl('/auth/login');
  }

  goToDashboardpage(){
      this.router.navigateByUrl('/dashboard');
  }

  
}
