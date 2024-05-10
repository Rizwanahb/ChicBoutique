import { CategoryService } from 'src/app/_services/category.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Category } from 'src/app/_models/category';
import { CartService } from 'src/app/_services/cart.service';
import { User } from "src/app/_models/user";
import { AuthService } from 'src/app/_services/auth.service';
import { Role } from 'src/app/_models/role';
import { UserService } from 'src/app/_services/user.service';
import { WishlistService } from 'src/app/_services/wishlist.service';
import { SearchService } from 'src/app/_services/search.service';
import { Product } from 'src/app/_models/product';
import { ProductService } from 'src/app/_services/product.service';






@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  title = 'ChicBotiqueClient';
  categories: Category[] = [];
  category: Category = { id: 0, categoryName: "" };
  currentUser: User = {
    id: 0, firstName: '', lastName: '', email: '', password: '', role: 2,
    address: '',
    telephone: '',
    city: '',
    postalcode: '',
    country: ''
  };
  allRoles = Role;
  // currentUserRole = 2;
  categoryId: number = 0;
  public searchTerm: string = "";

  products: Product[]=[];

  public totalItem: number = this.cartService.getBasket().length;
  //public totalItem: number = 0;
  public WL_totalItem: number = this.wishlistService.getWishlist().length;

    showSearchResults: boolean = false;
  x: any;
  isAdmin = false;
  isHovered = false;

  constructor(
    private router: Router,
    private authService: AuthService,
    private productService:ProductService,
    private categoryService: CategoryService,
    private cartService: CartService,
    private userService: UserService,
    private wishlistService: WishlistService,
    private searchService: SearchService,
   
  ) {
    this.searchService.search.subscribe((term) => {
      this.searchTerm = term;
      // Perform search or update your UI as needed when the search term changes
    });

  }

  ngOnInit(): void {
    
    /*this.cartService.getBasket()
    .pipe(
      map(basket => basket.length) // Transform array of cart items into count of items
    )
    .subscribe(totalItems => {
      this.totalItem = totalItems;
    });*/
    this.categoryService.getCategoriesWithoutProducts().subscribe(x => this.categories = x);
    console.log('value received ');
    this.authService.currentUser.subscribe(x => {
      this.currentUser = x;
      //this.currentUserRole = x.role;
      //this.isAdmin = this.checkIfUserAdmin(x)
      this.router.navigate(['/']);
    });

    console.log("x:", this.currentUser.role);
  }

  public checkIfUserAdmin(user: User | null) {
    //check role type and return true or false
    if (user != null) {
      return user.role == Role.Admin;
    }

    else
      return false;
  }

  public checkIfUserMember(user: User | null) {
    //check role type and return true or false
    if (user != null) {
      return user.role == Role.Member;
    }

    else
      return false;
  }
  /// Seach Funtionlity //
  onSearch(event: any) {
    this.searchTerm = (event.target as HTMLInputElement).value;
    console.log(this.searchTerm);
    this.searchService.search.next(this.searchTerm);
  }

  /*
  public checkIfUserNotGuest(user:User | null) {
    //check role type and return true or false
    if (user != null) {
      return user.role == Role.Admin || user.role == Role.Member
    }
    else
    return false;
  }
  public checkIfUserMember(user:User | null) {
    //check role type and return true or false
    if (user != null) {
      return user.role == Role.Member
    }
    else
    return false;
  }*/





  logout() {
    if (confirm('Are you sure you want to log out?')) {
      this.userService.getRole_(2);
      // ask authentication service to perform logout
      this.authService.logout();

      // subscribe to the changes in currentUser, and load Home component
      this.authService.currentUser.subscribe(x => {
        this.currentUser = x;
        this.router.navigate(['login']);
      });
    }
    else {
      if (this.x === 0) {
        this.router.navigate(['admin-panel']);
      }
      else {
        this.router.navigate(['/']);
      }
    }
  }

}
