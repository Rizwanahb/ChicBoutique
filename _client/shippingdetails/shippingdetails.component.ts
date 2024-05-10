import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ShippingDetails } from 'src/app/_models/shippingdetails';
import { User } from 'src/app/_models/user';
import { AuthService } from 'src/app/_services/auth.service';
import { CartService } from 'src/app/_services/cart.service';
import { OrderService } from 'src/app/_services/order.service';
import { ProductService } from 'src/app/_services/product.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-shippingdetails',
  templateUrl: './shippingdetails.component.html',
  styleUrls: ['./shippingdetails.component.css']
})
export class ShippingdetailsComponent implements OnInit {

 
  
  public shippingdetails: any = {};
  id: any;

  currentUser: User =
  { id: 0,
    email: '',
    firstName: '',
    lastName: '',
    password: '',
    address: '',
    city: '',
    country: '',
    postalcode: '',
    telephone: '',
    role: 2 };

  constructor( private orderService: OrderService,
    private cartService: CartService, private router: Router,
    private productService: ProductService,
    private userService:UserService,
    private authService:AuthService) { }

  ngOnInit(): void {
  

    }
 
    forshippingDetails: boolean = false;
    isGuest: boolean = false; 
  
    
  async submitShippingForm()
  {
    
        var guestEmail=sessionStorage.getItem('guestEmail');
        console.log("GuestEmail", guestEmail);
        if (this.isGuest || this.forshippingDetails) {
          // If it is guest, then it will ask for shippingdetails form
            console.log(this.shippingdetails);
            const addressData=this.shippingdetails
            this.orderService.setAddressData(addressData);
    
            this.router.navigate(['/payment']); 
        } 
      else{
            console.log("this is for users")
         this.authService.currentUser.subscribe(user => {
          if (this.currentUser) {
            this.userService.getUserbyEmail(user.email).subscribe(userData => {
              this.currentUser = userData;
              console.log("CurrentUser", this.currentUser)
               const addressData: ShippingDetails=
              {
                address: this.currentUser?.homeAddress?.address ?? '',
                city: this.currentUser?.homeAddress?.city ?? '',
                country: this.currentUser?.homeAddress?.country ?? '',
                postalCode: this.currentUser?.homeAddress?.postalCode?? '',
                phone: this.currentUser?.homeAddress?.phone ?? ''
              }
              this.orderService.setAddressData(addressData);
              this.router.navigate(['/payment']); 
      });
    }
    })
     
      //
      //this.orderService.setAddressData(addressData);
      
      
    }
   

    
  }

}
