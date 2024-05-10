import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/_models/user';
import { CartService } from 'src/app/_services/cart.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-guest',
  templateUrl: './guest.component.html',
  styleUrls: ['./guest.component.css']
})
export class GuestComponent implements OnInit {

  users: User[] = [];
  user: User = this.newUser();
  message: string[] = [];
  error = '';
 

  

  constructor(
    private userService: UserService,
    private router: Router,
    private cartService:CartService
   
  ) { }

  ngOnInit(): void {
    this.getUsers();
  }

  newUser(): User {
    return { id: 0, email: '',  firstName: '', lastName: '',   address:'', city:'', country: '', postalcode:'',telephone:'',role:2};
  }

  getUsers(): void {
    this.userService.getUsers()
      .subscribe((a: User[]) => this.users = a);
  }

  cancel(): void {
    this.message = [];
    this.user = this.newUser();
    this.router.navigate(['/']);
  }

   save(): void {
    this.message = [];

    if (this.user.email == '') {
      this.message.push('Email field cannot be empty');
    }
   
    if (this.user.firstName == '') {
      this.message.push('Enter Username');
    }
    if (this.user.lastName == '') {
      this.message.push('Enter Lastname');
    }
    if (this.user.password == '') {
      this.message.push('Password field cannot be empty');
    }
   if (this.user.address == '') {
      this.message.push('Enter Address');
    }
    if (this.user.city == '') {
      this.message.push('Enter City');
    }
    if (this.user.country == '') {
      this.message.push('Enter Country');
    }
    if (this.user.postalcode == '') {
      this.message.push('Enter Postalcode');
    }
    if (this.user.telephone == '') {
      this.message.push('Enter Telephone');
    }
  
  
    if (this.message.length == 0) {
      if (this.user.id == 0) {
        this.userService.guest_register(this.user)
          .subscribe({
            next: async (a: User) => {
            this.users.push(a);
            sessionStorage.setItem('guestEmail', a.email.toString());
            this.router.navigate(['shippingdetails']);
            //var result = await this.cartService.addOrder();
            /*this.cartService.clearBasket(); 
            Swal.fire({
              title: 'Success!',
              text: 'Order send successfully!',
              icon: 'success',
              confirmButtonText: 'Continue'
            });
            console.log('Order send successfully!')
            alert('Thanks for giving the informations and choosing us!'); */
            //this.router.navigate(['/thankyou/']);
           
           },
           error: (err: any)=>{
                        alert("User already exists!");
          }
        });
      } 
  }}



}
