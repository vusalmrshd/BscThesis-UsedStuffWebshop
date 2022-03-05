import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

const httpoptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json'
  })
}
@Injectable({
  providedIn: 'root'
})


export class ApiService{
    constructor(private http:HttpClient){}

    Get<T>(controller: string,action:string, options?: Object): Observable<T> {
        return this.http.get<T>(`${environment.baseUrl}/${controller}/${action}${options}`);
    }

    Post<T>(controller: string,action:string ,body?: any, options?: Object): Observable<T> {
      return this.http.post<T>(`${environment.baseUrl}/${controller}/${action}`, body, options);
    }

    Put<T>(controller: string,action:string ,body?: any, options?: Object): Observable<T> {
    return this.http.put<T>(`${environment.baseUrl}/${controller}/${action}`, body, options);
    }

    Delete<T>(controller:string,action:string,body?:any,options?:any):Observable<any>{
      return this.http.delete(`${environment.baseUrl}/${controller}/${action}${options}`);
    }

}
