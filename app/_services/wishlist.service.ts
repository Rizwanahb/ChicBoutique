import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { WishlistItem } from '../_models/wishlistItem';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class WishlistService {

  apiUrl = environment.apiUrl + '/wishlist';

  private wishlistName = "WebShopProjectWishlist";
  public wishlist: WishlistItem[] = [];
  public search = new BehaviorSubject<string>("");
  id:number=0;
  email:any; 
  user: Observable<User[]> | undefined;
  

  constructor(private router: Router,private authService: AuthService,private userService: UserService)
  { } 

  
  getWishlist(): WishlistItem[] {
    this.wishlist = JSON.parse(localStorage.getItem(this.wishlistName) || "[]");   
    return this.wishlist;
  }
  
  saveWishlist(): void {
    localStorage.setItem(this.wishlistName, JSON.stringify(this.wishlist));
  }
  
  
  
  addToWishlist(item: WishlistItem): void {
    this.getWishlist();
    let productFound = false; 

    if (!productFound) {
      this.wishlist.push(item);
    }
    this.saveWishlist();
  }


  clearWishList(): WishlistItem[] {
    this.getWishlist();
    this.wishlist = [];
    this.saveWishlist();
    return this.wishlist;
  }
  

  removeItemFromWishlist(productId: number): void {
    this.getWishlist();
    for (let i = 0; i < this.wishlist.length; i += 1) {
      if (this.wishlist[i].productId === productId) {

        this.wishlist.splice(i, 1);


      }
  }
  this.saveWishlist();

  
  
  }
}
