import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private router:Router) { }

   islogin():boolean{
    if (localStorage.getItem('jwt') != null) return true;
    else return false;
   }

   getToken():string{
     if(localStorage.getItem('jwt') != null)
      return 'Bearer ' +localStorage.getItem('jwt') || '{}';
    else
      return '';
   }

   logout(){
     localStorage.clear();
     this.router.navigateByUrl('/auth')
     return true;
   }
   saveUser(data:any,token:any):boolean{
     localStorage.setItem('jwt',token);
     localStorage.setItem('user',JSON.stringify(data));
     return true;
   }
   
   getUser():any{
     return JSON.parse(localStorage.getItem('user') || 'null');
   }
   getUserId():string{
     var user=JSON.parse(localStorage.getItem('user') || '{}');
     return user.userId;
   }
  

}
