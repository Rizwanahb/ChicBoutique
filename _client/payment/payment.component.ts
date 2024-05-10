import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ICreateOrderRequest, IPayPalConfig } from 'ngx-paypal';
import { Observable, of } from 'rxjs';
import { Order } from 'src/app/_models/order';
import { ShippingDetails } from 'src/app/_models/shippingdetails';
import { CartService } from 'src/app/_services/cart.service';
import { OrderService } from 'src/app/_services/order.service';
import { PaymentService } from 'src/app/_services/payment.service';
import { ProductService } from 'src/app/_services/product.service';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit {


  cartTotal =0;
 public payPalConfig?: IPayPalConfig;
  public clientBasketId:string =this.cookieService.get('VisitorID').toString();
  showSuccess!: any;
  order:Order = {
    id: 0,
    accountInfoId: '',
    clientBasketId:'',
    shippingDetails: {
      address: "",
      city: '',
      country: '',
      postalCode: '',
      phone: ''
    },

    orderDetails: [],
    amount: 0,
    transactionId: '',
    status: '',
    paymentMethod:''
  }

  //@ViewChild('paymentRef', {static : true}) paymentRef!: ElementRef;
  shippingdetails: any;

  id: any;
  trasactionId:  any;
 paymentStatus:any;
 paymentMethod:any;
  constructor(private router: Router, private cartService:CartService, 
    private orderService:OrderService, private paymentService: PaymentService,
    private productService:ProductService, private cookieService:CookieService,
    ) { }

  ngOnInit(): void {
    this.cartTotal= this.cartService.getTotalPrice();
    this.shippingdetails=this.orderService.getAddressData()

    console.log("Shipping Address",this.shippingdetails)
  localStorage.getItem('Cart Total') as any;
    console.log(this.cartTotal);
    console.log();

    this.initConfig();
  }

  private initConfig(): void {


      this.payPalConfig = {
        currency: 'DKK',
        clientId: `${environment.Client_Id}`,

        createOrderOnClient: (data) =>

          <ICreateOrderRequest>{


            intent: 'CAPTURE',
            purchase_units: [
              {
                amount: {
                  currency_code: 'DKK',
                  value: `${this.cartTotal}`,
                }
              },
            ],
          },
        onApprove: (data, actions) => {
          var test:any = data;
          console.log(
            test.paymentSource,
            'onApprove - transaction was approved',
            data,
            actions
          );
          this.trasactionId=this.orderService.setTransactionId(data.orderID);

          actions.order.get().then(async (details: any) => {
            this.orderService.getAddressData();
            console.log(details);
            this.paymentStatus=this.orderService.setPaymentStatus(details.status);
            this.paymentMethod = this.orderService.setPaymentMethod(test.paymentSource);
            var result = await this.cartService.addOrder();  //this is creating order in our system
              this.id =result.id;
              console.log('result', result);
              this.cartService.clearBasket();
              this.router.navigate(['thankyou'], { queryParams: { id: this.id } });
              
              //window.location.reload;
              
          });
        },

        onClientAuthorization: (data) => {

        },
        onCancel: (data, actions) => {
          console.log('OnCancel', data, actions);
          this.router.navigate(['cart'])

        },
        onError: (err) => {
          console.log('Try Again', err);
          alert("Try Again")
          this.router.navigate(['cart']);
        },

      };

    }
    cancel(){
      this.router.navigate(['cart']);
    }

}
