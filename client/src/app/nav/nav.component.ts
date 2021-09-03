import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.Service';
import {Router} from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any ={}
  currentUser$!: Observable<User>;
  loggedIn!: boolean;
  

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService ) { }

  ngOnInit(): void {
    this.currentUser$=this.accountService.currentUser$;
  }

  Login() {
    this.accountService.Login(this.model).subscribe(response =>
      {
        this.router.navigateByUrl('/members');
        console.log(response);
        this.loggedIn=true;
        
      }, error=> {
        console.log(error);
        this.toastr.error(error.error);
      })

   
  }

    logout() {

      this.accountService.logout();
      this.loggedIn=false; 
      this.router.navigateByUrl('/');
      
    }

    /*
    
    getCurrentUser() {
      this.accountService.currentUser$.subscribe(user => {
        this.loggedIn= !!user;
      }, error => {
        console.log(error);
      })
    }
    */
}
