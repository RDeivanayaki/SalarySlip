import { Component } from "@angular/core";
import { AuthService } from "src/app/services/auth.service";
import { NotificationService } from "src/app/services/notification.service";
import { Router } from "@angular/router";

@Component({
  selector:'app-userlogin',
  templateUrl:'./userlogin.component.html',
  styleUrls:[]
})

export class UserLoginComponent{

  constructor(private authService: AuthService, private notificationService : NotificationService,private route:Router){}

  userName:string="";
  password:string="";
  date:Date = new Date();

  onLogin(){

    //console.warn(this.date);
    var val={
      "userId":0,
      "userName":this.userName,
      "password":this.password,
      "userRole":"",
      "token":""
    }
    this.authService.onLogin(val).subscribe({
      next: (res:any) => {
        //console.log(res.Token);
        //if(res.Message)
        this.authService.storeToken(res.Token);
          //this.notificationService.showSuccess("<hr>" + res.Message,"Success!");
          this.route.navigate(['salaryslip']);
      },
      error: (err) => {
        //console.warn(err.error);
        this.notificationService.showError("<hr>" + err.error,"Error!");
      },
  });
  }

}
