import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { NotificationService } from '../services/notification.service';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private auth:AuthService,private router: Router,private notificationService:NotificationService){}
  canActivate():boolean{
    if(this.auth.isLoggedIn()){
      return true;
    }else{
      this.notificationService.showError("<hr> Please Login First!","Error!");
      this.router.navigate(['userlogin']);
      return false;
    }
  }

}
