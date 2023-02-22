import { Component } from "@angular/core";
import { SalarySlipService } from "src/app/salaryslip.service";
import { NotificationService } from "src/app/notification.service";

@Component({
  selector:'app-userlogin',
  templateUrl:'./userlogin.component.html',
  styleUrls:[]
})

export class UserLoginComponent{

  constructor(private salaryslipService: SalarySlipService, private notificationService : NotificationService){}

  userName:string="";
  password:string="";
  date:Date = new Date();

  onLogin(){

    console.warn(this.date);
    var val={
      "userId":0,
      "userName":this.userName,
      "password":this.password,
      "userRole":""
    }
    this.salaryslipService.onLogin(val).subscribe((res) => {
      if(JSON.parse(JSON.stringify(res)).Value == "Success"){
        this.notificationService.showSuccess("<hr> User login successfully!","Success!");
      }
      else
      {
        let msg:string = JSON.parse(JSON.stringify(res)).Value;
        this.notificationService.showError("<hr>" + msg + ", Unable to login","Error!");
      }

    },(err) => {console.warn(err)});
  }

  onCancel(){

  }

}
