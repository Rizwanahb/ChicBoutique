import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './_helpers/auth.guard';
import { FrontpageComponent } from './_client/frontpage/frontpage.component';
import { NavbarComponent } from './_client/navbar/navbar.component';
import { CategoryProductsComponent } from './_client/category-products/category-products.component';
import { ProductDetailsComponent } from './_client/product-details/product-details.component';
import { WishlistComponent } from './_client/wishlist/wishlist.component';
import { CartComponent } from './_client/cart/cart.component';
import { LoginComponent } from './_client/login/login.component';
import { RegisterComponent } from './_client/register/register.component';
import { AdminCategoryComponent } from './_admin/admin-category/admin-category.component';
import { AdminUserComponent } from './_admin/admin-user/admin-user.component';
import { FormsModule } from '@angular/forms';
import { AdminProductComponent } from './_admin/admin-product/admin-product.component';
import{ AdminPanelComponent } from './_admin/admin-panel/admin-panel.component';
import { ProfileComponent } from './_client/profile/profile.component';
import { GuestComponent } from './_client/guest/guest.component';
import { CheckOutComponent } from './_client/check-out/check-out.component';
import { ShippingdetailsComponent } from './_client/shippingdetails/shippingdetails.component';
import { ThankyouComponent } from './_client/thankyou/thankyou.component';
import { PaymentComponent } from './_client/payment/payment.component';

const adminRoutes: Routes = [
  { path: 'admin-panel', component: AdminPanelComponent,canActivate: [AuthGuard] },
  { path: 'admin/product', component: AdminProductComponent,  canActivate: [AuthGuard]  },
  { path: 'admin/category', component: AdminCategoryComponent,  canActivate: [AuthGuard]  },
  { path: 'admin/user', component: AdminUserComponent,  canActivate: [AuthGuard]  },
];

const clientRoutes: Routes = [
  { path: '', component: NavbarComponent },
  { path: 'frontpage', component: FrontpageComponent},
  { path: 'category_products/:id', component: CategoryProductsComponent },
  { path: 'product_details/:id', component: ProductDetailsComponent},
  { path: 'cart', component: CartComponent },
  {path:  'wishlist',component:WishlistComponent},
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'profile', component: ProfileComponent },
  { path: 'guest', component: GuestComponent },
  { path: 'checkout', component: CheckOutComponent },
  {path: 'shippingdetails', component: ShippingdetailsComponent },
  { path: 'thankyou', component:ThankyouComponent},
  { path: 'payment', component: PaymentComponent,  canActivate: [AuthGuard]  }
];

@NgModule({
 imports: [
  RouterModule.forRoot(adminRoutes, { useHash: true }), // Admin routes
  RouterModule.forRoot(clientRoutes, { useHash: true }), // Client routes
  FormsModule],
  exports: [RouterModule]
})
export class AppRoutingModule { }
