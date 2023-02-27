import { Component, Output,EventEmitter, ElementRef } from '@angular/core';
import { SalarySlipService } from 'src/app/services/salaryslip.service';
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-upload-excel',
  templateUrl: './upload-excel.component.html',
  styleUrls: []
})
export class UploadExcelComponent{

  constructor(private salaryslipService:SalarySlipService,private notificationService:NotificationService)  {}
    uploadedFilename:string="";
    inputVar:ElementRef;
  @Output() eventEmitter = new EventEmitter<string>()

    onFileUpload(event: any) {
      const file:File = event.target.files[0];
      if(file)
      {
        //console.warn("Filename is " + file.name);
        if((event.target.files[0].type !== "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
        && (event.target.files[0].type !== "application/vnd.ms-excel"))
        {
          this.notificationService.showInformation("<hr>Please select excel file type(.xls|.xlsx)","Information!");
        }
        else
        {
          const formData:FormData=new FormData();
          formData.append('uploadedFile',file,file.name);

          this.salaryslipService.fileUpload(formData).subscribe((data:any) =>{
            this.uploadedFilename = data.toString();
            this.eventEmitter.emit(file.name);
            console.log(file.name);
          },(err) => {console.warn(err)});
        }
        event.target.value = ''; //This is for Google chrome and Fire Fox browsers due to second time file click is not fired, so reset the variable manually
      this.inputVar.nativeElement.value=''; //This is for Microsoft Edge browser due to second time file click is not fired, so reset the variable manually
    }

    }

    onReset(){

      this.uploadedFilename = "";
    }

}
