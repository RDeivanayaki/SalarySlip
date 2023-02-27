import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignupComponent } from './user/signup/signup.component';
import { PasswordChangeComponent } from './user/passwordchange/passwordchange.component';
import { UserLoginComponent } from './user/userlogin/userlogin.component';
import { SalarySlipComponent } from './salaryslip/salaryslip.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', redirectTo: 'userlogin', pathMatch: 'full' },
  { path: 'userlogin',component: UserLoginComponent },
  { path: 'signup',component: SignupComponent},
  { path: 'passwordchange',component:PasswordChangeComponent },
  { path: 'salaryslip',component: SalarySlipComponent, canActivate:[AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
