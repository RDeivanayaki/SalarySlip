import { Component, OnInit } from '@angular/core';
import { SalarySlipService } from 'src/app/salaryslip.service';
import { NotificationService } from 'src/app/notification.service';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-upload-excel',
  templateUrl: './upload-excel.component.html',
  styleUrls: []
})
export class UploadExcelComponent implements OnInit{

  constructor(private salaryslipService:SalarySlipService,private notificationService:NotificationService)  {}

  monthList : any = []; //for months
  yearList : any = []; //for years
  isGenerating:boolean=false;

  month:string ="";
  year:string ="";
  uploadedFilename:string="";

  ngOnInit(): void {
    //this.getMonths(); //Need to enable later from some other component
    //this.getYears();
  }

  getMonths(): void {
    this.salaryslipService.getMonths()
    .subscribe(months => {

      this.monthList = months;

    });
  }

  getYears(): void {
    this.salaryslipService.getYears()
    .subscribe(years => {

        this.yearList = years;
      });
    }

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

        },(err) => {console.warn(err)});
      }
  }

  }

  OnClick():void{
    if(this.uploadedFilename.length > 0 && this.month.length > 0 && this.year.length > 0)
    {
      this.isGenerating=true;
      this.addSalary();
    }
    else
    {
      //console.warn("Uploaded file name : " + this.uploadedFilename);
      this.notificationService.showInformation("<hr>Please enter all values","Information!");
    }
  }

  addSalary(){

    var val = {
      "month":this.month.trim(),
      "year":this.year,
      "uploadedFilename":this.uploadedFilename,
    }

    this.salaryslipService.addsalaryDetail(val).subscribe((res:any)  => {

      if(JSON.parse(JSON.stringify(res)).Value === "Added Successfully!")
      {
        this.notificationService.showSuccess("<hr>Salary Slip Generated Successfully!","Success!");
        this.OnReset();
      }
      else
      {
        let msg:string=JSON.parse(JSON.stringify(res)).Value;
        this.notificationService.showError("<hr>" + msg + ", Unable to create Salary Slip","Error!");
      }

  },(err) => {console.warn(err)});

  }

  OnReset(): void{
    this.month ="";
    this.year ="";
    this.uploadedFilename="";
    this.isGenerating = false;
    this.getMonths();
    this.getYears();
  }

}
