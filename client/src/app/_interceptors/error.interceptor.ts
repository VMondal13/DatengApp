import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NavigationExtras, Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError(
        (error: HttpErrorResponse) => { 
          if(error){
              switch(error.status){
                case 400:
                  if(error.error.errors){
                    const modelstateError = [];
                    for(const key in error.error.errors){
                      if(error.error.errors[key]){
                          modelstateError.push(error.error.errors[key]);
                      }                       
                    }
                    throw modelstateError.flat();
                  } else {
                    this.toastr.error(error.error, error.status.toString());                    
                  }
                  break;
                case 401:
                  this.toastr.error('Unauthorised', error.status.toString());    
                  break;                
                case 404:
                  this.router.navigateByUrl('/not-found')
                  break;
                case 500:
                  const navigationsExtras: NavigationExtras = {state: {error: error.error}}
                  this.router.navigateByUrl('/server-error', navigationsExtras)
                  break;  
                default:
                  this.toastr.error('Something unexpected occurred.');
                  console.log(error);
                  break;
              }}
              throw error;
        })
    );
  }
}
