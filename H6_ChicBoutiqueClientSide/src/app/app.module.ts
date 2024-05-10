import { CategoryProductsComponent } from './_client/category-products/category-products.component';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule} from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FrontpageComponent } from './_client/frontpage/frontpage.component';
import { SortPipe } from './_pipes/sort.pipe';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { NavbarComponent } from './_client/navbar/navbar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductDetailsComponent } from './_client/product-details/product-details.component';
import { WishlistComponent } from './_client/wishlist/wishlist.component';
import { CartComponent } from './_client/cart/cart.component';
import { RegisterComponent } from './_client/register/register.component';
import { LoginComponent } from './_client/login/login.component';
import { AdminPanelComponent } from './_admin/admin-panel/admin-panel.component';
import { AdminProductComponent } from './_admin/admin-product/admin-product.component';
import { AdminCategoryComponent } from './_admin/admin-category/admin-category.component';
import { AdminUserComponent } from './_admin/admin-user/admin-user.component';
import { AuthGuard } from './_helpers/auth.guard';
import { ThankyouComponent } from './_client/thankyou/thankyou.component';
import { CheckOutComponent } from './_client/check-out/check-out.component';
import { PaymentComponent } from './_client/payment/payment.component';
import { ShippingdetailsComponent } from './_client/shippingdetails/shippingdetails.component';
import { ProfileComponent } from './_client/profile/profile.component';
import { GuestComponent } from './_client/guest/guest.component';
import { NgxPayPalModule } from 'ngx-paypal';
import { CookieService } from 'ngx-cookie-service';
import { FilterByUserIdPipe } from './_pipes/filterByUserId.pipe';


@NgModule({
  declarations: [
    AppComponent,
    FrontpageComponent,
    SortPipe,
    FilterByUserIdPipe,
    NavbarComponent,
    CategoryProductsComponent,
    ProductDetailsComponent,
    CartComponent,
    WishlistComponent,
    LoginComponent,
    RegisterComponent,
    AdminProductComponent,
    AdminPanelComponent,
    AdminCategoryComponent,
    AdminUserComponent,
    ThankyouComponent,
    CheckOutComponent,
    PaymentComponent,
    ShippingdetailsComponent,
    ProfileComponent,
    GuestComponent,






  ],

  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    NgxPayPalModule
  ],
  providers: [AuthGuard,CookieService],
  schemas: [CUSTOM_ELEMENTS_SCHEMA], // Add CUSTOM_ELEMENTS_SCHEMA
  bootstrap: [AppComponent]
})
export class AppModule { }
