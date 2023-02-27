import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http : HttpClient,private route:Router) { }

  signUp(val:any){
    return this.http.post(environment.apiUrl + "/Userlist/register",val);
  }

  onLogin(val:any){
    return(this.http.post(environment.apiUrl + "/UserLoginDetail/authenticate",val));
  }

  onLogout(){
    this.route.navigate(['userlogin']);
    localStorage.clear();

  }

  storeToken(tokenValue:string){
    localStorage.setItem('token',tokenValue);
  }

  getToken(){
    return localStorage.getItem('token');
  }

  isLoggedIn():boolean{
    return !!localStorage.getItem('token');
  }
}
