
import { Component ,OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { CookieService } from 'ngx-cookie-service';
import { CartService } from './_services/cart.service';
import { AuthService } from './_services/auth.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  public visitorGuid : Guid=Guid.create();
  
  
  constructor( private route: ActivatedRoute, private cookieService:CookieService, 
    private cartService:CartService, private authservice:AuthService) {
    
    /*this.visitorGuid=this.cartService.cookieValue();
    this.cookieService.set('VisitorID', (this. visitorGuid).toString(), 15);
    const cookieValue = this.cookieService.get('VisitorID');*/
    
    
  }
  ngOnInit(): void {
    
    const cookieValue = this.cookieService.get('VisitorID');
    console.log('Cookie value:VisitorID::::',  cookieValue);
    const hasVisited = this.cookieService.check('VisitorID');

    // If the visitor has not visited before
    if (!hasVisited) {
      // Set the visited cookie
      
      this.cookieService.set('VisitorID', (this. visitorGuid).toString(), 15);
           
    }
    
  }
  // Determine if the current route is the frontpage route
  isFrontpageRoute(): boolean {
    // Check if the first child route exists and its routeConfig is not null
    return this.route.snapshot.firstChild?.routeConfig?.path === '';
  }

  isAdminpageRoute(): boolean {
    // Check if the first child route exists and its routeConfig is not null
    return this.route.snapshot.firstChild?.routeConfig?.path === 'admin-panel';
  }
  title(title: any) {
    throw new Error('Method not implemented.');
  }
}
