import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SalarySlipService {

  //readonly appUrl = "http://localhost:5048";

  download:boolean=false;
  constructor(private http : HttpClient){}

  getMonths(): Observable<any[]> {
    return this.http.get<any[]>(environment.apiUrl + '/Monthlist')
  }

  getYears(): Observable<any[]> {
    return this.http.get<any[]>(environment.apiUrl + '/Yearlist')
  }

 fileUpload(val:any){
    return this.http.post(environment.apiUrl + "/MonthlySalaryDetail/Upload",val)
  }

  addsalaryDetail(val:any){
    return this.http.post(environment.apiUrl + '/SalaryForMonthYear',val);
}

downloadpdf(){
  return this.http.get(environment.apiUrl + "/SalaryForMonthYear/generatepdf",
  {observe:'response',responseType:'blob'})
}




}
