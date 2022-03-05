import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';



@Injectable()
export class Interceptor implements HttpInterceptor {

  constructor(private authservice:AuthService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      if (this.authservice.islogin()) {
        const cloned = req.clone({
           headers: req.headers.set("Authorization",this.authservice.getToken())
          });
        return next.handle(cloned);
      }
      else return next.handle(req);

  }
}