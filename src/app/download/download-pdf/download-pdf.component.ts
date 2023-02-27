import { Component, OnInit } from "@angular/core";
import { SalarySlipService } from "src/app/services/salaryslip.service";
import { NotificationService } from 'src/app/services/notification.service';

@Component({
  selector: 'app-download-pdf',
  templateUrl: './download-pdf.component.html',
  styleUrls: []
})

export class DownloadPdfComponent implements OnInit {

  errorMessage:String="";
  constructor(private salaryslipService:SalarySlipService,private notificationService:NotificationService){}

  ngOnInit(): void {}

  download(){
    this.salaryslipService.downloadpdf()
    .subscribe(response =>
      {

        let filename:string=response.headers.get('content-disposition')
        ?.split(';')[1].split('=')[1] as string;
        //console.warn("filename : " + filename);
        if(typeof(filename) !== "undefined")
       {
        let blob:Blob=response.body as Blob;

        const anchor = document.createElement('a');

        anchor.download=filename.trim();
        anchor.href=window.URL.createObjectURL(blob);
        anchor.click();
       }
       else
       {
        this.notificationService.showError("<hr>No data found!, please upload an excel file","Error!");
      }

      },(err) => {console.warn(err)});
  }
}
