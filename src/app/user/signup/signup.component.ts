import { Component } from "@angular/core";
import { AuthService } from "src/app/services/auth.service";
import { NotificationService } from "src/app/services/notification.service";

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls:[]
}
)

export class SignupComponent{

  constructor(private authService: AuthService,private notificationService: NotificationService){

  }

  userName:string="";
  password:string="";

  onClick(){
    //console.warn(this.userName);
    //console.warn(this.password);
    if(this.userName.length > 0 && this.password.length > 0)
    {

      this.signUp();
      this.OnReset();
    }

  }

signUp(){
    var val={
    "userId":0,
    "userName":this.userName,
    "password":this.password,
    "userRole":"",
    "token":""
  }
  this.authService.signUp(val).subscribe({
    next: (res:any) =>{
      this.notificationService.showSuccess("<hr>" + res.Message,"Success!");
    },
    error: (err) => {
      this.notificationService.showError("<hr>" + err.error,"Error!");
    }
  })

  /*this.authService.signUp(val).subscribe((res:any) =>{
    if(JSON.parse(JSON.stringify(res)).Value === "Success")
    {
      this.notificationService.showSuccess("<hr>User Created Successfully!","Success!");

    }
    else
    {
      let msg:string=JSON.parse(JSON.stringify(res)).Value;

      this.notificationService.showError("<hr>" + msg + ", Unable to create user","Error!");
    }
  },(err:any) => {console.warn(err)});*/

}

  OnReset(){
    this.userName="";
    this.password="";

  }
}
