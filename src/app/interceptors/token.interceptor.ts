import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { throwError,catchError, Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { NotificationService } from '../services/notification.service';
import { Router } from '@angular/router';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private auth:AuthService, private notificationService:NotificationService, private route:Router) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const myToken = this.auth.getToken();
    //console.log("myToken : " + myToken);
    if(myToken)
    {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${myToken}` //"Bearer " + myToken,
        }
      });
    }
    return next.handle(request); /*.pipe(
      catchError((err:any) => {
        if(err instanceof HttpErrorResponse)
        {
          if(err.status === 401){
            this.notificationService.showWarning("<hr> Token is expired, Please Login again","Warning");
            this.route.navigate(['userlogin']);
          }
        }

        //return throwError(() => new Error("Some other errors occured"))
      }
     )
    );*/
  }
}
