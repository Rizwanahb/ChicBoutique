import { CartItem } from 'src/app/_models/cartItem';
import { Component, OnInit } from '@angular/core';
import { CartService } from 'src/app/_services/cart.service';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { Role } from 'src/app/_models/role';
import { firstValueFrom } from 'rxjs';
import { ProductService } from 'src/app/_services/product.service';
import { CookieService } from 'ngx-cookie-service';
import { ReserveQuantity } from 'src/app/_models/reservequantity';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  public quantity: number = 0;//variable declaration for Productquantity

  public grandTotal: number = 0;// variable declaration for storing total amount of the purchase
  public cartProducts: CartItem[] = [];  //property
  public basket = this.cartService.basket; //getting basket from the service
  public totalItem: number =0;
  clientbasketId: string=this.cookieService.get('VisitorID').toString();
  constructor(
    private cartService: CartService,
    private router: Router,
    private authService: AuthService,
    private productService:ProductService, private cookieService:CookieService) //dependency injection of different services
  { }

  ngOnInit(): void {
    this.cartProducts = this.cartService.getBasket(); //getting all chosen cartproducts of the customer
    this.grandTotal = this.cartService.getTotalPrice();//getting the total price of the items
  }


  async processOrder() //method is for processing order after clicking the BUY button
  {


    if (this.authService.currentUserValue == null || this.authService.currentUserValue.id == 0) {
      this.router.navigate(['checkout']);

    }
    else
    {
     alert
      this.router.navigate(['shippingdetails']);

    }

  }
  removeItem(productId: number) {
    console.log(productId);
    if (confirm("are you sure to remove?")) {
      this.cartService.removeItemFromBasket(productId);
      this.cartProducts = this.cartService.getBasket();

    }


  }

  emptycart() {
    if (confirm("are u sure to remove?"))
      this.cartService.clearBasket();

      this.cartProducts=[];
      window.location.reload;

  }

  // Method to increase the quantity
  async increaseQuantity(item: any): Promise<void> {

   let itemId;
   itemId = this.cartService.basket.findIndex(({ productId }) => productId == item.productId);


    const availableStock = await firstValueFrom(this.productService.getAvailableStock(item.productId));
    console.log("AvailableStock",availableStock);
      if(item.quantity<availableStock)
      {
        item.quantity = item.quantity + 1;
        this.basket[itemId].quantity = item.quantity;


        this.cartService.saveBasket4(this.basket);
        this.cartProducts = this.cartService.getBasket();
        this.grandTotal = this.cartService.getTotalPrice();

      }
      else
      {
        alert("This product is not in stock. please choose another");

         this.router.navigate(['/']);
      }
  }

  // Method to decrease the quantity, ensuring it doesn't go below 1
  decreaseQuantity(item: any): void {
      if (item.quantity > 1) {

      let itemId;
      itemId = this.cartService.basket.findIndex(({ productId }) => productId == item.productId);



      item.quantity = item.quantity - 1;
      this.basket[itemId].quantity = item.quantity;


      this.cartService.saveBasket4(this.basket);
      this.cartProducts = this.cartService.getBasket();
      this.grandTotal = this.cartService.getTotalPrice();

    }
    else {
      alert("Quantity can not be negative.")
    }
    this.cartService.saveBasket();
  }



}