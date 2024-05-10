import { Component, OnInit } from '@angular/core';
import { Category, Product } from 'src/app/_models/product';
import { ActivatedRoute, Router } from '@angular/router';
import { CartService } from 'src/app/_services/cart.service';
import { ProductService } from 'src/app/_services/product.service';
import { WishlistService } from 'src/app/_services/wishlist.service';
import { WishlistItem } from 'src/app/_models/wishlistItem';
import { Observable, firstValueFrom, of, throwError } from 'rxjs';
import { switchMap, take } from 'rxjs/operators';
import { CartItem } from 'src/app/_models/cartItem';
import { ReserveQuantity } from 'src/app/_models/reservequantity';
import { CookieService } from 'ngx-cookie-service';
import { AuthService } from 'src/app/_services/auth.service';




@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  productId: number =0;
  wishlist: number[] = [];
  category:Category ={ id: 0, categoryName: '' };
  public quantity:number=0;
  public totalItem:number=0;
  product:Product={id: 0, title:"", price:0, description:"",image:"", stock:0,categoryId:0, category:this.category }
  wishlistItem : WishlistItem= {productId: 0, productTitle:"",productImage:"",productDescription:"",productPrice:0}
  reserveQuantity:ReserveQuantity ={clientBasketId:"", productId:0, amountToReserve:0}
  clientbasketId: string=this.cookieService.get('VisitorID').toString();
  constructor(
    private productService:ProductService,
    private cartService:CartService,
    private route:ActivatedRoute,
     private wishlistService: WishlistService,
     private router:Router,
     private cookieService:CookieService,
     private authService:AuthService,
    )
     {
     
      }

     addedToWishlist: boolean = false;
     
  ngOnInit(): void {
    
    this.route.params.subscribe(params => {
      this.productId = +params['id'];
    });
    this.productService.getProductById(this.productId).subscribe(x=>
      { this.product=x,

      console.log("product stock: ",this.product.stock);
    });

  }
  
  async addToCart(product: Product): Promise<any> {
    try {
     

      const availableStock = await firstValueFrom(this.productService.getAvailableStock(product.id));
      console.log("AvailableStock",availableStock);
      const item: CartItem = {
        
        productId: product.id,
        productTitle: product.title,
        productPrice: product.price,
        productImage: product.image,
        quantity: this.quantity+1
      };
      if (item.quantity <=availableStock) {
        
        let reserveQuantity: ReserveQuantity = {           // this is an object which stores customer_id, all of the ordereditems details and date when these have been ordered
          clientBasketId: this.clientbasketId, //guid value which is saved in the cookie
          productId:item.productId,
          amountToReserve:item.quantity

        }
        var reserve=await firstValueFrom(this.productService.reserveStock(reserveQuantity));
       
        
        if(reserve==true)
        {
          this.cartService.addToBasket(item);
          this.cartService.saveBasket();
          window.location.reload();
        console.log("reserved stock")
        }

        else {console.log("cannot reserved stock")}
      } 
      else {
         alert('Not enough stock,choose another');
         this.router.navigate(['/']);
      }
    
  }
  catch (error) {
      console.error('Error adding to cart:', error);
      
    }
    
  }
 handleAddtoWishlist(product:Product)
 {
    this.wishlistService.addToWishlist({
      productId: product.id,
      productTitle: product.title,
      productImage:product.image,
      productDescription:product.description,
      productPrice:product.price

    });
    //window.location.reload();
    this.addedToWishlist = true;
    console.log("Product Id: "+ this.productId + "  is added to wishlist");

  }



  handleRemovefromWishlist(){
    this.wishlistService.removeItemFromWishlist(this.productId);

    this.addedToWishlist= false;

  }
 

}



