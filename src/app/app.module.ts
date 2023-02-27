import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UploadExcelComponent } from './upload/upload-excel/upload-excel.component';
import { SalarySlipService } from './services/salaryslip.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { DownloadPdfComponent } from './download/download-pdf/download-pdf.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule} from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule} from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { ToastrModule } from 'ngx-toastr';
import { SignupComponent } from './user/signup/signup.component';
import { UserLoginComponent } from './user/userlogin/userlogin.component';
import { PasswordChangeComponent } from './user/passwordchange/passwordchange.component';
import { SalarySlipComponent } from './salaryslip/salaryslip.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { TokenInterceptor } from './interceptors/token.interceptor';


@NgModule({
  declarations: [
    AppComponent,
    UploadExcelComponent,
    DownloadPdfComponent,
    SignupComponent,
    UserLoginComponent,
    PasswordChangeComponent,
    SalarySlipComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    MatSidenavModule,
    MatListModule,
    MatProgressSpinnerModule,
    ToastrModule.forRoot()

  ],
  providers: [{
    provide:HTTP_INTERCEPTORS,
    useClass:TokenInterceptor,
    multi:true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
