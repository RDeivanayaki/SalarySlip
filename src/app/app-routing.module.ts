import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UploadExcelComponent } from './upload/upload-excel/upload-excel.component'
import { SignupComponent } from './user/signup/signup.component';
import { PasswordChangeComponent } from './user/passwordchange/passwordchange.component';
import { UserLoginComponent } from './user/userlogin/userlogin.component';
import { SalarySlipComponent } from './salaryslip/salaryslip.component';

const routes: Routes = [
  { path: '', component: SignupComponent, pathMatch: 'full' },
  { path: 'home',component: SignupComponent},
  { path: 'userlogin',component: UserLoginComponent },
  { path: 'passwordchange',component:PasswordChangeComponent },
  { path: 'salaryslip',component: UploadExcelComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
