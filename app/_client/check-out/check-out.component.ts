import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/_services/auth.service';
import { CartService } from 'src/app/_services/cart.service';
import { OrderService } from 'src/app/_services/order.service';


@Component({
  selector: 'app-check-out',
  templateUrl: './check-out.component.html',
  styleUrls: ['./check-out.component.css']
})
export class CheckOutComponent implements OnInit {

  constructor( private authService: AuthService, private orderService:OrderService, private cartService:CartService,
    ) { }
  currentUser=this.authService.currentUser;
  public shippingdetails: any = {};
  public totalAmount: any = {};
 
   ngOnInit(): void {
     this. shippingdetails= this.orderService.getAddressData();
     
     this.totalAmount=this.cartService.getTotalPrice();
   }

}
