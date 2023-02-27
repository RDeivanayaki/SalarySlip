import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'SalarySlip';
  opened=false;

  constructor(private auth:AuthService,private route:Router){}
  onHome(){
    this.route.navigate(['userlogin']);
  }
  onLogout(){
    this.auth.onLogout();
  }
}
