import { Component, OnInit, NgZone } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CartItem } from 'src/app/_models/cartItem';
import { Order } from 'src/app/_models/order';
import { OrderResponse } from 'src/app/_models/orderResponse';
import { ShippingDetails } from 'src/app/_models/shippingdetails';
import { AuthService } from 'src/app/_services/auth.service';
import { OrderService } from 'src/app/_services/order.service';


@Component({
  selector: 'app-thankyou',
  templateUrl: './thankyou.component.html',
  styleUrls: ['./thankyou.component.css']
})
export class ThankyouComponent implements OnInit {

  public result:any;
  
  constructor(private orderService:OrderService, private authService:AuthService, private route:ActivatedRoute) { }
  isShown: boolean = false ;
  public orderResponse: OrderResponse = {
    id: 0,
    accountInfoId: " ",
    amount: 0,
    transactionId: '',
    status: '',
    paymentMethod:'',
    accountInfo:{
      id: "",
      createdDate: '',
      userId: 0
    },
    orderDetails: [],
    shippingDetails: {
      address: '',
      city: '',

      postalCode: '',
      country: '',
      phone: ''
    }
    
  } ;
  public orderDetails: Array<CartItem> = [];
  public shippingdetails: ShippingDetails = {address: '',
  city: '',

  postalCode: '',
  country: '',
  phone: ''}

  orderId:number=0; 
  localAccountId:string ="";
  ngOnInit(){
   
   /* this.orderId = parseInt(this.route.snapshot.paramMap.get('id')||'0');
   console.log("OrderId",this.orderId);
    this.orderService.getOrderDetailsByOrderId(this.orderId).subscribe(res => {
      this.orderResponse = res;
      console.log("response", this.orderResponse)
    });
    
  */
    
  }
  
   detail(){

    //this.orderId = parseInt(this.route.snapshot.paramMap.get('id')||'0');
    this.route.queryParams.subscribe(params => {
      this.orderId = parseInt(params['id']) || 0;
    });
   console.log("OrderId",this.orderId)
   
    this.orderService.getOrderDetailsByOrderId(this.orderId).subscribe(res => {
      this.orderResponse = res;
      console.log("response", this.orderResponse)
     
     if(this.orderResponse.id===this.orderId)
     {
      console.log(this.orderResponse.orderDetails)
      console.log("responseID", this.orderResponse.id)
      this.isShown = true;
    }

    
  });
    
  }

}
