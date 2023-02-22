import { Component } from "@angular/core";
import { SalarySlipService } from "src/app/salaryslip.service";
import { NotificationService } from "src/app/notification.service";

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls:[]
}
)

export class SignupComponent{

  constructor(private salaryslipService: SalarySlipService,private notificationService: NotificationService){

  }
  islogin:boolean=false;
  userName="";
  password="";

  onClick(){
    //console.warn(this.userName);
    //console.warn(this.password);
    if(this.userName.length > 0 && this.password.length > 0)
    {
      this.islogin=true;
      this.addUser();
      this.OnReset();
    }

  }

addUser(){
    var val={
    "userId":0,
    "userName":this.userName,
    "password":this.password,
    "userRole":""
  }

  this.salaryslipService.addUser(val).subscribe((res:any) =>{
    if(JSON.parse(JSON.stringify(res)).Value === "Success")
    {
      this.notificationService.showSuccess("<hr>User Created Successfully!","Success!");

    }
    else
    {
      let msg:string=JSON.parse(JSON.stringify(res)).Value;

      this.notificationService.showError("<hr>" + msg + ", Unable to create user","Error!");
    }
  },(err) => {console.warn(err)});

}

  onCancel(){
    this.OnReset();
  }

  OnReset(){
    this.userName="";
    this.password="";
    this.islogin=false;
  }
}
