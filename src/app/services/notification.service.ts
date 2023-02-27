import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  constructor(private toastr: ToastrService) { }

  showSuccess(message:string, title:string){
      this.toastr.success(message,title,{
        enableHtml :  true,positionClass: 'toast-top-center'
      });
  }

  showInformation(message:string, title:string)
  {
    this.toastr.info(message,title,{
      enableHtml :  true,positionClass: 'toast-top-center'
    });
  }

  showWarning(message:string, title:string)
  {
    this.toastr.warning(message,title,{
      enableHtml :  true,positionClass: 'toast-top-center'
    });
  }

  showError(message:string, title:string)
  {
    this.toastr.error(message,title,{
      enableHtml :  true,positionClass: 'toast-top-center'
    });
  }

}
