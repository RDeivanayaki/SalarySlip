import { Component,OnInit, ViewChild } from "@angular/core";
import { SalarySlipService } from "../services/salaryslip.service";
import { NotificationService } from "../services/notification.service";
import { UploadExcelComponent } from "../upload/upload-excel/upload-excel.component";

@Component({
  selector:'app-salaryslip',
  templateUrl:'./salaryslip.component.html',
  styleUrls:[]
 })


export class SalarySlipComponent implements OnInit{

  constructor(private salaryslipService:SalarySlipService, private notificationService: NotificationService){}
  uploadedFilename:string;
  receiveMessage($event:string){
    this.uploadedFilename = $event;
    console.log("receiveMessage is called " + this.uploadedFilename);
  }

  @ViewChild(UploadExcelComponent) childComponent: UploadExcelComponent;

  monthList : any = []; //for months
  yearList : any = []; //for years
  isGenerating:boolean=false;

  month:string ="";
  year:string ="";

  ngOnInit(): void {
    this.getMonths(); //Need to enable later from some other component
    this.getYears();
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

    this.childComponent.onReset();
  }
}
