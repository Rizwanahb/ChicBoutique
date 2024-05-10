import { Component, OnInit } from '@angular/core';
import { WishlistService } from 'src/app/_services/wishlist.service';
import { WishlistItem } from 'src/app/_models/wishlistItem';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { CartService } from 'src/app/_services/cart.service';



@Component({
  selector: 'app-wishlist',
  templateUrl: './wishlist.component.html',
  styleUrls: ['./wishlist.component.css']
})
export class WishlistComponent implements OnInit {
  public wishlistItems: WishlistItem[] = [];  //property

  constructor(private wishlistService: WishlistService,
    private cartService: CartService, private router: Router,private authService: AuthService,
    )
  { }

  ngOnInit(): void {
    this.wishlistItems = this.wishlistService.getWishlist();
  }
  public wishlist = this.wishlistService.wishlist;
  removeItem(productId: number) {
    console.log(productId);
    if (confirm("are you sure to remove?")) {
      this.wishlistService.removeItemFromWishlist(productId);

    }

    window.location.reload();
  }

  addToCart(item:WishlistItem)
  {

    this.cartService.addToBasket({
      productId: item.productId,
      productTitle: item.productTitle,
      productPrice: item.productPrice,
      productImage:item.productImage,
      quantity: 1
    });
    this.wishlistService.removeItemFromWishlist(item.productId);
    window.location.reload();
  }

}
