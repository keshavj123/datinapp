import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.Service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any ={}
  currentUser$!: Observable<User>;
  loggedIn!: boolean;
  

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    this.currentUser$=this.accountService.currentUser$;
  }

  Login() {
    this.accountService.Login(this.model).subscribe(response =>
      {
        console.log(response);
        this.loggedIn=true;
        
      }, error=> {
        console.log(error);
      })

   
  }

    logout() {

      this.accountService.logout();
      this.loggedIn=false; 
      
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
